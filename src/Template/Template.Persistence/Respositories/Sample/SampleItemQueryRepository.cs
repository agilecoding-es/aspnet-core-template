using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Contracts.DTOs.Sample;
using Template.Application.Contracts.Repositories.Sample;
using Template.Domain.Entities.Sample;
using Template.Persistence.Database;
using static Template.Configuration.Constants.Configuration;

namespace Template.Persistence.Respositories.Sample
{
    public class SampleItemQueryRepository : QueryRepository, ISampleItemQueryRepository
    {
        #region Query Constants

        const string SampleItemsQuery =
            $@"SELECT i.Id, i.Description, i.ListId
                 FROM sample.SampleItems i (nolock)
               WHERE i.ListId = @SampleListId";

        #endregion

        public SampleItemQueryRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IEnumerable<SampleItemDto>> GetItemsByListId(int listId, CancellationToken cancellationToken)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SampleListId", listId);

            IEnumerable<SampleItemDto> result = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                result = await connection.QueryAsync<SampleItemDto>(SampleItemsQuery, parameters);
            }

            return result;
        }
    }
}
