using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Template.Common.Extensions;
using ClaimTypes = System.Security.Claims.ClaimTypes;

namespace Template.Security.Authorization.Requirements
{
    public class CanEditOnlyOtherAdminRolesAndClaimsHandler : AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CanEditOnlyOtherAdminRolesAndClaimsHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirement requirement)
        {
            var longitudOfGuid = 36;
            var httpContext = httpContextAccessor.HttpContext;

            var lastSegment = httpContext.Request.Path.Value.Split('/').Last();
            if (lastSegment.Contains('?'))
            {
                lastSegment = lastSegment.Replace(httpContext.Request.QueryString.Value, string.Empty);
            }

            string loggedInAdminId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            string adminIsBeingEdited = lastSegment;

            if (
                context.User.IsInRole(Roles.Superadmin) ||

                    (
                        context.User.IsInRole(Roles.Admin) ||
                        context.User.HasClaim(c => c.Type == ApplicationClaimTypes.EditRole && c.Value == true.AsString())
                    ) &&
                    adminIsBeingEdited.ToLower() != loggedInAdminId.ToLower()

               )
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
