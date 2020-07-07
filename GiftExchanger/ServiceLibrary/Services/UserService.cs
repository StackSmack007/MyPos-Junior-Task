using Infrasturcture.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceLibrary
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserGE> _userManager;

        public UserService(UserManager<UserGE> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> PhoneInUseByOtherUser(string myUserName, string phoneNumberFormated) =>
           await _userManager.Users.AnyAsync(x => x.UserName != myUserName && x.PhoneNumber == phoneNumberFormated);

        public async Task<bool> PhoneNumberInUse(string phoneNumber) =>
            await _userManager.Users.Select(x => x.PhoneNumber).AnyAsync(x => x == phoneNumber);

        public async Task<bool> UserNameTaken(string userName) =>
            await _userManager.Users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower());
    }
}