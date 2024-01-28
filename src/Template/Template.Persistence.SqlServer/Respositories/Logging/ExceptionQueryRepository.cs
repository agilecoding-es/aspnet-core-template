using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Template.Application.Features.LoggingContext.Contracts;
using Template.Application.Features.SampleContext.Contracts.DTOs;

namespace Template.Persistence.SqlServer.Respositories.Logging
{
    public class ExceptionQueryRepository : QueryRepository, IExceptionQueryRepository
    {
        #region Query Constants

        const string ExceptionsCount =
            @"SELECT COUNT(*) FROM log.""Exceptions""";

        #endregion

        public ExceptionQueryRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<int> GetExceptionsCountAsync(CancellationToken cancellationToken)
        {
            int result = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                result = await connection.QueryFirstOrDefaultAsync<int>(ExceptionsCount);
            }

            return result;
        }
    }
}
