using Infrastructure.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLibrary;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GiftExchangerApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ITransferService transferService;

        public DashboardController(ITransferService transferService)
        {
            this.transferService = transferService;
        }

        public async Task<IActionResult> Index() //TO DO Pagination
        {
            string userId = User.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier"))?.Value;
            var result = await transferService.GetTransactionsByIdAsync(userId);
            return View(result);
        }

        public IActionResult SendGift()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendGiftAsync(TransferDTOin dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await transferService.GiveCreditsAsync(User, dto);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                ViewData["Error"] = ex.Message;
                return View(dto);
            }

            return RedirectToAction("Index");
        }
    }
}