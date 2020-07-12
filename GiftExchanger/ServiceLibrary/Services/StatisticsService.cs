using CommonLibrary;
using CommonLibrary.Interfaces;
using Infrastructure.DTOS;
using Infrasturcture.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceLibrary
{
    public class StatisticsService : IStatisticsService
    {

        private readonly UserManager<UserGE> _userManager;
        private readonly IRepository<CreditTransfer> transfersRepository;
        private readonly ICasheHandler casheHandler;

        public StatisticsService(UserManager<UserGE> userManager, IRepository<CreditTransfer> transfersRepository, ICasheHandler casheHandler)
        {
            this._userManager = userManager;
            this.transfersRepository = transfersRepository;
            this.casheHandler = casheHandler;
        }

        public async Task<AppStatisticsDTOout> GetStatisticsAsync()
        {
            if (!await casheHandler.HasDataAsync(GlobalConstants.StatisticsCasheName))
            {
                UserGE[] users = await _userManager.Users.ToArrayAsync();
                var admins = new HashSet<string>();

                foreach (var user in users)
                {
                    if (await _userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRole))
                    {
                        admins.Add(user.UserName);
                    }
                }

                var result = new AppStatisticsDTOout
                {
                    AllUserNames = users.Select(x => x.UserName).ToArray(),
                    AdminUserNames = admins,
                    TotalTransactions = await transfersRepository.All.Where(x => !x.IsDeleted).CountAsync(),
                    TotalTransferedCredits = await transfersRepository.All.Where(x => !x.IsDeleted).SumAsync(x => x.Ammount),
                };

                await casheHandler.SetDataAsync(GlobalConstants.StatisticsCasheName, result);
            }

            return await casheHandler.GetData<AppStatisticsDTOout>(GlobalConstants.StatisticsCasheName);
        }
    }
}