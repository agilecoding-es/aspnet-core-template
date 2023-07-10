using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Template.Configuration;


namespace Template.MvcWebApp.IntegrationTests.Queries
{
    public static class ExceptionsQueries
    {
        public static SqlConnection CreateConnection(string connectionString) => new SqlConnection(connectionString);

        public static async Task<int> GetExceptionsCountAsync(this SqlConnection connection)
        {
            var query = @"SELECT COUNT(*) FROM log.logs";

            using (connection)
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<int>(query);
            }
        }
    }
}
