using GScript.Filters;
using GScript.Helpers;
using GScript.Services;
using GScriptNuget;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GScript.Controllers
{
    [AuthorizeAdmin]
    public class AdminController : Controller
    {
        private ServiceScripts ServiceApi;
        private PathProvider pathprovider;

        public AdminController(ServiceScripts serviceApi,
            PathProvider pathprovider)
        {
            this.ServiceApi = serviceApi;
            this.pathprovider = pathprovider;
        }

        public async Task<IActionResult> Index()
        {
            return View(await this.ServiceApi.GetAppsAsync());
        }

        // CREATE
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(App app, IFormFile file)
        {
            // GUARDAMOS LA IMAGEN
            String filename = file.FileName;
            String path = this.pathprovider.MapPath(filename, PathProvider.Folders.Apps);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            await this.ServiceApi.PostAppAsync(app.Name, app.ExeName,
                app.Path, filename);
            return RedirectToAction("Create", "Admin");
        }

        // UPDATE
        public async Task<IActionResult> Update(int id)
        {
            return View(await this.ServiceApi.GetAppAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, String name, 
            String exename, String path, IFormFile file)
        {
            // GUARDAMOS LA IMAGEN
            if (file == null)
            {
                await this.ServiceApi.PutAppAsync(id, name, exename, path, null);
            }
            else
            {
                App app = await this.ServiceApi.GetAppAsync(id);
                String oldPath = app.Icon;
                String fullOldPath = this.pathprovider.MapPath(oldPath, PathProvider.Folders.Apps);
                System.IO.File.Delete(fullOldPath);

                String filename = file.FileName;
                String newPath = this.pathprovider.MapPath(filename, PathProvider.Folders.Apps);
                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                await this.ServiceApi.PutAppAsync(id, name, exename, path, filename);
            }

            return RedirectToAction("Update", id);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await this.ServiceApi.DeleteAppAsync(id);
            return RedirectToAction("Index", "Admin");
        }
    }
}
