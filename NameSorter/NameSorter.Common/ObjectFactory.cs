using Ninject;
using System;
using System.Linq;
using System.Reflection;

namespace NameSorter.Common
{
    public class ObjectFactory
    {
        private static IKernel _kernel;

        public static IKernel Kernel
        {
            get
            {
                // Singleton of Ninject StandardKernel 
                if (_kernel == null)
                {
                    _kernel = new StandardKernel();
                }

                return _kernel;
            }
        }

        public static T CreateInstance<T>()
        {
            return Kernel.Get<T>();
        }

        public static void BindAll()
        {
            BindServices();
            BindEngines();
            BindRepositories();
        }

        public static void BindServices()
        {
            Bind("NameSorter.Services");
        }

        public static void BindEngines()
        {
            Bind("NameSorter.Engines");
        }

        public static void BindRepositories()
        {
            Bind("NameSorter.Repositories");
        }

        private static void Bind(string assemblyName)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            string interfacesNamespace = string.Format("{0}.Interfaces", assemblyName);
            string implementationsNamespace = string.Format("{0}.Implementations", assemblyName);

            if (assembly != null)
            {
                // Retrieve all interfaces and implementation types
                var interfaces = assembly.GetTypes().Where(e => e.Namespace == interfacesNamespace);
                var implementations = assembly.GetTypes().Where(e => e.Namespace == implementationsNamespace);

                if (interfaces != null && interfaces.Count() > 0)
                {
                    // Loop through all types in the interfaces and Bind by matching to the implementations
                    foreach (Type interfaceType in interfaces)
                    {
                        // Retrieve the implementation by assuming the interface name is the same except that it starts with I
                        Type implementationType = implementations.FirstOrDefault(e => String.Format("I{0}", e.Name) == interfaceType.Name);
                        if (implementationType != null)
                        {
                            // Bind interface to its implementation
                            Kernel.Bind(interfaceType).To(implementationType);
                        }
                    }
                }
            }
        }
    }
}
