using Mapster;
using Template.Application.Contracts.DTOs.Sample;
using Template.Application.Sample.Queries;
using Template.MvcWebApp.Areas.Sample.Models.SampleList;

namespace Template.MvcWebApp.Mappings.Sample
{
    public class ViewModelsToDtoMappings : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .NewConfig<SampleItemViewModel, SampleItemDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.ListId, src => src.ListId)
                .Map(dest => dest.Description, src => src.Description);
        }
    }
}
