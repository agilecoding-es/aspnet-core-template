using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Template.Application.Contracts.Repositories;
using Template.Configuration.AssemblyInfo;

namespace Template.Application
{
    public sealed class ApplicationAssembly : IAssemblyInfo
    {
        public static Assembly Assembly => typeof(ApplicationAssembly).Assembly;
        public static string AssemblyName => typeof(ApplicationAssembly).Assembly.FullName;

    }
}
