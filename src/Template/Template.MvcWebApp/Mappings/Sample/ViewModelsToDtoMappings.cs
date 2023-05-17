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
            //config
            //    .NewConfig<SampleListViewModel, SampleListWithItemsDto>()
            //    .Map(dest => dest.i, src => src.);
        }
    }
}
