using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.Entities.Sample;

namespace Template.Domain.Contracts
{
    public interface ISampleRepository
    {
        Task<SampleList?> GetByIdAsync(SampleListKey id);
    }
}
