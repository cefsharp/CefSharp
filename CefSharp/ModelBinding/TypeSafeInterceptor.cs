using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// When using this interceptor .NET methods can continue to use .NET only types like <see cref="Guid"/> and <see cref="Tuple"/> both as parameters and return values.<br/>
    /// Asynchronous <see cref="Task{TResult}"/> are also supported and can be bound to the window to allow support for promises without <see cref="CefSharpSettings.ConcurrentTaskExecution"/><br/>
    /// The Javascript side will get back values it can use without forcing boilerplate on users.
    /// </summary>
    public class TypeSafeInterceptor : IMethodInterceptor
    {
        public bool IsAsynchronous => true;

        /// <summary>
        /// Interception starts when we detect Javascript has attempted to invoke a .NET method.
        /// Now we evaluate the method and return what we deem to be the correct result for Javascript.
        /// </summary>
        /// <param name="method">the .NET method that will be invoked</param>
        /// <param name="parameters">all of the input parameters</param>
        /// <param name="methodName">the name of the method</param>
        /// <returns>The serialized return value of the executed method</returns>
        public async Task<object> InterceptAsync(Func<object[], object> method, object[] parameters, string methodName)
        {
            // execute the method using the provided parameters
            var returnValue = method(parameters);
            // there is no return value, so return immediately. 
            if (returnValue == null)
            {
                return null;
            }
            // serialize the method's return value
            return await Intercept(returnValue).ConfigureAwait(false);
        }

        /// <summary>
        /// Not used.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public object Intercept(Func<object[], object> method, object[] parameters, string methodName)
        {
            throw new NotImplementedException("To use the TypeSafeInterceptor please set IsAsynchronous to true");
        }

        /// <summary>
        /// Performs dynamic analysis on a given input value and determines the best serialization to apply.
        /// </summary>
        /// <param name="value">the value the analyze</param>
        /// <returns>the serialized value</returns>
        public static async Task<object> Intercept(object value)
        {
            var returnType = value.GetType();
            // the .NET method didn't return an asynchronous Task so we can just serialize and return.
            if (!returnType.IsGenericType || returnType.GetGenericTypeDefinition() != typeof(Task<>))
            {
                return SerializeObject(value);
            }
            // our anonymous method is an asynchronous Task. Run it and await results.
            var result = await ConvertTask(value as Task).ConfigureAwait(false);
            if (result == null)
            {
                return null;
            }
            // try to serialize the Task result. If the result of the Task was another Task this will catch it.
            return SerializeObject(result);
        }

        /// <summary>
        /// Converts an Object into a runnable <see cref="Task"/>.
        /// </summary>
        /// <param name="task">the task that will be executed via reflection.</param>
        /// <returns>the <paramref name="task"/> results if any.</returns>
        private static async Task<object> ConvertTask(Task task)
        {
            // run the task
            await task.ConfigureAwait(false);
            var voidTaskType = typeof(Task<>).MakeGenericType(Type.GetType("System.Threading.Tasks.VoidTaskResult"));
            // if the task is a void, we can just return.
            if (voidTaskType.IsInstanceOfType(task))
            {
                return null;
            }
            // now use reflection to get the results 
            var property = task.GetType().GetProperty("Result", BindingFlags.Public | BindingFlags.Instance);
            // if there are none we return
            if (property == null)
            {
                return null;
            }
            // grab the actual current return value from memory
            return property.GetValue(task);
        }

        /// <summary>
        /// Serializes a .NET object into the type-safe Javascript object.
        /// </summary>
        /// <param name="value">the .NET value that needs to be converted.</param>
        /// <returns>A Javascript ready object.</returns>
        /// <remarks>
        /// It's dictionaries all the way down
        /// </remarks>
        private static object SerializeObject(object value)
        {
            if (value == null)
                return null;
            var resultType = value.GetType();
            // check if the incoming value needs special care
            // this would be a lot cleaner with C# 8.0 pattern matching

            // serialize the Guid to a Javascript safe object (string)
            if (value is Guid guid)
                return SerializeGuid(guid);
            if (value is Version version)
                return SerializeVersion(version);
            // serialize the dictionary 
            if (value is IDictionary dict)
                return SerializeDictionary(dict);
            // serialize the list
            if (value is ICollection collection)
                return SerializeCollection(collection);
            // serialize tuples so they are usable
            if (resultType.IsValueTupleType())
                return SerializeTuple(value);
            // no conversion necessary other than a cast 
            if (resultType.IsEnum)
                return (int)value;
            // all primitive types should be fine as is
            if (value.IsNumber())
                return value;
            // a string doesn't require special conversion.
            if (value is string)
                return value;
            // nor does a boolean
            if (value is bool)
                return value;


            var typeName = resultType.FullName;
            // no type name, no pass.
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new TypeBindingException(resultType, null, BindingFailureCode.SourceNotAssignable);
            }
            // something has gone horribly wrong, no System types should be on returning objects.
            if (typeName.StartsWith("System."))
            {
                // throw and say it louder for the people in the back
                switch (typeName)
                {
                    case "System.Object":
                        throw new TypeBindingException(resultType, null, BindingFailureCode.UnsupportedJavascriptType, resultType.FullName, "Returning results must have a valid type to be serialized.");
                    case "System.Threading.Tasks.Task":
                    case "System.Threading.Tasks.VoidTaskResult":
                        throw new TypeBindingException(resultType, null, BindingFailureCode.UnsupportedJavascriptType, resultType.FullName, "Serialized child objects cannot have a TypeDefinition which inherits Task.");
                    case "System.Void":
                        throw new TypeBindingException(resultType, null, BindingFailureCode.UnsupportedJavascriptType, resultType.FullName, "Void is not a type that can be serialized.");
                    default:
                        throw new TypeBindingException(resultType, null, BindingFailureCode.UnsupportedJavascriptType, resultType.FullName, "System types cannot be serialized.");
                }
            }

            if (resultType.IsCustomStruct())
            {
                throw new TypeBindingException(resultType,
                    null, BindingFailureCode.UnsupportedJavascriptType,
                    resultType.FullName, "Structs are not supported as immutable fields cannot be guaranteed to stay unchanged if this structure is marshaled back to .NET.");
            }
            // if the underlying value isn't a .NET class or interface, return the value as it is.
            if (!resultType.IsClass && !resultType.IsInterface)
                return value;

            var javaScriptObject = new Dictionary<string, object>();
            // I Write Sins Not Tragedies
            // for .NET classes being returned to Javascript, don't allow them to have fields.
            // this avoids leaking data and exposing the backing fields for public properties. It also forces bound objects to practice good hygiene 
            var fields = value.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            if (fields.Length > 0)
            {
                throw new TypeBindingException(resultType,
                    null, BindingFailureCode.UnsupportedJavascriptType,
                    resultType.FullName, "Cannot be serialized because it has public fields. " +
                    "To avoid data leakage, please make the fields private and create a property that uses the underlying field.");
            }

            // gather up all the valid properties on a class
            // this does not return properties in a particular order
            var properties = value.GetType().GetValidProperties();
            foreach (var property in properties)
            {
                // convert the model property name to camelCase to respect Javascript conventions
                // then grab the current value of the property and serialize the result of that as well.
                javaScriptObject[property.ConvertNameToCamelCase()] = SerializeObject(property.GetValue(value));
            }
            return javaScriptObject;
        }
        /// <summary>
        /// Javascript does not natively support the <see cref="Version"/> class so it is serialized to a string here
        /// </summary>
        /// <param name="version">the instance of Version to be serialized</param>
        /// <returns>a string representation of the <paramref name="version"/> instance</returns>
        private static string SerializeVersion(Version version)
        {
            return version.ToString();
        }

        /// <summary>
        /// Javascript does not support <see cref="Guid"/> structures so serialize it into a string.
        /// </summary>
        /// <param name="guid">a GUID</param>
        /// <returns>the string representation of a GUID</returns>
        private static string SerializeGuid(Guid guid)
        {
            return guid.ToString("N");
        }

        /// <summary>
        /// A tuple is a data structure that has a specific number and sequence of elements.
        /// Because tuples are not actually serializable in C#, we convert them to an array of serialized objects.
        /// </summary>
        /// <param name="tuple">Can be a Tuple or ValueTuple</param>
        /// <returns></returns>
        private static List<object> SerializeTuple(object tuple)
        {
            // ValueTuples are structs, so we get it objects fields
            return tuple.GetType().GetFields().Select(field => SerializeObject(field.GetValue(tuple))).ToList();
        }

        /// <summary>
        /// Iterates through a collection, serializes the items, and returns the collection.
        /// </summary>
        /// <param name="collection">The collection that contains .NET types that need to be serialized.</param>
        /// <returns>The newly formed Javascript collection</returns>
        private static List<object> SerializeCollection(IEnumerable collection)
        {
            return (from object entry in collection select SerializeObject(entry)).ToList();
        }

        /// <summary>
        /// Iterate through a dictionary and form a new one with the originals serialized values.
        /// </summary>
        /// <param name="dict">The dictionary that contains .NET types that need to be serialized.</param>
        /// <returns>The newly formed Javascript ready dictionary.</returns>
        private static Dictionary<string, object> SerializeDictionary(IEnumerable dict)
        {
            var ret = new Dictionary<string, object>();
            foreach (DictionaryEntry entry in dict)
            {
                var key = entry.Key.ToString();
                if (entry.Key.GetType().IsEnum)
                    key = ((int)entry.Key).ToString();

                ret[key] = SerializeObject(entry.Value);
            }

            return ret;
        }
    }
}
