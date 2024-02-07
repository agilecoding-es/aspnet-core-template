namespace Template.Application.Features.SampleContext.Contracts.DTOs
{
    public class SampleListWithItemsCountDto
    {
        public SampleListWithItemsCountDto(){}

        public int Id { get; set; }
        public string Name { get; set; }
        public int ItemsCount { get; set; }

    }
}
