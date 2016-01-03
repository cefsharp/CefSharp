using System.Collections.Generic;

namespace CefSharp.Prebuild
{
    class ClassDescriptor
    {
        private readonly List<MethodDescriptor> methods = new List<MethodDescriptor>();

        public string Name { get; private set; }

        public string Location { get; private set; }

        public List<MethodDescriptor> Methods
        {
            get { return methods; }
        }

        public ClassDescriptor(string name, string location)
        {
            Name = name;
            Location = location;
        }
    }
}