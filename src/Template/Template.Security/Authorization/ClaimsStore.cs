using System.Security.Claims;

namespace Template.Security.Authorization
{
    public static class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            new Claim(ApplicationClaimTypes.AddRole, ApplicationClaimTypes.AddRole),//true.ToString()),
            new Claim(ApplicationClaimTypes.EditRole, ApplicationClaimTypes.EditRole),//true.ToString()),
            new Claim(ApplicationClaimTypes.DeleteRole, ApplicationClaimTypes.DeleteRole),//true.ToString())

            new Claim(ApplicationClaimTypes.EditUser, ApplicationClaimTypes.EditUser),//true.ToString()),
            new Claim(ApplicationClaimTypes.DeleteUser, ApplicationClaimTypes.DeleteUser)//true.ToString())
        };
    }
}
