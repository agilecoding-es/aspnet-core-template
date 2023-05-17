using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Template.Configuration.AssemblyInfo;

namespace Template.Infrastructure
{
    public sealed class InfrastructureAssembly: IAssemblyInfo
    {
        public static Assembly Assembly => typeof(InfrastructureAssembly).Assembly;
        public static string AssemblyName => typeof(InfrastructureAssembly).Assembly.FullName;
    }
}
