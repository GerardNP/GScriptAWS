using GScriptNuget;
using GScript.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GScript.Controllers
{
    public class UsersController : Controller
    {
        private ServiceScripts ServiceApi;

        public UsersController(ServiceScripts serviceApi)
        {
            this.ServiceApi = serviceApi;
        }

        public async Task<IActionResult> Profile()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            User user = await this.ServiceApi.GetUserAsync(userId);
            ViewData["email"] = user.Email;

            return View(await this.ServiceApi.GetScriptsAsync(userId));
        }

        public async Task<IActionResult> DeleteScript(int id)
        {
            await this.ServiceApi.DeleteScriptAsync(id);
            return RedirectToAction("Profile", "Users");
        }
    }
}
