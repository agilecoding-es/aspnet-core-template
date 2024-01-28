using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Template.Application.Features.LoggingContext.Contracts;
using Template.Application.Features.SampleContext.Contracts;
using Template.Application.Features.SampleContext.Contracts.DTOs;

namespace Template.Persistence.PosgreSql.Respositories.Logging
{
    public class ExceptionQueryRepository : QueryRepository, IExceptionQueryRepository
    {
        #region Query Constants

        const string ExceptionsCount =
            @"SELECT COUNT(*) FROM log.exceptions";

        #endregion

        public ExceptionQueryRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<int> GetExceptionsCountAsync(CancellationToken cancellationToken)
        {
            int result = 0;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                result= await connection.ExecuteScalarAsync<int>(ExceptionsCount);
            }

            return result;
        }
    }
}
