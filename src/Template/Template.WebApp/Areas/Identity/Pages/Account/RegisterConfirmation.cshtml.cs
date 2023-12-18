﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Template.Application.Features.Identity;

namespace Template.WebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IEmailSender _sender;

        public RegisterConfirmationModel(UserManager userManager, RoleManager roleManager, IEmailSender sender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _sender = sender;
        }

        /// <summary>
        ///
        ///
        /// </summary>
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        ///
        ///
        /// </summary>
        public bool DisplayConfirmAccountLink { get; set; }

        /// <summary>
        ///
        ///
        /// </summary>
        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }
            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                //TODO: Localize
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;
#if DEBUG
            // Once you add a real email sender, you should remove this code that lets you confirm the account
            DisplayConfirmAccountLink = true;
            if (DisplayConfirmAccountLink)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme);
            }
#endif

            return Page();
        }
    }
}
