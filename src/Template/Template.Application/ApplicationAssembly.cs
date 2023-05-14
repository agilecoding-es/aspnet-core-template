using System.Reflection;

namespace Template.Application
{
    public sealed class ApplicationAssembly
    {
        public static Assembly Assembly => typeof(ApplicationAssembly).Assembly;

        public static string AssemblyName => typeof(ApplicationAssembly).Assembly.FullName;
    }
}
