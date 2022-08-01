using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TestingAplikasi.DAO;

namespace TestingAplikasi.Controllers
{
    public class AccountController : Controller
    {
        AccountDAO dao;
        public AccountController()
        {
            dao = new AccountDAO();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult ActLogin(string username, string password)
        {
            ClaimsIdentity identity = new ClaimsIdentity();
            bool isAuthenticated = false;
            var data = dao.getKaryawan(username);

            if(data != null)
            {
                // data ada masuk ke pengecekan password
                if(data.password == password)
                {
                    // data password sama
                    isAuthenticated = true;
                    identity = new ClaimsIdentity(new[] {
                                    new Claim(ClaimTypes.Name, data.nama),
                                    new Claim(ClaimTypes.Role, data.deskripsi),
                                    new Claim("username", data.npp),
                                    new Claim("role", data.deskripsi)
                                }, CookieAuthenticationDefaults.AuthenticationScheme);
                }
                else
                {
                    // password salah
                    TempData["error"] = "Password salah!";
                }
            }
            else
            {
                // data karyawan tidak ditemukan
                TempData["error"] = "Data tidak ditemukan!";
            }

            if (isAuthenticated)
            {
                var principal = new ClaimsPrincipal(identity);
                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public IActionResult LogOut()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
