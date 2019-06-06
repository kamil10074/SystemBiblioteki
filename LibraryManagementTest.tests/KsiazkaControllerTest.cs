using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryManagement.Controllers;
using System.Web.Mvc;
using LibraryManagement.Models;

namespace LibraryManagementTest.tests
{
    [TestClass]
    public class KsiazkaControllerTest
    {
        private LibraryManagementDataEntities db = new LibraryManagementDataEntities();
        [TestMethod]
        public void DetailsTest()
        {
            var obj = new KsiazkaController();
            var result = obj.Details(1) as ViewResult;
            var blad = obj.Details(2) as ViewResult;
            Assert.AreEqual(null, blad);
        }

        [TestMethod]
        public void IndexTest()
        {
            var obiekt = new KsiazkaController();
            var resultat = obiekt.Delete(1) as ViewResult;
            var blad1 = obiekt.Delete(2) as ViewResult;
            Assert.AreNotEqual(resultat, blad1);
        }
    }
}
