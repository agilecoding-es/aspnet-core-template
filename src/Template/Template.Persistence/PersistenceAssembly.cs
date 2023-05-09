using System.Reflection;

namespace Template.Persistence
{
    public sealed class PersistenceAssembly
    {
        public static Assembly Assembly => typeof(PersistenceAssembly).Assembly;
        public static string AssemblyName => typeof(PersistenceAssembly).Assembly.FullName;

    }
}
