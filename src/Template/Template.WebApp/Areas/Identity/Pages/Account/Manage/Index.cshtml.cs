using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using Template.Application.Features.IdentityContext.Services;
using Template.Domain.Entities.Identity;

namespace Template.WebApp.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager _userManager;
        private readonly SignInManager _signInManager;
        private readonly IStringLocalizer _localizer;

        public IndexModel(
            UserManager userManager,
            SignInManager signInManager, IStringLocalizer localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
        }

        /// <summary>
        ///
        ///
        /// </summary>
        [Display(Name = "Username")]
        public string Username { get; set; }

        /// <summary>
        ///
        ///
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

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
            [Phone(ErrorMessage = "The {0} field is not a valid phone number.")]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                //TODO: Localize
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
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

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = _localizer.GetString("Identity_Account_Manage_Index_StatusMessage_Error");
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = _localizer.GetString("Identity_Account_Manage_Index_StatusMessage_Ok");
            return RedirectToPage();
        }
    }
}
