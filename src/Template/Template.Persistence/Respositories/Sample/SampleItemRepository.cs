using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Contracts.Repositories.Sample;
using Template.Domain.Entities.Sample;
using Template.Persistence.Database;

namespace Template.Persistence.Respositories.Sample
{
    public class SampleItemRepository : Repository<SampleItem, SampleItemKey>, ISampleItemRepository
    {
        public SampleItemRepository(Context context) : base(context)
        {
        }
    }
}
