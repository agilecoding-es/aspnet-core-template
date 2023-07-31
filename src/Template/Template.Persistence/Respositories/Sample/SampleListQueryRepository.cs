using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Template.Application.Contracts.DTOs.Sample;
using Template.Application.Contracts.Repositories.Sample;
using Template.Domain.Entities.Sample;

namespace Template.Persistence.Respositories.Sample
{
    public class SampleListQueryRepository : QueryRepository, ISampleListQueryRepository
    {
        #region Query Constants
        
        const string SampleListQuery =
            $@"SELECT l.Id, l.Name, l.UserId
                 FROM sample.SampleLists l (nolock)
               WHERE l.Id = @SampleListId";

        const string SampleListWithItemsCountByUserQuery =
            $@"SELECT l.Id, l.Name, COUNT(i.Id) ItemsCount
                 FROM sample.SampleLists l (nolock) 
            LEFT JOIN sample.SampleItems i (nolock) ON l.Id = i.ListId
               WHERE l.UserId = @UserId
            GROUP BY l.Id, l.Name";

        const string SampleItemByListIdQuery =
            $@"SELECT i.Id, i.Description, i.ListId
                 FROM sample.SampleItems i (nolock)
                WHERE i.ListId = @SampleListId";

        #endregion

        public SampleListQueryRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<SampleListWithItemsDto> GetByIdWithItemsAsync(int sampleListId, CancellationToken cancellationToken)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SampleListId", sampleListId);

            SampleListWithItemsDto result = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                result = await connection.QueryFirstOrDefaultAsync<SampleListWithItemsDto>(SampleListQuery, parameters);

                if (result != null)
                {
                    result.Items = (await connection.QueryAsync<SampleItemDto>(SampleItemByListIdQuery, parameters)).AsList();
                }
            }

            return result;
        }

        public async Task<List<SampleListWithItemsCountDto>> ListWithItemsCountByUserAsync(string userId, CancellationToken cancellationToken)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);

            List<SampleListWithItemsCountDto> result = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                result = (await connection.QueryAsync<SampleListWithItemsCountDto>(SampleListWithItemsCountByUserQuery, parameters)).AsList();
            }

            return result;
        }
    }
}
