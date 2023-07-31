using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using Template.Application.Identity;
using Template.Common;
using Template.Common.Extensions;
using Template.Domain.Entities.Identity;
using Template.MvcWebApp.Areas.Administration.Models;
using Template.Security.Authorization;
using Template.Security.DataProtection;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Template.MvcWebApp.Areas.Administration.Controllers
{
    [Authorize(Roles = Roles.SuperadminAndAdminRoles)]
    public class UsersController : BaseAdministrationController
    {
        private readonly IMediator mediator;
        private readonly UserManager userManager;
        private readonly RoleManager roleManager;
        private readonly IAuthorizationService authorizationService;
        private readonly IHtmlLocalizer localizer;
        private readonly IEmailSender emailSender;
        private readonly IDataProtector protector;

        public UsersController(IMediator mediator, UserManager userManager, RoleManager roleManager, IAuthorizationService authorizationService, IHtmlLocalizer localizer, IEmailSender emailSender, IDataProtectionProvider dataProtectionProvider) : base(mediator, userManager, roleManager, localizer)
        {
            this.mediator = mediator;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.authorizationService = authorizationService;
            this.localizer = localizer;
            this.emailSender = emailSender;
            //TODO: Encrypt routvalues
            this.protector = dataProtectionProvider.CreateProtector(ProtectionPurpose.RoleIdRouteValue);
        }

        public async Task<IActionResult> Index()
        {
            var users = new List<User>();

            var superadminUsers = await userManager.GetUsersInRoleAsync(Roles.Superadmin);
            var allUsers = await userManager.Users.ToListAsync();

            var allUsersExceptSuperadmin = allUsers.Except(superadminUsers).ToList();
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            users = await userManager.IsInRoleAsync(user, Roles.Superadmin) ? allUsers : allUsersExceptSuperadmin;

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                //TODO: Crear constante y revisar modelo de manejo de errores
                ModelState.AddModelError(Constants.KeyErrors.ValidationError, "User not found");
            }

            var model = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsLockout = user.LockoutEnd !=null && user.LockoutEnd > DateTimeOffset.Now,
                Claims = (await userManager.GetClaimsAsync(user)).Where(c=>c.Value == true.AsString()).Select(c => c.Type).ToList(),
                Roles = (await userManager.GetRolesAsync(user)).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByIdAsync(model.Id);

                if (user == null)
                {
                    //TODO: Crear constante y revisar modelo de manejo de errores
                    ModelState.AddModelError(Constants.KeyErrors.ValidationError, "User not found");
                }
                else
                {
                    user.UserName = model.UserName;

                    //TODO: Establecer lógica para enviar mail al antiguo mail con posibilidad de revertir el cambio
                    user.Email = model.Email;

                    var result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in result.Errors)
                    {
                        //TODO: Revisar modelo de manejo de errores
                        ModelState.AddModelError(Constants.KeyErrors.ValidationError, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            //TODO: Agregar confirmación antes de eliminar un usuario
            //TODO: Agregar clases para soft delete a las entidades
            //TODO: Agregar auditoria a las entidades

            User user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                //TODO: Crear constante y revisar modelo de manejo de errores
                ModelState.AddModelError(Constants.KeyErrors.ValidationError, "User not found");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    //TODO: Revisar modelo de manejo de errores
                    ModelState.AddModelError(Constants.KeyErrors.ValidationError, error.Description);
                }
            }

            return View(nameof(Edit));
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string id)
        {
            //TODO: Agregar confirmación antes de eliminar un usuario
            //TODO: Agregar clases para soft delete a las entidades
            //TODO: Agregar auditoria a las entidades

            User user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                //TODO: Crear constante y revisar modelo de manejo de errores
                ModelState.AddModelError(Constants.KeyErrors.ValidationError, "User not found");
            }
            else
            {

                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                await emailSender.SendEmailAsync(
                user.Email,
                    localizer.GetString("Identity_Account_ForgotPassword_ConfirmEmailSubject"),
                    localizer.GetString("Identity_Account_ForgotPassword_ConfirmEmailBody", HtmlEncoder.Default.Encode(callbackUrl)));

            }

            return RedirectToAction(nameof(Edit),new { id });

        }

        [HttpGet]
        public async Task<IActionResult> ManageRoles(string id)
        {
            ViewBag.UserId = id;
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                //TODO: Crear constante y revisar modelo de manejo de errores
                ModelState.AddModelError(Constants.KeyErrors.ValidationError, "User not found");
            }

            var model = new List<UserRolesViewModel>();
            foreach (var role in roleManager.Roles)
            {
                model.Add(new UserRolesViewModel()
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = await userManager.IsInRoleAsync(user, role.Name)
                });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageRoles(List<UserRolesViewModel> model, string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                //TODO: Crear constante y revisar modelo de manejo de errores
                ModelState.AddModelError(Constants.KeyErrors.ValidationError, "User not found");
            }

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                //TODO: Crear constante y revisar modelo de manejo de errores
                ModelState.AddModelError(Constants.KeyErrors.ValidationError, "Cannot remove user existing roles");
                return View(model);
            }

            var newRolesToAdd = model.Where(ur => ur.IsSelected).Select(ur => ur.RoleName).ToList();
            result = await userManager.AddToRolesAsync(user, newRolesToAdd);

            if (!result.Succeeded)
            {
                //TODO: Crear constante y revisar modelo de manejo de errores
                ModelState.AddModelError(Constants.KeyErrors.ValidationError, "Cannot add selected roles to the user");
                return View(model);
            }

            return RedirectToAction(nameof(Edit), new { id });
        }


        [HttpGet]
        public async Task<IActionResult> ManageClaims(string id)
        {
            ViewBag.UserId = id;
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                //TODO: Crear constante y revisar modelo de manejo de errores
                ModelState.AddModelError(Constants.KeyErrors.ValidationError, "User not found");
            }

            var assignedClaims = await userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel()
            {
                UserId = user.Id,
            };

            foreach (var claim in ClaimsStore.AllClaims)
            {
                UserClaimViewModel userClaimViewModel = new UserClaimViewModel()
                {
                    ClaimType = claim.Type
                };

                if (assignedClaims.Any(c => c.Type == claim.Type && c.Value == true.AsString()))
                {
                    userClaimViewModel.IsSelected = true;
                }
                model.Claims.Add(userClaimViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageClaims(UserClaimsViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                //TODO: Crear constante y revisar modelo de manejo de errores
                ModelState.AddModelError(Constants.KeyErrors.ValidationError, "User not found");
            }

            var claims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                //TODO: Crear constante y revisar modelo de manejo de errores
                ModelState.AddModelError(Constants.KeyErrors.ValidationError, "Cannot remove user existing claims");
                return View(model);
            }

            var newClaimsToAdd = model.Claims.Select(uc => new Claim(uc.ClaimType, uc.IsSelected.AsString() )).ToList();
            result = await userManager.AddClaimsAsync(user, newClaimsToAdd);

            if (!result.Succeeded)
            {
                //TODO: Crear constante y revisar modelo de manejo de errores
                ModelState.AddModelError(Constants.KeyErrors.ValidationError, "Cannot add selected claims to the user");
                return View(model);
            }

            return RedirectToAction(nameof(Edit), new { id = model.UserId });
        }

    }
}
