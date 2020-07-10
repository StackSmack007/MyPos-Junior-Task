using Infrastructure.DTOS;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceLibrary
{
    public interface IUserService
    {
        Task<bool> PhoneInUseByOtherUserAsync(string myUserName, string phoneNumberFormated);
        Task<bool> UserNameTakenAsync(string userName);
        Task<bool> PhoneNumberInUseAsync(string phoneNumber);
        IQueryable<UserDataDTOout> GetUsersInfo();

        Task<decimal> GetUserCreditsByIdAsync(string userId);
    }
}
