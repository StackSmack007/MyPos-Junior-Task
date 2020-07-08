using Infrastructure.DTOS;
using System.Threading.Tasks;

namespace ServiceLibrary
{
    public interface IStatisticsService
    {
        Task<AppStatisticsDTOout> GetStatisticsAsync();
    }
}