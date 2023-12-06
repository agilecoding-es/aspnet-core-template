using Mapster;
using Template.Application.Features.Sample.Contracts.DTOs;
using Template.MvcWebApp.Areas.SampleMvc.Models.SampleList;

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
