using CommonLibrary;
using Infrastructure.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLibrary;
using System.Threading.Tasks;

namespace GiftExchangerApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class TransfersController : Controller
    {
        private readonly ITransferService transferService;

        public TransfersController(ITransferService transferService)
        {
            this.transferService = transferService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await this.transferService.GetAllTransfersInfo().ToArrayAsync();
            return View(data);
        }


        [HttpPost]
        public IActionResult AddCredits(CreditAdditionDTOin dto)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", "Users", new { Area = "Admin" });

            transferService.IncreaseUserCredits(dto);
            return RedirectToAction("Index", "Users", new { Area = "Admin" });
        }
    }
}