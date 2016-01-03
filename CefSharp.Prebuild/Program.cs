using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using DSerfozo.LibclangSharp;

namespace CefSharp.Prebuild
{
    class Program
    {
        private const string StateFileName = "state.txt";
        private static readonly List<string> Interfaces = new List<string>();
        private static readonly List<string> Headers = new List<string>();
        private static readonly ClassVisitor ClassVisitor = new ClassVisitor();
        private static string OutputDir;
        private static string InterfacesFile;
        private static string CefInclude;
        private static string CefVersion;
        private static string[] Includes;

        static void Main(string[] args)
        {
            if (args.Length < 5)
            {
                throw new InvalidOperationException();
            }

            ProcessArguments(args);

            //make sure output dir exists
            Directory.CreateDirectory(OutputDir);
            //check if all the stuff is generated already
            if (CheckStateFile())
            {
                return;
            }

            FillInterfacesAndHeaders();
            RunClassVisitor();
            GenerateOutput();
            CreateStateFile();
        }

        private static void GenerateOutput()
        {
            //the interfaces list can be modified from the template hence no foreach
            for (var i = 0; i < Interfaces.Count; i++)
            {
                var className = Interfaces[i];
                if (ClassVisitor.Classes.ContainsKey(className))
                {
                    var templ = InitializeTemplate(className);
                    var outputFileName = Path.Combine(OutputDir, string.Format("{0}Safe.h", className));
                    using (var output = new StreamWriter(File.Open(outputFileName, FileMode.Create)))
                    {
                        output.Write(templ.TransformText());
                    }
                }
            }
        }

        private static void RunClassVisitor()
        {
            var index = CreateIndex();
            foreach (var translationUnit in index.TranslationUnits)
            {
                translationUnit.Cursor.VisitChildren(ClassVisitor.Visit);
            }
        }

        private static void FillInterfacesAndHeaders()
        {
            //read the list of directly used cef interfaces/headers
            foreach (var line in File.ReadAllLines(InterfacesFile)
                .Select(l => l.Split(',')))
            {
                Interfaces.Add(line[0].Trim());
                Headers.Add(line[1].Trim());
            }
        }

        private static void CreateStateFile()
        {
            File.WriteAllText(Path.Combine(OutputDir, StateFileName), CefVersion + Environment.NewLine + GetInterfaceListHash());
        }

        private static void ProcessArguments(string[] args)
        {
            Includes = args[0].Split(';');
            CefInclude = Path.Combine(args[1], "include");
            InterfacesFile = args[2];
            OutputDir = args[3];
            CefVersion = args[4];
        }

        private static Index CreateIndex()
        {
            var includes = Path.Combine(CefInclude, "include");
            var files = Directory.GetFiles(includes, "*.h")
                .Where(f => Headers.Contains(Path.GetFileName(f)));
            var index = new Index();
            foreach (var file in files)
            {
                index.AddTranslationUnit(file, BuildCompilerArgs());
            }
            return index;
        }

        private static string GetInterfaceListHash()
        {
            var md5 = MD5.Create();
            md5.Initialize();
            var handlersHash = Convert.ToBase64String(md5.ComputeHash(File.ReadAllBytes(InterfacesFile)));
            return handlersHash;
        }

        private static bool CheckStateFile()
        {
            var stateFile = Path.Combine(OutputDir, StateFileName);
            if (File.Exists(stateFile))
            {
                var lines = File.ReadAllLines(stateFile);
                if (lines.Length == 2 && lines[0] == CefVersion && lines[1] == GetInterfaceListHash())
                {
                    return true;
                }
            }
            return false;
        }

        private static WrapperTemplate InitializeTemplate(string className)
        {
            
            var templ = new WrapperTemplate
            {
                Session = new Dictionary<string, object>()
                {
                    //list of classes to generate app domain safe wrappers for
                    //NOTE: this can be modified by the template
                    {"SafeClasses", Interfaces},
                    //member info for the class currently being wrapped
                    {"ClassData", ClassVisitor.Classes[className]},
                    //name of the wrapper template class
                    {"SafeTypeTemplateName", "SafeType"},
                    //name of the default template spec typedef
                    {"SafeTypeName", "Safe"}
                }
            };
            templ.Initialize();
            return templ;
        }

        private static IEnumerable<string> BuildCompilerArgs()
        {
            var includeDirs = Includes.ToList();
            includeDirs.Add(CefInclude);
            var compilerArgs = new List<string>
            {
                "-x",
                "c++"
            };
            foreach (var includeDir in includeDirs)
            {
                compilerArgs.Add("-I");
                compilerArgs.Add(includeDir);
            }
            return compilerArgs;
        }
    }
}
