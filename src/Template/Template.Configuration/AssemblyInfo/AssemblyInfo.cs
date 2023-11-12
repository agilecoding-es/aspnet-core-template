using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Template.Configuration.AssemblyInfo
{
    public abstract class AssemblyBasicInfo<T>
    {
        public static Assembly Assembly => typeof(T).Assembly;
        public static string AssemblyName => Assembly.GetName().Name;
        public static string AssemblyFullName => Assembly.FullName;
    }
}
