using Mapster;
using Template.Application.Features.SampleContext.Contracts.DTOs;
using Template.WebApp.Areas.SampleMvc.Models.SampleList;

namespace Template.WebApp.Mappings.Sample
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
