using CommonLibrary;
using CommonLibrary.Extensions;
using Infrastructure.DTOS;
using Infrasturcture.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
                .AnyAsync(x => x.UserName.ToLower() == dto.RecieverPhoneOrUsername.ToLower() ||
                x.PhoneNumber == GlobalConstants.SanitizePhone(dto.RecieverPhoneOrUsername));

            if (!recieverFound)
            {
                throw new ArgumentOutOfRangeException($"User with phone or username {dto.RecieverPhoneOrUsername} was not found!");
            }

            UserGE sender = null;
            UserGE reciever = null;

            lock (LockObjects.UserBalanceObject)
            {
                sender = _userManager.GetUserAsync(user).GetAwaiter().GetResult();
                if (sender.CreditBalance < dto.Ammount)
                {
                    throw new ArgumentOutOfRangeException($"Insufficient funds");
                }

                reciever = _userManager.Users.FirstOrDefault(x => x.UserName.ToLower() == dto.RecieverPhoneOrUsername.ToLower() ||
                                                             x.PhoneNumber == GlobalConstants.SanitizePhone(dto.RecieverPhoneOrUsername));
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
