using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CommonLibrary;
using Infrasturcture.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLibrary;

namespace GiftExchangerApp.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<UserGE> _userManager;
        private readonly IUserService userService;
        private readonly SignInManager<UserGE> _signInManager;

        public IndexModel(
            UserManager<UserGE> userManager, 
            IUserService userService,
            SignInManager<UserGE> signInManager)
        {
            _userManager = userManager;
            this.userService = userService;
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string UserName { get; set; }

            [Required, RegularExpression(@"(\d[\s\-]*){12}", ErrorMessage = "Invalid Phone number: Must contain 12 digits! Can include dashes and spaces!")]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            public decimal Balance { get; set; }
        }

        private async Task LoadAsync(UserGE user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = (await _userManager.GetPhoneNumberAsync(user)).Substring(1);

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                UserName = userName,
                Balance = user.CreditBalance
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
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
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var gsmRecievedFormated = Helpers.SanitizePhone(Input.PhoneNumber);
            if (gsmRecievedFormated == user.PhoneNumber)
            {
                StatusMessage = GlobalConstants.PhoneAlreadySetError(gsmRecievedFormated);
                return Page();
            }

            if (await userService.PhoneInUseByOtherUserAsync(Input.UserName,gsmRecievedFormated))
            {
                StatusMessage = GlobalConstants.PhoneAlreadyUsedError(gsmRecievedFormated);
                return Page();
            }

            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, gsmRecievedFormated);
            if (!setPhoneResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to set phone number.";
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your Phone number has been updated";
            return RedirectToPage();
        }
    }
}
