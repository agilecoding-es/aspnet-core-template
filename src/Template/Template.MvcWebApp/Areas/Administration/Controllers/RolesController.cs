using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Template.Application.Identity;
using Template.Configuration;
using Template.Domain.Entities.Identity;
using Template.Domain.Entities.Shared;
using Template.MvcWebApp.Areas.Administration.Models;

namespace Template.MvcWebApp.Areas.Administration.Controllers
{
    public class RolesController : BaseAdministrationController
    {
        public RolesController(IMediator mediator, UserManager userManager, RoleManager roleManager, IHtmlLocalizer localizer) : base(mediator, userManager, roleManager, localizer)
        {

        }

        public IActionResult Index()
        {
            var roles = roleManager.Roles.ToList();

            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
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
                    ModelState.AddModelError(Constants.KeyErrors.VALIDATION_ERROR, error.Description);
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
                ModelState.AddModelError(Constants.KeyErrors.VALIDATION_ERROR, "Role not found");
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
                    ModelState.AddModelError(Constants.KeyErrors.VALIDATION_ERROR, "Role not found");
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
                        ModelState.AddModelError(Constants.KeyErrors.VALIDATION_ERROR, error.Description);
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UsersInRole(string id)
        {
            ViewBag.RoleId = id;
            var role = await roleManager.FindByIdAsync(id);
            var users = userManager.Users.ToList();

            if (role == null)
            {
                ModelState.AddModelError(Constants.KeyErrors.VALIDATION_ERROR, "Role not found");
            }

            var model = new List<UserRoleViewModel>();
            foreach (var user in userManager.Users)
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
                ModelState.AddModelError(Constants.KeyErrors.VALIDATION_ERROR, "Role not found");
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

                if(result.Succeeded)
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
