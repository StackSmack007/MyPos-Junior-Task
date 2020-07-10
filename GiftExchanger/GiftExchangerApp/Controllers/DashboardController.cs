using CommonLibrary;
using CommonLibrary.Extensions;
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
            var result = await transferService.GetTransactionsUserIdAsync(User.Id());
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
                ViewData["Error"] = GlobalConstants.GeneralError(ModelState.Values.SelectMany(v => v.Errors).First().ErrorMessage);
                return View(dto);
            }

            try
            {
                await transferService.TransferCreditsAsync(User, dto);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                ViewData["Error"] = ex.ParamName;
                return View(dto);
            }

            return RedirectToAction("Index");
        }
    }
}