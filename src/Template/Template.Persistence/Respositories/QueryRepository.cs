﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Common;
using Template.Configuration;

namespace Template.Persistence.Respositories
{
    public abstract class QueryRepository
    {

        protected readonly string _connectionString;

        public QueryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(Constants.Configuration.ConnectionString.DefaultConnection.Value) ?? throw new ArgumentNullException(nameof(configuration));
        }
    }
}
