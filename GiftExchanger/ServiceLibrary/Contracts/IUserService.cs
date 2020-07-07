using System.Threading.Tasks;

namespace ServiceLibrary
{
    public  interface IUserService
    {
        Task<bool> PhoneInUseByOtherUser(string myUserName, string phoneNumberFormated);
        Task<bool> UserNameTaken(string userName);
        Task<bool> PhoneNumberInUse(string phoneNumber);
    }
}
