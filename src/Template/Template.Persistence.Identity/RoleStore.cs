using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.Entities.Identity;
using Template.Persistence.Database;

namespace Template.Persistence.Identity
{
    public class RoleStore : RoleStore<Role,Context,string,UserRole,RoleClaim>
    {
        public RoleStore(Context context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}
