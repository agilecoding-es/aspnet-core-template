using Mapster;

namespace Template.Application.Features.Sample.Mappings
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
