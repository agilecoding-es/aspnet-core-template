using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Configuration.Setup
{
    public interface IAppBuilder
    {
        IServiceCollection Services { get; }
        ConfigurationManager Configuration { get; }
        IWebHostEnvironment Environment { get; }

        IAppBuilder AddSettings(Action<IServiceCollection, ConfigurationManager> options);
        IAppBuilder AddPresentation();
        IAppBuilder AddIdentity();
        IAppBuilder AddAuthentication();
        IAppBuilder AddAuthorization();
        IAppBuilder AddDependencies();
        IAppBuilder AddResources();
        IAppBuilder AddMapster();
        IAppBuilder AddHelthChecks();

        WebApplication Build();
    }
}
