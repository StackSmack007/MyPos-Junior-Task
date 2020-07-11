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

        public TransferService(UserManager<UserGE> userManager, IRepository<CreditTransfer> transfersRepository)
        {
            this._userManager = userManager;
            this.transfersRepository = transfersRepository;
        }

        public IQueryable<TransferInfoDTOout> GetAllTransfersInfo() =>
           transfersRepository.All.Where(x => !x.IsDeleted).OrderByDescending(x => x.CreatedOn).To<TransferInfoDTOout>();
        public async Task<UserDashboardInfoDTOout> GetTransactionsUserIdAsync(string userId) =>
            await _userManager.Users.Where(x => x.Id == userId).To<UserDashboardInfoDTOout>().FirstOrDefaultAsync();

        public bool IncreaseUserCredits(CreditAdditionDTOin dto)
        {
            lock (LockObjects.UserBalanceObject)
            {
                var userFd = _userManager.FindByIdAsync(dto.RecieverId).GetAwaiter().GetResult();
                if (userFd is null)
                {
                    return false;
                }

                userFd.CreditBalance += dto.Ammount;
                _userManager.UpdateAsync(userFd).GetAwaiter().GetResult();
            }

            return true;
        }

        public async Task TransferCreditsAsync(ClaimsPrincipal user, TransferDTOin dto)
        {
            bool recieverFound = await _userManager.Users
                .AnyAsync(x => x.UserName.ToLower() == dto.RecieverUnameOrPhone.ToLower() ||
                x.PhoneNumber == Helpers.SanitizePhone(dto.RecieverUnameOrPhone));

            if (!recieverFound)
            {
                throw new ArgumentOutOfRangeException(GlobalConstants.UserNotLocatedByPhoneOrUserNameError(dto.RecieverUnameOrPhone));
            }

            UserGE sender = null;
            UserGE reciever = null;

            lock (LockObjects.UserBalanceObject)
            {
                sender = _userManager.GetUserAsync(user).GetAwaiter().GetResult();
                if (sender.CreditBalance < dto.Ammount)
                {
                    throw new ArgumentOutOfRangeException(GlobalConstants.InsufficientFundsError);
                }

                reciever = _userManager.Users.FirstOrDefault(x => x.UserName.ToLower() == dto.RecieverUnameOrPhone.ToLower() ||
                                                             x.PhoneNumber == Helpers.SanitizePhone(dto.RecieverUnameOrPhone));

                if (reciever.Id == sender.Id)
                {
                    throw new ArgumentOutOfRangeException(GlobalConstants.AutoSendCreditsError);
                }

                sender.CreditBalance -= dto.Ammount;
                reciever.CreditBalance += dto.Ammount;
                _userManager.UpdateAsync(sender).GetAwaiter().GetResult();
                _userManager.UpdateAsync(reciever).GetAwaiter().GetResult();
            }

            var transaction = new CreditTransfer
            {
                Sender = sender,
                Reciever = reciever,
                Ammount = dto.Ammount,
                Comment = dto.Comment
            };
            sender.TransactionsSent.Add(transaction);
            await _userManager.UpdateAsync(sender);
            CommonLibrary.Cashe.CasheData.ResetData(GlobalConstants.StatisticsStore);
        }
    }
}
