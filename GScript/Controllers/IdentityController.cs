using GScript.Filters;
using GScriptNuget;
using GScript.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GScript.Repositories;

namespace GScript.Controllers
{
    public class IdentityController : Controller
    {
        private ServiceScripts ServiceApi;
        private IRepositoryScripts repo;

        public IdentityController(IRepositoryScripts repo
            , ServiceScripts serviceApi
            )
        {
            this.ServiceApi = serviceApi;
            this.repo = repo;
        }


        //[AuthorizeGuest]
        public IActionResult Signup()
        {
            return View();
        }

        //[AuthorizeGuest]
        [HttpPost]
        public async Task<IActionResult> Signup(String name, String email, String password)
        {
            //await this.ServiceApi.SignUpAsync(name, email, password);
            this.repo.Signup(name, email, password);
            return RedirectToAction("Login", "Identity");
        }


        //[AuthorizeGuest]
        public IActionResult Login()
        {
            return View();
        }

        //[AuthorizeGuest]
        [HttpPost]
        public async Task<IActionResult> Login(String email, String password)
        {
            bool exist = await this.ServiceApi.ExistsUserAsync(email);
            //bool exist = this.repo.ExistsUser(email);

            if (exist)
            {
                //User user = await this.ServiceApi.LoginAsync(email, password);
                User user = this.repo.LogIn(email, password);
                if (user != null) // USUARIO LOGUEADO
                {
                    ClaimsIdentity identity = new ClaimsIdentity
                        (CookieAuthenticationDefaults.AuthenticationScheme
                        , ClaimTypes.Name, ClaimTypes.Role);
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
                    identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));

                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync
                        (CookieAuthenticationDefaults.AuthenticationScheme, principal
                        , new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTime.Now.AddMinutes(15)
                        });

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["mensaje"] = "La contraseña introducida no es correcta.";
                    return View();
                }
            } 
            else
            {
                ViewData["mensaje"] = "El correo introducido no pertenece a ningún usuario.";
                return View();
            }
        }

        [AuthorizeUser]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
