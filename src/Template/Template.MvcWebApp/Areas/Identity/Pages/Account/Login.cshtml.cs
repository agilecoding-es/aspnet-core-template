// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Template.Application.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Template.MvcWebApp.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Identity;

namespace Template.MvcWebApp.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager _signInManager;
        private readonly IStringLocalizer _localizer;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager signInManager, IStringLocalizer localizer, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _localizer = localizer;
            _logger = logger;
        }

        /// <summary>
        ///
        ///
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///
        ///
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///
        ///
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///
        ///
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///
        ///
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///
            ///
            /// </summary>
            [Required(ErrorMessage = "The {0} field is required.")]
            [EmailAddress(ErrorMessage = "The {0} field is not a valid e-mail address.")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///
            ///
            /// </summary>
            [Required(ErrorMessage = "The {0} field is required.")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///
            ///
            /// </summary>
            [Display(Name = "Remember me")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, _localizer.GetString("Identity_Account_Login_ModelState"));
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
