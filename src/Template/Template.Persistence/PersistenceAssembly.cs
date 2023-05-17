using System.Reflection;
using Template.Application.Contracts.Repositories;
using Template.Configuration.AssemblyInfo;

namespace Template.Persistence
{
    public sealed class PersistenceAssembly: IAssemblyInfo
    {
        public static Assembly Assembly => typeof(PersistenceAssembly).Assembly;
        public static string AssemblyName => typeof(PersistenceAssembly).Assembly.FullName;
    }
}
