// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using Template.Application.Identity;

namespace Template.MvcWebApp.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager _userManager;
        private readonly IStringLocalizer _localizer;

        public ConfirmEmailModel(UserManager userManager, IStringLocalizer localizer)
        {
            _userManager = userManager;
            _localizer = localizer;
        }

        /// <summary>
        ///
        ///
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
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
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? _localizer.GetString("Identity_Account_ConfirmEmail_StatusMessage_Ok"): _localizer.GetString("Identity_Account_ConfirmEmail_StatusMessage_Error");
            return Page();
        }
    }
}
