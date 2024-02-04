using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Template.Domain.Entities.Identity;

namespace Template.Application.Features.IdentityContext.Services
{
    public class SignInManager : SignInManager<User>
    {
        public SignInManager(UserManager userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<User> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<User> confirmation) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }
    }
}
