using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryManagement.Controllers
{
    public class HomeController : Controller
    {
        private LibraryManagementDataEntities db = new LibraryManagementDataEntities();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}