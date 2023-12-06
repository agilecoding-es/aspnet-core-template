using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using Template.Application.Features.Identity;

namespace Template.MvcWebApp.Areas.Identity.Pages.Account
{
    public class LoginWith2faModel : PageModel
    {
        private readonly SignInManager _signInManager;
        private readonly UserManager _userManager;
        private readonly ILogger<LoginWith2faModel> _logger;
        private readonly IStringLocalizer _localizer;

        public LoginWith2faModel(
            SignInManager signInManager,
            UserManager userManager,
            ILogger<LoginWith2faModel> logger,
            IStringLocalizer localizer)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
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
        public bool RememberMe { get; set; }

        /// <summary>
        ///
        ///
        /// </summary>
        public string ReturnUrl { get; set; }

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
            [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Authenticator code")]
            public string TwoFactorCode { get; set; }

            /// <summary>
            ///
            ///
            /// </summary>
            [Display(Name = "Remember this machine")]
            public bool RememberMachine { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                //TODO: Localize
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            ReturnUrl = returnUrl;
            RememberMe = rememberMe;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                //TODO: Localize
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            var authenticatorCode = Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, Input.RememberMachine);

            var userId = await _userManager.GetUserIdAsync(user);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
                return LocalRedirect(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                return RedirectToPage("./Lockout");
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
                ModelState.AddModelError(string.Empty, _localizer.GetString("Identity_Account_LoginWith2fa_ModelState"));
                return Page();
            }
        }
    }
}
