using GScript.Extensions;
using GScript.Filters;
using GScript.Helpers;
using GScriptNuget;
using GScript.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GScript.Controllers
{
    public class AppsController : Controller
    {
        private ServiceScripts ServiceApi;

        public AppsController(ServiceScripts serviceApi)
        {
            this.ServiceApi = serviceApi;
        }

        public async Task<IActionResult> Index()
        {
            return View(await this.ServiceApi.GetAppsAsync());
        }

        [HttpPost]
        public IActionResult SaveIdApps(List<int> ids)
        {
            HttpContext.Session.SetObject("idapps", ids);
            return RedirectToAction("Resume", "Apps");
        }

        public IActionResult DeleteIdApps()
        {
            //var aux2 = HttpContext.Session.GetObject<List<int>>("idapps");
            HttpContext.Session.Remove("idapps");
            //var aux = HttpContext.Session.GetObject<List<int>>("idapps");
            return RedirectToAction("Resume", "Apps");
        }

        public async Task<IActionResult> Resume()
        {
            //var aux = HttpContext.Session.GetObject<List<int>>("idapps");
            List<int> idapps = HttpContext.Session.GetObject<List<int>>("idapps");
            if(idapps == null) //.Count == 0
            {
                return View();
            }
            else {
                List<App> apps = await this.ServiceApi.GetAppsAsync(idapps); ;
                return View(apps);
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> Result(bool useConsole, String dir)
        {
            List<int> idapps = HttpContext.Session.GetObject<List<int>>("idapps");
            String result = "";
            if (useConsole)
            {
                result = await this.ServiceApi.GetAppsScriptAsync(true, idapps, dir);
            } else
            {
                result = await this.ServiceApi.GetAppsScriptAsync(false, idapps, dir);
            }
            ViewData["script"] = result;
            ViewData["idapps"] = idapps;
            return View();
        }

        [AuthorizeUser]
        [HttpPost]
        public async Task<IActionResult> PostScript(String name, String script )
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await this.ServiceApi.PostScriptAsync(name, script, userId);
            return  RedirectToAction("Resume", "Apps");
        }

        public FileResult Download(String script)
        {
            var stream = new MemoryStream(Encoding.ASCII.GetBytes(script));
            return File(stream, "text/plain", "script.bat");
        }
    }
}
