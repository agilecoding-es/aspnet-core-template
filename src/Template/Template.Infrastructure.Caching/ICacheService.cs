using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Infrastructure.Caching
{
    public interface ICacheService
    {
        Task RemoveAsync(string key);
    }
}
