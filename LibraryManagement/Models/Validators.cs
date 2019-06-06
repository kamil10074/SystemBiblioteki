using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;


namespace LibraryManagement.Models
{
    public class DateValitador : AbstractValidator<Wypozyczenia_Ksiazki>
    {
        public DateValitador()
        {
            RuleFor(x => x.Data_Zwrotu).GreaterThanOrEqualTo(x => x.Data_Wypozyczenia).WithMessage("Data Zwrotu jest przed Datą Wypożyczenia");
        }
    }







    public class RzeczValidator : AbstractValidator<Rzecz>
    {
        public RzeczValidator()
        {
            RuleFor(x => x.ilosc).GreaterThan(0).WithMessage("Ilość musi być większa niż 0");
            RuleFor(x => x.data_zwrotu).GreaterThanOrEqualTo(x => x.data_wypozyczenia).WithMessage("Data Zwrotu jest przed Datą Wypożyczenia");
        }
    }

    public class CzytelnikValidator : AbstractValidator<Czytelnik>
    {
        public CzytelnikValidator()
        {
            RuleFor(x => x.Imie).NotEmpty().WithMessage("Imię jest wymagane");
            RuleFor(x => x.Nazwisko).NotEmpty().WithMessage("Nazwisko jest wymagane");
            RuleFor(x => x.Uzytkownik).NotEmpty().WithMessage("Nazwa użytkownika jest wymagana");
            RuleFor(x => x.Haslo).NotEmpty().WithMessage("Hasło jest wymagane");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Niepoprawny adres email");


        }
    }

    public class LoginValidator : AbstractValidator<Czytelnik>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Wazne).Must(x => x == true).WithMessage("Twoje konto nie jest jeszcze aktywne");
        }
    }
}