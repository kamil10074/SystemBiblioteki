using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryManagement.Models
{
    public class Rzecz
    {
        public string tytul { get; set; }
        public int ilosc { get; set; }
        public int ID { get; set; }
        public int type { get; set; }
        public int ID_Czytelnika { get; set; }
        public DateTime data_wypozyczenia { get; set; }
        public DateTime data_zwrotu { get; set; }

        public Rzecz() { }

    }
}