﻿using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Template.Application.Features.IdentityContext;

namespace Template.WebApp.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IStringLocalizer _localizer;

        public ForgotPasswordModel(UserManager userManager, IEmailSender emailSender, IStringLocalizer localizer)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _localizer = localizer;
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
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                Input.Email,
                    _localizer.GetString("Identity_Account_ForgotPassword_ConfirmEmailSubject"),
                    _localizer.GetString("Identity_Account_ForgotPassword_ConfirmEmailBody", HtmlEncoder.Default.Encode(callbackUrl)));

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
