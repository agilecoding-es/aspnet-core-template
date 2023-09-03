using Bogus;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Template.Domain.Entities.Identity;
using Template.Application.Identity;
using Microsoft.AspNetCore.Builder;
using System.Reflection;
using Template.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Security.Authorization;
using Template.Domain.Entities.Sample;
using Template.Application.Contracts.Repositories.Sample;
using Template.Persistence.Database;

namespace Template.MvcWebApp.IntegrationTests.Fixtures
{
    public class SampleListFixture
    {
        private readonly WebAppFactory factory;

        public SampleListFixture()
        {
            factory = WebAppFactory.GetFactoryInstance();
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
           .RuleFor(e => e.Name, f => f.Lorem.Word())
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
