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
using FluentValidation;

namespace LibraryManagement.Controllers
{
    public class Wypozyczenia_KsiazkiController : Controller
    {
        private LibraryManagementDataEntities db = new LibraryManagementDataEntities();
        private int stan = -1;

        // GET: Wypozyczenia_Ksiazki
        public ActionResult Index()
        {
            var wypozyczenia_Ksiazki = db.Wypozyczenia_Ksiazki.Include(w => w.Czytelnik).Include(w => w.Ksiazka);
            return View(wypozyczenia_Ksiazki.ToList());
        }

        // GET: Wypozyczenia_Ksiazki/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wypozyczenia_Ksiazki wypozyczenia_Ksiazki = db.Wypozyczenia_Ksiazki.Find(id);
            if (wypozyczenia_Ksiazki == null)
            {
                return HttpNotFound();
            }
            return View(wypozyczenia_Ksiazki);
        }

        // GET: Wypozyczenia_Ksiazki/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wypozyczenia_Ksiazki wypozyczenia_Ksiazki = db.Wypozyczenia_Ksiazki.Find(id);
            if (wypozyczenia_Ksiazki == null)
            {
                return HttpNotFound();
            }
            ViewBag.BStan = wypozyczenia_Ksiazki.Stan;
            //stan = wypozyczenia_Ksiazki.Stan;
            ViewBag.Stan = new SelectList(db.Stan, "ID", "Opis", wypozyczenia_Ksiazki.Stan);
            return View(wypozyczenia_Ksiazki);
        }

        // POST: Wypozyczenia_Ksiazki/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ID_Czytelnika,ID_Ksiazki,Data_Wypozyczenia,Data_Zwrotu,Stan")] Wypozyczenia_Ksiazki wypozyczenia_Ksiazki, int bstan)
        {
            if (ModelState.IsValid)
            {
                DateValitador valitador = new DateValitador();
                ValidationResult res = valitador.Validate(wypozyczenia_Ksiazki);
                if (!res.IsValid)
                    {
                    ViewBag.Stan = new SelectList(db.Stan, "ID", "Opis", wypozyczenia_Ksiazki.Stan);
                    ViewBag.Error = "Error";
                    return View(wypozyczenia_Ksiazki);
                    }

                db.Entry(wypozyczenia_Ksiazki).State = EntityState.Modified;
                if (bstan != wypozyczenia_Ksiazki.Stan && wypozyczenia_Ksiazki.Stan == 3)
                {
                    db.Ksiazka.Find(wypozyczenia_Ksiazki.ID_Ksiazki).Stan_Magazynowy++;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Stan = new SelectList(db.Stan, "ID", "Opis", wypozyczenia_Ksiazki.Stan);
            return View(wypozyczenia_Ksiazki);
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
