using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryManagement.Models;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Results;

namespace LibraryManagement.Controllers
{
    public class KoszykController : Controller
    {
        private List<Rzecz> list;
        private LibraryManagementDataEntities db = new LibraryManagementDataEntities();
        // GET: Koszyk
        public ActionResult Index()
        {
            if (Session["Koszyk"] == null)
            {
                list = new List<Rzecz>();
            }
            else
            {
                list = (List<Rzecz>)Session["Koszyk"];
            }
            return View(list.ToList());
        }

        public ActionResult Add(int id, int type)
        {
            if (Session["Koszyk"] == null)
            {
                list = new List<Rzecz>();
            }
            else
            {
                list = (List<Rzecz>)Session["Koszyk"];
            }

            if (type == 0)
            {
                Ksiazka k = db.Ksiazka.Where(x => x.ID == id).FirstOrDefault();
                if (!contains(k.Tytul))
                {
                    list.Add(new Rzecz() { tytul = k.Tytul, ilosc = 1, ID = k.ID, type = type });
                }
            }
            Session["Koszyk"] = list;

            return RedirectToAction("Index");
        }

        private bool contains(string tytul)
        {
            foreach (Rzecz r in list)
            {
                if (r.tytul.Equals(tytul))
                {
                    return true;
                }
            }
            return false;
        }


        public ActionResult Delete(int id)
        {
            if (Session["Koszyk"] != null)
            {
                list = (List<Rzecz>)Session["Koszyk"];
                list.RemoveAt(id);
                //list.Remove(rzecz);
                Session["Koszyk"] = list;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(List<Rzecz> list, string submit, string save)
        {
            if (!string.IsNullOrEmpty(save))
            {
                Session["Koszyk"] = list;
                return View(list.ToList());
            }

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Index"); // Tutaj dać jakiś błąd
            }

            System.Web.HttpContext.Current.Application.Lock();
            int limit = (int)System.Web.HttpContext.Current.Application["Limit"];
            System.Web.HttpContext.Current.Application.UnLock();
            int sum = 0;
            foreach (Rzecz r in list)
            {
                sum += r.ilosc;
            }

            if (sum > limit)
            {
                ViewBag.Error = "Przekroczono limit wypożyczeń";
                return View(list.ToList()); // Tutaj dać jakiś błąd
            }

            foreach (Rzecz r in list)
            {
                RzeczValidator validator = new RzeczValidator();
                FluentValidation.Results.ValidationResult result = validator.Validate(r);
                if (!result.IsValid)
                {
                    List<string> errorStr = new List<string>();
                    foreach (ValidationFailure vf in result.Errors)
                    {
                        errorStr.Add(vf.ErrorMessage);
                    }
                    ViewBag.Error = errorStr;
                    return View(list.ToList());
                }

                r.ID_Czytelnika = Int32.Parse(Session["UserID"].ToString());
                if (r.type == 0)
                {
                    Ksiazka k = db.Ksiazka.Find(r.ID);
                    if (r.ilosc > k.Stan_Magazynowy)
                    {
                        ViewBag.Error = "Nie można przetworzyć zamówienia";
                        return View(list.ToList());
                    }
                    for (int i = 0; i < r.ilosc; i++)
                    {
                        db.Wypozyczenia_Ksiazki.Add(new Wypozyczenia_Ksiazki { ID_Czytelnika = r.ID_Czytelnika, ID_Ksiazki = r.ID, Data_Wypozyczenia = r.data_wypozyczenia, Data_Zwrotu = r.data_zwrotu });
                        k.Stan_Magazynowy -= 1;
                    }
                }
                
            }


            db.SaveChanges();
            list.Clear();
            Session["Koszyk"] = null;
            return RedirectToAction("Index", "Ksiazka", null);
        }
    }
}
