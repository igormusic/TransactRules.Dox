
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Reflection;
using System.Xml;
using System.Runtime.Serialization;


namespace TransactRules.Core.Utility
{
    public class TypeEnumerator{
       
        private static Dictionary<string, Type> types = new Dictionary<string, Type>();
        private static Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();
        private static bool isInitialized = false;
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(TypeEnumerator));

        private const string AssemblyFileNamePrefix = "TransactRules";

        /// <summary>
        /// List of all types found in assemblies in current working directory with prefix defined by AssemblyFileNamePrefix
        /// </summary>
        public static IEnumerable<Type> Types
        {
            get
            {
                if (isInitialized == false)
                {
                    Initialize();
                }

                return types.Values;
            }
        }

        private static void Initialize()
        {
            lock (types)
            {

                string path = AppDomain.CurrentDomain.RelativeSearchPath;

                if (string.IsNullOrEmpty(path))
                {
                    //for nUnit testing RelativeSearchPath returns nothing
                    path = AppDomain.CurrentDomain.BaseDirectory;
                }

                DirectoryInfo directory = new DirectoryInfo(path);

                ReadTypes(directory, types, assemblies, (f => f.Name.StartsWith(AssemblyFileNamePrefix, StringComparison.InvariantCultureIgnoreCase)));

                isInitialized = true;
            }
        }
	
        private static void ReadTypes(DirectoryInfo directory, Dictionary<string, Type> types, Dictionary<string, Assembly> assemblies, Func<FileInfo,bool> fileCriteria)
            {
                var files = from file in directory.GetFiles()
                            where fileCriteria(file)
                              && (file.Extension == ".dll" || file.Extension == ".exe")
                            select file;
	
                foreach (var file in files)
                {
                    try
                    {
                        log.Debug(string.Format("Loading assembly {0}", file.FullName));
	                  
                        Assembly assembly = Assembly.LoadFrom(file.FullName);

                       
                        if ( assemblyHasNotBeenAdded(assemblies, assembly) )
                        {
                            assemblies.Add( assembly.FullName, assembly );

                            foreach ( Type type in assembly.GetTypes( ) )
                            {
                                if ( types.ContainsKey( type.FullName ) )
                                    continue;

                                types.Add( type.FullName, type );
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Warn(string.Format("Assembly {0} loading failed. Error message: {1}", file.FullName, ex.Message));
                    }
                }

                foreach (DirectoryInfo subdirectory in directory.GetDirectories())
                {
                    ReadTypes(subdirectory, types, assemblies, fileCriteria);
                }
            }

        private static bool assemblyHasNotBeenAdded(Dictionary<string, Assembly> assemblies, Assembly assembly)
        {
            return !assemblies.Keys.Contains(assembly.FullName);
        }
    }	
	       
}
