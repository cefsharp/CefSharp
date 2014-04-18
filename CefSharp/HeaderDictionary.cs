using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CefSharp
{
    public class HeaderDictionary : IHeaderDictionary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CefSharp.HeaderDictionary" /> class.
        /// </summary>
        /// <param name="store">The underlying data store.</param>
        public HeaderDictionary(IDictionary<string, string[]> store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            Store = store;
        }

        private IDictionary<string, string[]> Store { get; set; }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.ICollection" /> that contains the keys in the <see cref="T:CefSharp.HeaderDictionary" />;.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.ICollection" /> that contains the keys in the <see cref="T:CefSharp.HeaderDictionary" />.</returns>
        public ICollection<string> Keys
        {
            get { return Store.Keys; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<string[]> Values
        {
            get { return Store.Values; }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:CefSharp.HeaderDictionary" />;.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="T:CefSharp.HeaderDictionary" />.</returns>
        public int Count
        {
            get { return Store.Count; }
        }

        /// <summary>
        /// Gets a value that indicates whether the <see cref="T:CefSharp.HeaderDictionary" /> is in read-only mode.
        /// </summary>
        /// <returns>true if the <see cref="T:CefSharp.HeaderDictionary" /> is in read-only mode; otherwise, false.</returns>
        public bool IsReadOnly
        {
            get { return Store.IsReadOnly; }
        }

        /// <summary>
        /// Get or sets the associated value from the collection as a single string.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <returns>the associated value from the collection as a single string or null if the key is not present.</returns>
        public string this[string key]
        {
            get { return GetHeader(key); }
            set { Set(key, value); }
        }

        /// <summary>
        /// Throws KeyNotFoundException if the key is not present.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <returns></returns>
        string[] IDictionary<string, string[]>.this[string key]
        {
            get { return Store[key]; }
            set { Store[key] = value; }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<string, string[]>> GetEnumerator()
        {
            return Store.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        /// <summary>
        /// Get the associated values from the collection without modification.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <returns>the associated value from the collection without modification, or null if the key is not present.</returns>
        public IList<string> GetValues(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            string[] values;
            return Store.TryGetValue(key, out values) ? values : null;

        }

        /// <summary>
        /// Add a new value. Appends to the header if already present
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="value">The header value.</param>
        public void Append(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            string existing = GetHeader(key);
            if (existing == null)
            {
                Set(key, value);
            }
            else
            {
                Store[key] = new[] { existing + "," + value };
            }
        }

        /// <summary>
        /// Add new values. Each item remains a separate array entry.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="values">The header values.</param>
        public void AppendValues(string key, params string[] values)
        {
            if (values == null || values.Length == 0)
            {
                return;
            }

            var existing = GetValues(key);

            if (existing == null)
            {
                SetValues(key, values);
            }
            else
            {
                SetValues(key, existing.Concat(values).ToArray());
            }

        }

        /// <summary>
        /// Sets a specific header value.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="value">The header value.</param>
        public void Set(string key, string value)
        {

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            if (string.IsNullOrWhiteSpace(value))
            {
                Store.Remove(key);
            }
            else
            {
                Store[key] = new[] { value };
            }
        }

        /// <summary>
        /// Sets the specified header values without modification.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="values">The header values.</param>
        public void SetValues(string key, params string[] values)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            if (values == null || values.Length == 0)
            {
                Store.Remove(key);
            }
            else
            {
                Store[key] = values;
            }
        }

        /// <summary>
        /// Adds the given header and values to the collection.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="value">The header values.</param>
        public void Add(string key, string[] value)
        {
            Store.Add(key, value);
        }

        /// <summary>
        /// Determines whether the <see cref="T:CefSharp.HeaderDictionary" /> contains a specific key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>true if the <see cref="T:CefSharp.HeaderDictionary" /> contains a specific key; otherwise, false.</returns>
        public bool ContainsKey(string key)
        {
            return Store.ContainsKey(key);
        }

        /// <summary>
        /// Removes the given header from the collection.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <returns>true if the specified object was removed from the collection; otherwise, false.</returns>
        public bool Remove(string key)
        {
            return Store.Remove(key);
        }

        /// <summary>
        /// Retrieves a value from the dictionary.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <param name="value">The value.</param>
        /// <returns>true if the <see cref="T:CefSharp.HeaderDictionary" /> contains the key; otherwise, false.</returns>
        public bool TryGetValue(string key, out string[] value)
        {
            return Store.TryGetValue(key, out value);
        }

        /// <summary>
        /// Adds a new list of items to the collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(KeyValuePair<string, string[]> item)
        {
            Store.Add(item);
        }

        /// <summary>
        /// Clears the entire list of objects.
        /// </summary>
        public void Clear()
        {
            Store.Clear();
        }

        /// <summary>
        /// Returns a value indicating whether the specified object occurs within this collection.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>true if the specified object occurs within this collection; otherwise, false.</returns>
        public bool Contains(KeyValuePair<string, string[]> item)
        {
            return Store.Contains(item);
        }

        /// <summary>
        /// Copies the <see cref="T:CefSharp.HeaderDictionary" /> elements to a one-dimensional Array instance at the specified index.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the specified objects copied from the <see cref="T:CefSharp.HeaderDictionary" />.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(KeyValuePair<string, string[]>[] array, int arrayIndex)
        {
            Store.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the given item from the the collection.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>true if the specified object was removed from the collection; otherwise, false.</returns>
        public bool Remove(KeyValuePair<string, string[]> item)
        {
            return Store.Remove(item);
        }

        /// <summary>
        /// Get the associated value from the collection as a single string.
        /// </summary>
        /// <param name="key">The header name.</param>
        /// <returns>the associated value from the collection as a single string or null if the key is not present.</returns>
        public string GetHeader(string key)
        {
            var values = GetValues(key);
            return values == null ? null : string.Join(",", values);
        }
    }
}