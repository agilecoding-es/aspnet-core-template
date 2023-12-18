namespace Template.Application.Features.Sample.Contracts.DTOs
{
    public class SampleListWithItemsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }

        public List<SampleItemDto> Items { get; set; }

        public string SuccessMessage { get; set; }
    }
}
