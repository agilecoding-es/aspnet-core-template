using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Template.Domain.Entities.Identity;
using Template.Persistence.SqlServer.Database;

namespace Template.Persistence.Identity.SqlServer
{
    public class UserStore : UserStore<User, Role, Context, string, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>
    {
        public UserStore(Context context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}
