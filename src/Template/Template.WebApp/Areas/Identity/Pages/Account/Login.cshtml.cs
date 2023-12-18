using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using Template.Application.Features.Identity;
using Template.Common;
using Template.Domain.Entities.Identity;

namespace Template.WebApp.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly UserManager _userManager;
        private readonly SignInManager _signInManager;
        private readonly IStringLocalizer _localizer;
        private readonly ILogger<LoginModel> _logger;
        private readonly IWebHostEnvironment _environment;

        public LoginModel(UserManager userManager, SignInManager signInManager, IStringLocalizer localizer, ILogger<LoginModel> logger, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
            _logger = logger;
            _environment = environment;
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
            [RegularExpression(pattern: RegExPatterns.Validators.UsernameOrEmail, ErrorMessage = "The {0} field is not a valid username or e-mail address.")]
            [Display(Name = "Username or email")]
            public string UsernameOrEmail { get; set; }

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
                User user;
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                if (Input.UsernameOrEmail.Contains('@'))
                    user = await _userManager.FindByEmailAsync(Input.UsernameOrEmail);
                else
                    user = await _userManager.FindByNameAsync(Input.UsernameOrEmail);

                if (user == null)
                {
                    //TODO: Crear constante y revisar modelo de manejo de errores
                    ModelState.AddModelError(string.Empty, _localizer.GetString("Identity_Account_Login_ModelState"));
                }

                var isLockoutEnabled = true;//_environment.IsProduction();
                var result = await _signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, lockoutOnFailure: isLockoutEnabled);
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
