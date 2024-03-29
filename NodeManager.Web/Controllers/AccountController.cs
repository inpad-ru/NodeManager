﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NodeManager.Web.Models; // пространство имен моделей RegisterModel и LoginModel
//using NodeManager.Domain; // пространство имен UserContext и класса User
using NodeManager.Web.DBInfrastucture;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Web;

namespace NodeManager.Web.Controllers
{
    public class AccountController : Controller
    {
        private  NodeManagerDBEntities db;
        public AccountController(NodeManagerDBEntities context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Users user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user); // аутентификация

                    return RedirectToAction("List", "Node");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Users user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    var u = new Users { Email = model.Email, Name = model.Name, Password = model.Password, Company = model.Company };
                    db.Users.Add(u);
                    await db.SaveChangesAsync();

                    await Authenticate(u); // аутентификация

                    //return RedirectToAction("Index", "Home");
                    return RedirectToAction("List", "Node");
                }
                else { 
                ModelState.AddModelError("", "Пользователь с таким Email уже зарегистрирован");
                //string script = "alert(\"Hello!\");";
                //ScriptManager.RegisterStartupScript(this, GetType(),
                //                      "ServerControlScript", script, true);
            }
            }
            return View(model);
        }

        private async Task Authenticate(Users user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Company", user.Company)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("List", "Node");
        }
    }
}