using EcommerceApp.Application.Models.DTOs;
using EcommerceApp.Application.Services.LoginService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EcommerceApp.MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var logedUser = await _loginService.Login(loginDTO);


            if (true)
            {
                var jsonUser = JsonConvert.SerializeObject(loginDTO);
                HttpContext.Session.SetString("logedUser", jsonUser);
                var claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.Role, logedUser.Roles.ToString()));

                var userIdentity = new ClaimsIdentity(claims, "Login");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                if (logedUser.Roles==Domain.Enums.Roles.Admin)
                {
                    return RedirectToAction("Index", "Admin", new { area = "Admin" });
                }

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                Response.Cookies.Delete(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            return View();
        }

    }
}
