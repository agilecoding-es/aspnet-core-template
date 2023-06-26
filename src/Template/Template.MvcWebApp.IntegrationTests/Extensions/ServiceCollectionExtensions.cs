﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Configuration;
using static Template.Configuration.Constants.Configuration;

namespace Template.MvcWebApp.IntegrationTests.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static async Task<IServiceCollection> ResetDatabase(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString(Constants.Configuration.ConnectionString.DEFAULT_CONNECTION);
            if (
                (
                !connectionString.Contains("localhost") &&
                !connectionString.Contains("SQLEXPRESS") &&
                !connectionString.Contains("MSSQLSERVER") &&
                !connectionString.Contains("ats.sql") &&
                !connectionString.Contains("(local)") &&
                !connectionString.Contains("(localdb)") &&
                !connectionString.Contains("127.0.0.1")
                )
            )
            {
                throw new Exception("Cannot RESET Database if you have a non development Database in your appsettings file.");
            }
            return services;
        }
    }
}
