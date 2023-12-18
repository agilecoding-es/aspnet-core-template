using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.Features.Sample.Contracts;
using Template.Domain.Entities.Identity;
using Template.Domain.Entities.Sample;
using Template.Persistence.PosgreSql.Database;

namespace Template.WebApp.IntegrationTests.Fixtures
{
    internal class SampleListFixture
    {
        private readonly WebAppFactory factory;

        public SampleListFixture(WebAppFactory factory)
        {
            this.factory = factory;
        }

        /// <summary>
        /// Sets the number of SampleList without items specified in parameter <paramref name="countOfSampleList"/> with the <paramref name="user"/> as owner.
        /// </summary>
        /// <param name="user">Owner of SampleList</param>
        /// <param name="countOfSampleList">Count of SampleList</param>
        /// <returns></returns>
        public async Task<List<SampleList>> SetFixtureAsync(User user, int countOfSampleList = 5)
        {
            var faker = new Faker<SampleList>()
           .RuleFor(e => e.Name, f => $"Lista {f.IndexFaker}")
           .RuleFor(e => e.UserId, user.Id);

            var data = faker.Generate(countOfSampleList);

            await factory.ExecuteInScopeAsync(async services =>
            {
                var context = services.GetService<Context>();
                var sampleListRepository = services.GetService<ISampleListRepository>();

                await sampleListRepository.AddRangeAsync(data, CancellationToken.None);
                await context.SaveChangesAsync();
            });

            return data;
        }
    }
}
