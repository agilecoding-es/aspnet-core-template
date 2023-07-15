using System.Security.Claims;

namespace Template.Authorization.Constants
{
    public static class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.AddRole, ClaimTypes.AddRole),//true.ToString()),
            new Claim(ClaimTypes.EditRole, ClaimTypes.EditRole),//true.ToString()),
            new Claim(ClaimTypes.DeleteRole, ClaimTypes.DeleteRole),//true.ToString())

            new Claim(ClaimTypes.EditUser, ClaimTypes.EditUser),//true.ToString()),
            new Claim(ClaimTypes.DeleteUser, ClaimTypes.DeleteUser)//true.ToString())
        };
    }
}
