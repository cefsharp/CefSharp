using System;
using System.Collections.Generic;
using System.IO;
using DSerfozo.LibclangSharp;

namespace CefSharp.Prebuild
{
    sealed class ClassVisitor
    {
        private readonly Dictionary<string, ClassDescriptor> visited = new Dictionary<string, ClassDescriptor>();

        public IDictionary<string, ClassDescriptor> Classes
        {
            get { return visited; }
        }

        public ClassVisitor()
        {
        }

        public ChildVisitResult Visit(Cursor cursor, Cursor parent)
        {
            if (cursor.Kind == CursorKind.ClassDecl)
            {
                var className = cursor.Spelling;
                VisitClass(cursor, className);
            }

            return ChildVisitResult.Continue;
        }

        private void VisitClass(Cursor cursor, string className)
        {
            if (!visited.ContainsKey(className))
            {
                visited.Add(className, new ClassDescriptor(className, Path.GetFileName(cursor.Location.FileName)));
            }

            var currentClass = visited[className];
            cursor.VisitChildren(VisitMethod, currentClass);
        }

        private ChildVisitResult VisitMethod(Cursor cursor, Cursor parent, ClassDescriptor @class)
        {
            if(cursor.Kind == CursorKind.CXXMethod)
            {
                var methodName = cursor.Spelling;
                var resultType = cursor.ResultType.Spelling;
                var currentMethod = new MethodDescriptor(methodName, resultType);

                cursor.VisitChildren(VisitParams, currentMethod);


                if (!@class.Methods.Contains(currentMethod))
                {
                    @class.Methods.Add(currentMethod);
                }
            }
            return ChildVisitResult.Continue;
        }

        private ChildVisitResult VisitParams(Cursor cursor, Cursor parent, MethodDescriptor method)
        {
            if (cursor.Kind == CursorKind.ParmDecl)
            {
                method.Parameters.Add(Tuple.Create(cursor.Type.Spelling, cursor.Spelling));
            }
            return ChildVisitResult.Continue;
        }
    }
}
