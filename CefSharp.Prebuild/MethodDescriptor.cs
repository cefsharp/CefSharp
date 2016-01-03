using System;
using System.Collections.Generic;
using System.Linq;

namespace CefSharp.Prebuild
{
    class MethodDescriptor
    {
        private readonly List<Tuple<string, string>> parameters = new List<Tuple<string, string>>();
        private readonly string name;
        private readonly string result;

        public string Name
        {
            get { return name; }
        }

        public string Result
        {
            get { return result; }
        }

        public List<Tuple<string, string>> Parameters
        {
            get { return parameters; }
        }

        public MethodDescriptor(string name, string result)
        {
            this.result = result;
            this.name = name;
        }

        public override int GetHashCode()
        {
            var p = (Parameters.Count > 0 ? Parameters.Select(i => i.Item1 + " " + i.Item2).Aggregate((i, j) => i + ", " + j) : "");
            return new { Name, Result, p }.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var p = (MethodDescriptor)obj;
            return Name == p.Name && Result == p.Result && Parameters.SequenceEqual(p.Parameters);
        }
    }
}