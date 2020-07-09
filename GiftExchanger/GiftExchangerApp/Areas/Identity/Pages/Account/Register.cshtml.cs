using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Infrasturcture.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using CommonLibrary;
using ServiceLibrary;
using System.Threading;

namespace GiftExchangerApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<UserGE> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UserGE> _userManager;
        private readonly IUserService userService;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<UserGE> userManager,
            SignInManager<UserGE> signInManager,
            RoleManager<IdentityRole> roleManager,
            IUserService userService,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            this.userService = userService;
            _logger = logger;
            StatusMessages = new List<string>();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]

        public string ReturnUrl { get; set; }

        public IList<string> StatusMessages { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required, StringLength(16, MinimumLength = 4)]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required, RegularExpression(@"(\d[\s\-]*){12}", ErrorMessage = "Invalid Phone number: Must contain 12 digits! Can include dashes and spaces!")]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            //  StatusMessages = new List<string>();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var phoneNumber = GlobalConstants.SanitizePhone(Input.PhoneNumber);
                if (await userService.PhoneNumberInUse(phoneNumber))
                {
                    StatusMessages.Add(GlobalConstants.PhoneAlreadyUsedError(phoneNumber));
                    return Page();
                }

                var user = new UserGE { UserName = Input.UserName, PhoneNumber = phoneNumber };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    var adminRoleName = GlobalConstants.AdministratorRole;
                    await EnsureRoleExists(adminRoleName);
                    await AssignRoleToFirstNUsers(GlobalConstants.AdminsCount, user, adminRoleName);

                    _logger.LogInformation("User created a new account with password.");
                    StatusMessages.Add("Successfull Registration!");
                    Thread.Sleep(1000);
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        // return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: true);
                        return LocalRedirect(returnUrl);
                    }
                }

                StatusMessages = result.Errors.Select(x => "Error: " + x.Description).ToList();
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
        private async Task EnsureRoleExists(string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private async Task AssignRoleToFirstNUsers(int n, UserGE user, string role)
        {
            bool shouldHaveRole = _userManager.Users.Count() <= n;
            if (shouldHaveRole)
            {
                await _userManager.AddToRoleAsync(user, role);
            }
        }
    }
}