using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Contracts.DTOs.Sample;
using Template.Domain.Entities.Sample;

namespace Template.Application.Contracts.Repositories.Sample
{
    public interface ISampleItemRepository : IRepository<SampleItem, int>
    {
    }
}
