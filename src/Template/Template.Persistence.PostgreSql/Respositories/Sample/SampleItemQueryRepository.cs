﻿using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Template.Application.Features.Sample.Contracts;
using Template.Application.Features.Sample.Contracts.DTOs;

namespace Template.Persistence.PosgreSql.Respositories.Sample
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

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                result = await connection.QueryAsync<SampleItemDto>(SampleItemsQuery, parameters);
            }

            return result;
        }
    }
}
