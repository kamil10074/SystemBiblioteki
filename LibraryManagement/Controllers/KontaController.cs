using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LibraryManagement.Models;
using FluentValidation.Results;

namespace LibraryManagement.Controllers
{

    public class KontaController : Controller
    {
        private LibraryManagementDataEntities db = new LibraryManagementDataEntities();
        // GET: Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        //Get
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register([Bind(Include = "ID,Imie,Nazwisko,Uzytkownik,Haslo,Email")]Czytelnik czytelnik)
        {
            if (ModelState.IsValid)
            {
                CzytelnikValidator validator = new CzytelnikValidator();
                ValidationResult result = validator.Validate(czytelnik);

                if (!result.IsValid)
                {
                    List<string> errors = new List<string>();
                    foreach (ValidationFailure vf in result.Errors)
                    {
                        errors.Add(vf.ErrorMessage);
                    }
                    ViewBag.Error = errors;
                    return View(czytelnik);
                }

                db.Czytelnik.Add(czytelnik);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(czytelnik);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Czytelnik czytelnik, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                List<string> errors = new List<string>();
                var currentuser = db.Czytelnik.Where(user => user.Uzytkownik.Equals(czytelnik.Uzytkownik) && user.Haslo.Equals(czytelnik.Haslo)).FirstOrDefault();
                if (currentuser != null)
                {
                    LoginValidator validator = new LoginValidator();
                    ValidationResult result = validator.Validate(currentuser);

                    if (!result.IsValid)
                    {
                        foreach (ValidationFailure vf in result.Errors)
                        {
                            errors.Add(vf.ErrorMessage);
                        }
                        ViewBag.Error = errors;
                        return View(czytelnik);
                    }

                    FormsAuthentication.SetAuthCookie(currentuser.ID.ToString(), false);
                    Session["UserID"] = currentuser.ID.ToString();
                    Session["UserRole"] = currentuser.Rola.ToString();
                    if (!string.IsNullOrEmpty(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Ksiazka");
                    }
                }
            }

            return View(czytelnik);
        }

        [HttpGet]
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}