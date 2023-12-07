using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Template.Application.Features.Identity;

namespace Template.WebApp.Areas.Identity.Pages.Account.Manage
{
    public class TwoFactorAuthenticationModel : PageModel
    {
        private readonly UserManager _userManager;
        private readonly SignInManager _signInManager;
        private readonly ILogger<TwoFactorAuthenticationModel> _logger;
        private readonly IStringLocalizer _localizer;

        public TwoFactorAuthenticationModel(
            UserManager userManager, SignInManager signInManager, ILogger<TwoFactorAuthenticationModel> logger, IStringLocalizer localizer)
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
        public bool HasAuthenticator { get; set; }

        /// <summary>
        ///
        ///
        /// </summary>
        public int RecoveryCodesLeft { get; set; }

        /// <summary>
        ///
        ///
        /// </summary>
        [BindProperty]
        public bool Is2faEnabled { get; set; }

        /// <summary>
        ///
        ///
        /// </summary>
        public bool IsMachineRemembered { get; set; }

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

            HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null;
            Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
            IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user);
            RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user);

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

            await _signInManager.ForgetTwoFactorClientAsync();
            StatusMessage = _localizer.GetString("Identity_Account_Manage_TwoFactorAuthentication_StatusMessage_BrowserForgotten");
            return RedirectToPage();
        }
    }
}
