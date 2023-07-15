using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Template.Domain.Entities.Identity;
using Template.Persistence.Database;

namespace Template.Persistence.Identity
{
    public class UserStore : UserStore<User, Role, Context, string, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>
    {
        public UserStore(Context context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}
