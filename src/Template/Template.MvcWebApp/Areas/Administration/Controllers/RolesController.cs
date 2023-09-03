﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.EntityFrameworkCore;
using Template.Application.Identity;
using Template.Common;
using Template.Domain.Entities.Identity;
using Template.MvcWebApp.Areas.Administration.Models;
using Template.Security.Authorization;
using Template.Security.DataProtection;

namespace Template.MvcWebApp.Areas.Administration.Controllers
{
    [Authorize(Roles = RoleGroup.SuperadminAndAdminRoles)]
    public class RolesController : BaseAdministrationController
    {
        private readonly IDataProtector protector;

        public RolesController(IMediator mediator, UserManager userManager, RoleManager roleManager, IHtmlLocalizer localizer, IDataProtectionProvider dataProtectionProvider) : base(mediator, userManager, roleManager, localizer)
        {
            //TODO: Encrypt routvalues
            this.protector = dataProtectionProvider.CreateProtector(ProtectionPurpose.RoleIdRouteValue);
        }

        public async Task<IActionResult> Index()
        {
            var superadminRole = await roleManager.Roles.Where(r=> r.Name == Roles.Superadmin).ToListAsync();
            var allRoles = await roleManager.Roles.ToListAsync();

            var allRolesExceptSuperadmin = allRoles.Except(superadminRole).ToList();
            
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            List<Role> roles = await userManager.IsInRoleAsync(user, Roles.Superadmin) ? allRoles : allRolesExceptSuperadmin;

            return View(roles);
        }

        [HttpGet]
        [Authorize(Policy = Roles.Superadmin)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = Roles.Superadmin)]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                Role role = new Role()
                {
                    Name = model.RoleName
                };

                var result = await roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    //TODO: Crear constante y revisar modelo de manejo de errores
                    ModelState.AddModelError(Constants.KeyErrors.ValidationError, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                //TODO: Crear constante y revisar modelo de manejo de errores
                ModelState.AddModelError(Constants.KeyErrors.ValidationError, "Role not found");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
                Users = (await userManager.GetUsersInRoleAsync(role.Name)).Select(u => u.UserName).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                Role role = await roleManager.FindByIdAsync(model.Id);

                if (role == null)
                {
                    //TODO: Crear constante y revisar modelo de manejo de errores
                    ModelState.AddModelError(Constants.KeyErrors.ValidationError, "Role not found");
                }
                else
                {
                    role.Name = model.RoleName;

                    var result = await roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in result.Errors)
                    {
                        //TODO: Crear constante y revisar modelo de manejo de errores
                        ModelState.AddModelError(Constants.KeyErrors.ValidationError, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = Roles.Superadmin)]
        public async Task<IActionResult> Delete(string id)
        {
            //TODO: Agregar confirmación antes de eliminar un usuario
            //TODO: Agregar clases para soft delete a las entidades
            //TODO: Agregar auditoria a las entidades

            Role role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                //TODO: Crear constante y revisar modelo de manejo de errores
                ModelState.AddModelError(Constants.KeyErrors.ValidationError, "Role not found");
            }
            else
            {
                var result = await roleManager.DeleteAsync(role);

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

            return View(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> UsersInRole(string id)
        {
            ViewBag.RoleId = id;
            var role = await roleManager.FindByIdAsync(id);
            var loggedUser = await userManager.FindByNameAsync(User.Identity.Name);

            if (role == null)
            {
                //TODO: Crear constante y revisar modelo de manejo de errores
                ModelState.AddModelError(Constants.KeyErrors.ValidationError, "Role not found");
            }

            var model = new List<UserRoleViewModel>();

            var users = new List<User>();
            var allUsers = await userManager.Users.Where(u=> u.Id != loggedUser.Id).ToListAsync();

            var allUsersExceptLoggedUser = allUsers.Except(new List<User> { loggedUser}).ToList();
            users = await userManager.IsInRoleAsync(loggedUser, Roles.Superadmin) ? allUsers : allUsersExceptLoggedUser;


            foreach (var user in users)
            {
                model.Add(new UserRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsSelected = await userManager.IsInRoleAsync(user, role.Name)
                });
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> UsersInRole(List<UserRoleViewModel> model, string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ModelState.AddModelError(Constants.KeyErrors.ValidationError, "Role not found");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                var isInRole = await userManager.IsInRoleAsync(user, role.Name);
                if (model[i].IsSelected && !isInRole)
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && isInRole)
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction(nameof(Edit), new { id });
                }
            }

            return RedirectToAction(nameof(Edit), new { id });
        }
    }
}
