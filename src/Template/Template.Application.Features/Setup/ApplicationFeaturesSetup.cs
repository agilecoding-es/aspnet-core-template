using MediatR;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Behaviours;
using Template.Application.Features;
using Template.Application.Features.IdentityContext.Services;

namespace Template.Configuration.Setup
{
    public static class ApplicationFeaturesSetup
    {
        public static IAppBuilder AddApplicationFeatures(this IAppBuilder appBuilder)
        {
            appBuilder.Services
                .AddMemoryCache()
                .AddMediatR(configuration =>
                    configuration.RegisterServicesFromAssembly(ApplicationFeaturesAssembly.Assembly))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(LogginBehavior<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

            appBuilder.Services.AddTransient<IEmailSender, AuthEmailSender>();

            return appBuilder;

        }
    }
}
