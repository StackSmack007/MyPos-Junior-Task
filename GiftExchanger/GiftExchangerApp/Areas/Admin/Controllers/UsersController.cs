using CommonLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceLibrary;
using System.Threading.Tasks;

namespace GiftExchangerApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = GlobalConstants.AdministratorRole)]
    public class UsersController : Controller
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var allUsers = await userService.GetUsersInfo().ToArrayAsync();
            return View(allUsers);
        }
    }
}