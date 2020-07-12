using System;
using System.Linq;
using CommonLibrary;
using CommonLibrary.Extensions;
using Infrastructure.DTOS;
using Infrasturcture.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CommonLibrary.Interfaces;

namespace ServiceLibrary
{
    public class TransferService : ITransferService
    {
        private readonly UserManager<UserGE> _userManager;
        private readonly IRepository<CreditTransfer> transfersRepository;
        private readonly ICasheHandler casheHandler;

        public TransferService(UserManager<UserGE> userManager, IRepository<CreditTransfer> transfersRepository, ICasheHandler casheHandler)
        {
            this._userManager = userManager;
            this.transfersRepository = transfersRepository;
            this.casheHandler = casheHandler;
        }

        public IQueryable<TransferInfoDTOout> GetAllTransfersInfo() =>
           transfersRepository.All.Where(x => !x.IsDeleted).OrderByDescending(x => x.CreatedOn).To<TransferInfoDTOout>();
        public async Task<UserDashboardInfoDTOout> GetTransactionsUserIdAsync(string userId) =>
            await _userManager.Users.Where(x => x.Id == userId).To<UserDashboardInfoDTOout>().FirstOrDefaultAsync();

        public async Task<bool> IncreaseUserCreditsAsync(CreditAdditionDTOin dto)
        {
            using (var transaction = await transfersRepository.BeginTransactionAsync())
            {
                var userFd = await _userManager.FindByIdAsync(dto.RecieverId);
                if (userFd is null)
                {
                    return false;
                }
                userFd.CreditBalance += dto.Ammount;
                await _userManager.UpdateAsync(userFd);
                await transaction.CommitAsync();
            }

            return true;
        }

        public async Task TransferCreditsAsync(ClaimsPrincipal user, TransferDTOin dto)
        {
            using (var transaction = await transfersRepository.BeginTransactionAsync())
            {
                UserGE sender = await _userManager.GetUserAsync(user);
                if (sender.CreditBalance < dto.Ammount)
                {
                    throw new ArgumentOutOfRangeException(GlobalConstants.InsufficientFundsError);
                }

                UserGE reciever = _userManager.Users.FirstOrDefault(x => x.UserName.ToLower() == dto.RecieverUnameOrPhone.ToLower() ||
                                                             x.PhoneNumber == Helpers.SanitizePhone(dto.RecieverUnameOrPhone));

                if (reciever is null)
                {
                    throw new ArgumentOutOfRangeException(GlobalConstants.UserNotLocatedByPhoneOrUserNameError(dto.RecieverUnameOrPhone));
                }
                else if (reciever.Id == sender.Id)
                {
                    throw new ArgumentOutOfRangeException(GlobalConstants.AutoSendCreditsError);
                }

                sender.CreditBalance -= dto.Ammount;
                reciever.CreditBalance += dto.Ammount;

                var transfer = new CreditTransfer
                {
                    Sender = sender,
                    Reciever = reciever,
                    Ammount = dto.Ammount,
                    Comment = dto.Comment
                };
                sender.TransactionsSent.Add(transfer);
                await transfersRepository.SaveChangesAsync();
                await transaction.CommitAsync();
                await casheHandler.ClearDataAsync(GlobalConstants.StatisticsCasheName);
            }
        }
    }
}
