using FamilyTreeBackend.Core.Application.Operation.Models;
using FamilyTreeBackend.Infrastructure.Persistence.Role;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Operation.Services;
using System.Threading.Tasks;

namespace Operation.Controllers
{
    [Authorize(Roles = ApplicationUserRoles.Admin)]
    public class UsersController : Controller
    {
        private readonly IWebAccessUserService _userService;

        public UsersController(IWebAccessUserService userService)
        {
            _userService = userService;
        }

        // GET: UserController
        public async Task<ActionResult> Index(string filter)
        {
            var result = await _userService.FindUserByFilter(filter);
            return View(result);
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var result = await _userService.GetUserUpdateInfo(id);
            return View(result);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, WebAccessUserUpdateModel collection)
        {
            try
            {
                var result = await _userService.UpdateUser(id, collection);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public async Task<ActionResult> Toggle(string id)
        {
            var result = await _userService.GetUser(id);
            return View(result);
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ToggleAsync(string id, WebAccessUserModel collection)
        {
            try
            {
                await _userService.ToggleUser(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
