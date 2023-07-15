using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using System.Security.Claims;
using Template.Application.Identity;
using Template.Authorization.Constants;
using Template.Common;
using Template.Domain.Entities.Identity;
using Template.MvcWebApp.Areas.Administration.Models;

namespace Template.MvcWebApp.Areas.Administration.Controllers
{
    [Authorize(Roles = Roles.SuperadminAndAdminRoles)]
    public class UsersController : BaseAdministrationController
    {
        public UsersController(IMediator mediator, UserManager userManager, RoleManager roleManager, IHtmlLocalizer localizer) : base(mediator, userManager, roleManager, localizer)
        {

        }

        public IActionResult Index()
        {
            var users = userManager.Users.ToList();

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                //TODO: Crear constante y revisar modelo de manejo de errores
                ModelState.AddModelError(Constants.KeyErrors.ValidationError.Value, "User not found");
            }

            var model = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Claims = (await userManager.GetClaimsAsync(user)).Select(c => c.Type).ToList(),
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
                    ModelState.AddModelError(Constants.KeyErrors.ValidationError.Value, "User not found");
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
                        ModelState.AddModelError(Constants.KeyErrors.ValidationError.Value, error.Description);
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
                ModelState.AddModelError(Constants.KeyErrors.ValidationError.Value, "User not found");
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
                    ModelState.AddModelError(Constants.KeyErrors.ValidationError.Value, error.Description);
                }
            }

            return View(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ManageRoles(string id)
        {
            ViewBag.UserId = id;
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                //TODO: Crear constante y revisar modelo de manejo de errores
                ModelState.AddModelError(Constants.KeyErrors.ValidationError.Value, "User not found");
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
                ModelState.AddModelError(Constants.KeyErrors.ValidationError.Value, "User not found");
            }

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                //TODO: Crear constante y revisar modelo de manejo de errores
                ModelState.AddModelError(Constants.KeyErrors.ValidationError.Value, "Cannot remove user existing roles");
                return View(model);
            }

            var newRolesToAdd = model.Where(ur => ur.IsSelected).Select(ur => ur.RoleName).ToList();
            result = await userManager.AddToRolesAsync(user, newRolesToAdd);

            if (!result.Succeeded)
            {
                //TODO: Crear constante y revisar modelo de manejo de errores
                ModelState.AddModelError(Constants.KeyErrors.ValidationError.Value, "Cannot add selected roles to the user");
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
                ModelState.AddModelError(Constants.KeyErrors.ValidationError.Value, "User not found");
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

                if (assignedClaims.Any(c => c.Type == claim.Type))
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
                ModelState.AddModelError(Constants.KeyErrors.ValidationError.Value, "User not found");
            }

            var claims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                //TODO: Crear constante y revisar modelo de manejo de errores
                ModelState.AddModelError(Constants.KeyErrors.ValidationError.Value, "Cannot remove user existing claims");
                return View(model);
            }

            var newClaimsToAdd = model.Claims.Where(uc => uc.IsSelected).Select(uc => new Claim(uc.ClaimType, uc.ClaimType)).ToList();
            result = await userManager.AddClaimsAsync(user, newClaimsToAdd);

            if (!result.Succeeded)
            {
                //TODO: Crear constante y revisar modelo de manejo de errores
                ModelState.AddModelError(Constants.KeyErrors.ValidationError.Value, "Cannot add selected claims to the user");
                return View(model);
            }

            return RedirectToAction(nameof(Edit), new { id = model.UserId });
        }

    }
}
