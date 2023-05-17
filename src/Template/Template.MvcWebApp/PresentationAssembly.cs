using System.Reflection;
using Template.Configuration.AssemblyInfo;

namespace Template.MvcWebApp
{
    public sealed class PresentationAssembly : IAssemblyInfo
    {
        public static Assembly Assembly => typeof(PresentationAssembly).Assembly;
        public static string AssemblyName => typeof(PresentationAssembly).Assembly.FullName;
    }
}
