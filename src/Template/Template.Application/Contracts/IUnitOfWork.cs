using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Application.Contracts
{
    public interface IUnitOfWork
    {
        void SaveChanges();

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
