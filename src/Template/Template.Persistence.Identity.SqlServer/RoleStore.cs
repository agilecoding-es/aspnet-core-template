using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Template.Domain.Entities.Identity;
using Template.Persistence.SqlServer.Database;

namespace Template.Persistence.Identity.SqlServer
{
    public class RoleStore : RoleStore<Role, Context, string, UserRole, RoleClaim>
    {
        public RoleStore(Context context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}
