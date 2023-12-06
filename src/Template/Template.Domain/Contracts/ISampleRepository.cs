using Template.Domain.Entities.Sample;

namespace Template.Domain.Contracts
{
    public interface ISampleRepository
    {
        Task<SampleList?> GetByIdAsync(int sampleListId);
    }
}
