using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Infrasturcture.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using CommonLibrary;
using Microsoft.EntityFrameworkCore;

namespace GiftExchangerApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<UserGE> _userManager;
        private readonly SignInManager<UserGE> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly string ambiguousMSG = "Error: UserName/GSM or password mismatch";

        public LoginModel(SignInManager<UserGE> signInManager,
            ILogger<LoginModel> logger,
            UserManager<UserGE> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required, StringLength(16, MinimumLength = 4)]
            [Display(Name = "Username or Phone")]
            public string UnameOrPhone { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var userFd = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == Input.UnameOrPhone ||
                                                                          x.PhoneNumber == Helpers.SanitizePhone(Input.UnameOrPhone));

                if (userFd != null && 
                    (await _signInManager.PasswordSignInAsync(userFd, Input.Password, Input.RememberMe, lockoutOnFailure: false)).Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    StatusMessage = ambiguousMSG;
                    return Page();
                }
            }

            return Page();
        }
    }
}
