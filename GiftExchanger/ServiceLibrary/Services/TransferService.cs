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

namespace ServiceLibrary
{
    public class TransferService : ITransferService
    {
        private readonly UserManager<UserGE> _userManager;

        public TransferService(UserManager<UserGE> userManager)
        {
            this._userManager = userManager;
        }

        public async Task<UserTransferInfoDTOout> GetTransactionsByIdAsync(string userId) =>
            await _userManager.Users.Where(x => x.Id == userId).To<UserTransferInfoDTOout>().FirstOrDefaultAsync();

        public async Task GiveCreditsAsync(ClaimsPrincipal user, TransferDTOin dto)
        {
            bool recieverFound = await _userManager.Users
                .AnyAsync(x => x.UserName.ToLower() == dto.RecieverUnameOrPhone.ToLower() ||
                x.PhoneNumber == GlobalConstants.SanitizePhone(dto.RecieverUnameOrPhone));

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
                                                             x.PhoneNumber == GlobalConstants.SanitizePhone(dto.RecieverUnameOrPhone));

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
        }
    }
}
