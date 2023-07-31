using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Contracts.DTOs.Sample;

namespace Template.Application.Contracts.Repositories.Sample
{
    public interface ISampleItemQueryRepository
    {
        Task<IEnumerable<SampleItemDto>> GetItemsByListId(int listId, CancellationToken cancellationToken);
    }
}
