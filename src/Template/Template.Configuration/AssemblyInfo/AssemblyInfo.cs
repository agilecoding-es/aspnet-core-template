using System.Reflection;

namespace Template.Configuration.AssemblyInfo
{
    public abstract class AssemblyBasicInfo<T>
    {
        public static Assembly Assembly => typeof(T).Assembly;
        public static string AssemblyName => Assembly.GetName().Name;
        public static string AssemblyFullName => Assembly.FullName;
    }
}
