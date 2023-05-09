// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Template.Application.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Identity;
using Template.Domain.Entities.Identity;

namespace Template.MvcWebApp.Areas.Identity.Pages.Account.Manage
{
    public class ExternalLoginsModel : PageModel
    {
        private readonly UserManager _userManager;
        private readonly SignInManager _signInManager;
        private readonly IUserStore<User> _userStore;
        private readonly IStringLocalizer _localizer;

        public ExternalLoginsModel(
            UserManager userManager,
            SignInManager signInManager,
            IUserStore<User> userStore, IStringLocalizer localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userStore = userStore;
            _localizer = localizer;
        }

        /// <summary>
        ///
        ///
        /// </summary>
        public IList<UserLoginInfo> CurrentLogins { get; set; }

        /// <summary>
        ///
        ///
        /// </summary>
        public IList<AuthenticationScheme> OtherLogins { get; set; }

        /// <summary>
        ///
        ///
        /// </summary>
        public bool ShowRemoveButton { get; set; }

        /// <summary>
        ///
        ///
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                //TODO: Localize
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            CurrentLogins = await _userManager.GetLoginsAsync(user);
            OtherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
                .Where(auth => CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
                .ToList();

            string passwordHash = null;
            if (_userStore is IUserPasswordStore<User> userPasswordStore)
            {
                passwordHash = await userPasswordStore.GetPasswordHashAsync(user, HttpContext.RequestAborted);
            }

            ShowRemoveButton = passwordHash != null || CurrentLogins.Count > 1;
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveLoginAsync(string loginProvider, string providerKey)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                //TODO: Localize
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);
            if (!result.Succeeded)
            {
                StatusMessage = _localizer.GetString("Identity_Account_Manage_ExternalLogins_StatusMessage_ExternalLoginNotRemoved");
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = _localizer.GetString("Identity_Account_Manage_ExternalLogins_StatusMessage_ExternalLoginRemoved");
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostLinkLoginAsync(string provider)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Page("./ExternalLogins", pageHandler: "LinkLoginCallback");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetLinkLoginCallbackAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                //TODO: Localize
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var info = await _signInManager.GetExternalLoginInfoAsync(userId);
            if (info == null)
            {
                //TODO: Localize
                throw new InvalidOperationException($"Unexpected error occurred loading external login info.");
            }

            var result = await _userManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                StatusMessage = _localizer.GetString("Identity_Account_Manage_ExternalLogins_StatusMessage_ExternalLoginNotAdded");
                return RedirectToPage();
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            StatusMessage = _localizer.GetString("Identity_Account_Manage_ExternalLogins_StatusMessage_ExternalLoginAdded");
            return RedirectToPage();
        }
    }
}
