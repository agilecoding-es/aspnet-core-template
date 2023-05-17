using Mapster;
using Template.Application.Sample.Queries;
using Template.Domain.Entities.Sample;

namespace Template.Application.Mappings.Sample
{
    public class EntitiesToDtoMappings : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //config
            //    .NewConfig<SampleList, GetSampleListByUser.Response>()
            //    .Map(dest => dest.ItemsCount, src => src.Items.Count());
        }
    }
}
