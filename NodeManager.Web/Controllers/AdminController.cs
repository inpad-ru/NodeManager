//using NodeManager.Domain;
using NodeManager.Web.DBInfrastucture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using NodeManager.Web.Abstract;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using NodeManager.Web.Models;
using System.Drawing;
using System.IO;
using NodeManager.Web.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace NodeManager.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private INodes repos;

        public IActionResult GetAllUsers()
        {
            var model = new AllUsersModel()
            {
                Users = repos.Users.ToList(),
                UserName = HttpContext.User.Identity.Name,
                IsLogin = HttpContext.User.Identity.IsAuthenticated,
            };
            return View(model);
        }

    }
}
