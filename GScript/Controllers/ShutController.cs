using GScript.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GScript.Controllers
{
    public class ShutController : Controller
    {
        private ServiceScripts ServiceApi;

        public ShutController(ServiceScripts serviceApi)
        {
            this.ServiceApi = serviceApi;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResultOptions(String operation, String msg, int time)
        {
            String script = await this.ServiceApi.GetShutScriptAsync(operation, msg, time);
            ViewData["script"] = script;
            return View();
        }

        public FileResult Download(String script)
        {
            var stream = new MemoryStream(Encoding.ASCII.GetBytes(script));
            return File(stream, "text/plain", "script.bat");
        }
    }
}
