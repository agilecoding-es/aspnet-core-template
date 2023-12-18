using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Template.Application.Features.Identity;

namespace Template.WebApp.Areas.Identity.Pages.Account.Manage
{
    public class ResetAuthenticatorModel : PageModel
    {
        private readonly UserManager _userManager;
        private readonly SignInManager _signInManager;
        private readonly ILogger<ResetAuthenticatorModel> _logger;
        private readonly IStringLocalizer _localizer;

        public ResetAuthenticatorModel(
            UserManager userManager,
            SignInManager signInManager,
            ILogger<ResetAuthenticatorModel> logger, IStringLocalizer localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _localizer = localizer;
        }

        /// <summary>
        ///
        ///
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                //TODO: Localize
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                //TODO: Localize
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await _userManager.SetTwoFactorEnabledAsync(user, false);
            await _userManager.ResetAuthenticatorKeyAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            _logger.LogInformation("User with ID '{UserId}' has reset their authentication app key.", user.Id);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = _localizer.GetString("Identity_Account_Manage_ResetAuthenticator_StatusMessage_AuthenticatorAppReset");

            return RedirectToPage("./EnableAuthenticator");
        }
    }
}
