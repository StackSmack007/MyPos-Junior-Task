using CommonLibrary;
using CommonLibrary.Interfaces;
using Infrastructure.DTOS;
using Infrasturcture.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceLibrary
{
    public class StatisticsService : IStatisticsService
    {
        private readonly UserManager<UserGE> _userManager;
        private readonly IRepository<CreditTransfer> transfersRepository;
        private readonly IMemoryCache cache;

        public StatisticsService(UserManager<UserGE> userManager, IRepository<CreditTransfer> transfersRepository, IMemoryCache cache)
        {
            this._userManager = userManager;
            this.transfersRepository = transfersRepository;
            this.cache = cache;
        }

        public async Task<AppStatisticsDTOout> GetStatisticsAsync()
        {
            if (cache.TryGetValue(GlobalConstants.StatisticsCasheName, out AppStatisticsDTOout resFd))
            {
                return resFd;
            }

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

            cache.Set(GlobalConstants.StatisticsCasheName, result);
            return result;
        }
    }
}