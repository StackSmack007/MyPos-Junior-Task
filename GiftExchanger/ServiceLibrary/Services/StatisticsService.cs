using CommonLibrary;
using CommonLibrary.Interfaces;
using Infrastructure.DTOS;
using Infrasturcture.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceLibrary
{
    public class StatisticsService : IStatisticsService
    {
        private readonly UserManager<UserGE> _userManager;
        private readonly IRepository<CreditTransfer> transfersRepository;

        public StatisticsService(UserManager<UserGE> userManager, IRepository<CreditTransfer> transfersRepository)
        {
            this._userManager = userManager;
            this.transfersRepository = transfersRepository;
        }


        public async Task<AppStatisticsDTOout> GetStatisticsAsync()
        {
            UserGE[] users = await this._userManager.Users.ToArrayAsync();

            var adminsCount = 0;


            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRole))
                {
                    adminsCount++;
                }
            }

            var result = new AppStatisticsDTOout
            {
                TotalUsersCount = users.Count(),
                AdminsUsersCount = adminsCount,
                TotalTransactions = await transfersRepository.All.Where(x => !x.IsDeleted).CountAsync(),
                TotalTransferedCredits = await transfersRepository.All.Where(x => !x.IsDeleted).SumAsync(x => x.Ammount),
            };

            return result;
            //  db.Users.Count(x => x.Roles.Select(k => k.RoleId).Contains(g.Key.Id))
        }



    }
}
