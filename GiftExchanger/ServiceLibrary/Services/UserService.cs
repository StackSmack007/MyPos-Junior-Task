using CommonLibrary.Extensions;
using Infrastructure.DTOS;
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

        public IQueryable<UserDataDTOout> GetUsersInfo() =>
            this._userManager.Users.To<UserDataDTOout>();

        public async Task<bool> AddCreditsAsync(CreditAdditionDTOin dto)
        {
            var userFd = await _userManager.FindByIdAsync(dto.RecieverId);
            if (userFd is null)
            {
                return false;
            }

            userFd.CreditBalance += dto.Ammount;
            await _userManager.UpdateAsync(userFd);
            return true;
        }

    }
}