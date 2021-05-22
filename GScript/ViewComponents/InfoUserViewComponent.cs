using GScriptNuget;
using GScript.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GScript.ViewComponents
{
    public class InfoUserViewComponent : ViewComponent
    {
        private ServiceScripts ServiceApi;

        public InfoUserViewComponent(ServiceScripts serviceApi)
        {
            this.ServiceApi = serviceApi;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
