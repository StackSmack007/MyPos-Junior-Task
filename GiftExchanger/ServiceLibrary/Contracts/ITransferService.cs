using Infrastructure.DTOS;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ServiceLibrary
{
    public interface ITransferService
    {
        IQueryable<TransferInfoDTOout> GetAllTransfersInfo();
        Task<UserDashboardInfoDTOout> GetTransactionsUserIdAsync(string userId);
        Task TransferCreditsAsync(ClaimsPrincipal user, TransferDTOin dto);
        Task<bool> IncreaseUserCreditsAsync(CreditAdditionDTOin dto);
    }
}
