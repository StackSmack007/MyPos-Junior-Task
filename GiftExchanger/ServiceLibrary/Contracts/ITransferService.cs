using Infrastructure.DTOS;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ServiceLibrary
{
    public interface ITransferService
    {
        Task<UserTransferInfoDTOout> GetTransactionsByIdAsync(string userId);
        Task GiveCreditsAsync(ClaimsPrincipal user, TransferDTOin dto);
        IQueryable<TransferInfoDTOout> GetAllTransfersInfo();
    }
}
