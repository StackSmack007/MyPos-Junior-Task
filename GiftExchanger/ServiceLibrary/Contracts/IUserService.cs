using Infrastructure.DTOS;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceLibrary
{
    public interface IUserService
    {
        Task<bool> PhoneInUseByOtherUser(string myUserName, string phoneNumberFormated);
        Task<bool> UserNameTaken(string userName);
        Task<bool> PhoneNumberInUse(string phoneNumber);
        IQueryable<UserDataDTOout> GetUsersInfo();
        Task<bool> AddCreditsAsync(CreditAdditionDTOin dto);
    }
}
