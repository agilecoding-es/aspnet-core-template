// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Template.Application.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;

namespace Template.MvcWebApp.Areas.Identity.Pages.Account
{
    public class ConfirmEmailChangeModel : PageModel
    {
        private readonly UserManager _userManager;
        private readonly SignInManager _signInManager;
        private readonly IStringLocalizer _localizer;

        public ConfirmEmailChangeModel(UserManager userManager, SignInManager signInManager, IStringLocalizer localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
        }

        /// <summary>
        ///
        ///
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string email, string code)
        {
            if (userId == null || email == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                //TODO: Localize
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ChangeEmailAsync(user, email, code);
            if (!result.Succeeded)
            {
                StatusMessage = _localizer.GetString("Identity_Account_Manage_ConfirmEmailChange_StatusMessage_ErrorEmail");
                return Page();
            }

            // In our UI email and user name are one and the same, so when we update the email
            // we need to update the user name.
            var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
            if (!setUserNameResult.Succeeded)
            {
                StatusMessage = _localizer.GetString("Identity_Account_Manage_ConfirmEmailChange_StatusMessage_ErrorUserName");
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = _localizer.GetString("Identity_Account_Manage_ConfirmEmailChange_StatusMessage_Ok");
            return Page();
        }
    }
}