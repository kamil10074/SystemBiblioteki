using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryManagement.Models;
using FluentValidation.Results;

namespace LibraryManagement.Controllers
{
    [Authorize]
    public class CzytelnikController : Controller
    {
        private LibraryManagementDataEntities db = new LibraryManagementDataEntities();
        // GET: Czytelnik
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            int id = 0;
            if (Session["UserID"] != null)
            {
                id = Int32.Parse(Session["UserID"].ToString());
            }
            //ViewBag.UserName = db.Czytelnik.Find(id).Uzytkownik.ToString();
            return View(db.Czytelnik.ToList());
        }

        // GET: Czytelnik/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Czytelnik czytelnik = db.Czytelnik.Find(id);
            if (czytelnik == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserRoleString = RolaToString(czytelnik.Rola);
            return View(czytelnik);
        }

        // GET: Czytelnik/Create
        public ActionResult Create()
        {
            return View();
        }

        private string RolaToString(int? rola)
        {
            switch (rola)
            {
                case 0:
                    {
                        return "Czytelnik";
                    }
                case 1:
                    {
                        return "Pracownik";
                    }
                case 2:
                    {
                        return "Administator";
                    }
                default:
                    {
                        return "Nieznana Rola: " + rola;
                    }
            }
        }

        // POST: Czytelnik/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Imie,Nazwisko,Uzytkownik,Haslo,Email")] Czytelnik czytelnik)
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
                return RedirectToAction("Index");
            }

            return View(czytelnik);
        }

        // GET: Czytelnik/Edit/5
        public ActionResult Edit(int? id)
        {
            Czytelnik user = null;
            ViewBag.UserRole = null;
            if (Session["UserID"] != null)
            {
                user = db.Czytelnik.Find(Int32.Parse(Session["UserID"].ToString()));
            }
            if (user != null)
            {
                ViewBag.UserRole = user.Rola;
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Czytelnik czytelnik = db.Czytelnik.Find(id);
            if (czytelnik == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserRoleString = RolaToString(czytelnik.Rola);
            ViewBag.RoleSelectList = new SelectList(db.Rola, "ID", "Nazwa", czytelnik.Rola);
            return View(czytelnik);
        }

        // POST: Czytelnik/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Imie,Nazwisko,Uzytkownik,Haslo,Email,Wazne,Rola")] Czytelnik czytelnik)
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
                    ViewBag.UserRoleString = RolaToString(czytelnik.Rola);
                    ViewBag.RoleSelectList = new SelectList(db.Rola, "ID", "Nazwa", czytelnik.Rola);
                    return View(czytelnik);
                }

                db.Entry(czytelnik).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(czytelnik);
        }

        // GET: Czytelnik/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Czytelnik czytelnik = db.Czytelnik.Find(id);
            if (czytelnik == null)
            {
                return HttpNotFound();
            }
            return View(czytelnik);
        }

        // POST: Czytelnik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Czytelnik czytelnik = db.Czytelnik.Find(id);
            db.Czytelnik.Remove(czytelnik);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
