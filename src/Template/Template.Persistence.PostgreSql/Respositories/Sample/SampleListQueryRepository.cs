using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Template.Application.Features.Sample.Contracts;
using Template.Application.Features.Sample.Contracts.DTOs;

namespace Template.Persistence.PosgreSql.Respositories.Sample
{
    public class SampleListQueryRepository : QueryRepository, ISampleListQueryRepository
    {
        #region Query Constants

        const string SampleListQuery =
            $@"select l.id as {nameof(SampleListWithItemsDto.Id)}, l.name as {nameof(SampleListWithItemsDto.Name)}, l.user_id as {nameof(SampleListWithItemsDto.UserId)}
                 from sample.samplelists l 
               where l.id = @SampleListId";

        const string SampleListWithItemsCountByUserQuery =
            $@"select l.id as {nameof(SampleListWithItemsCountDto.Id)}, l.name as {nameof(SampleListWithItemsCountDto.Name)}, count(i.id) as {nameof(SampleListWithItemsCountDto.ItemsCount)}
                 from sample.samplelists l  
            left join sample.sampleitems i  on l.id = i.list_id
               where l.user_id = @UserId
            group by l.id, l.name";

        const string SampleItemByListIdQuery =
            $@"select i.id as {nameof(SampleItemDto.Id)}, i.description as {nameof(SampleItemDto.Description)}, i.list_id as {nameof(SampleItemDto.ListId)}
                 from sample.sampleitems i 
                where i.list_id = @SampleListId";

        #endregion

        public SampleListQueryRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<SampleListWithItemsDto> GetByIdWithItemsAsync(int sampleListId, CancellationToken cancellationToken)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SampleListId", sampleListId);

            SampleListWithItemsDto result = null;
            try { 
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                result = await connection.QueryFirstOrDefaultAsync<SampleListWithItemsDto>(SampleListQuery, parameters);

                if (result != null)
                {
                    result.Items = (await connection.QueryAsync<SampleItemDto>(SampleItemByListIdQuery, parameters)).AsList();
                }
            }
            }catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<List<SampleListWithItemsCountDto>> ListWithItemsCountByUserAsync(string userId, CancellationToken cancellationToken)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);

            List<SampleListWithItemsCountDto> result = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                result = (await connection.QueryAsync<SampleListWithItemsCountDto>(SampleListWithItemsCountByUserQuery, parameters)).AsList();
            }

            return result;
        }
    }
}
