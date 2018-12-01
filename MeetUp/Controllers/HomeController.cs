using MeetUp.DAL;
using MeetUp.Models;
using MeetUp.ViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MeetUp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            /*DAL.MeetUpContext db = new DAL.MeetUpContext();
            User u1 = new User() { userId = 1, name = "Jan", surname = "Kowalski", age = 30, password = "123456789", confirmedpassword = "123456789", login = "admin" };
            User u2 = new User() { userId = 2, name = "Arek", surname = "Grygoruk", age = 21, password = "987654321", confirmedpassword = "987654321", login = "crazyoll" };
            Event e1 = new Event() { Date = DateTime.Now, Id = 1, Name = "tak admina", number = 5000, palce = "Warszawa", User = u1 };
            Event e2 = new Event() { Date = DateTime.Now, Id = 2, Name = "tak admina v2", number = 300, palce = "Bialystok", User = u1 };
            Event e3 = new Event() { Date = DateTime.Now, Id = 3, Name = "usera", number = 3000, palce = "PB", User = u2 };
            db.Users.Add(u1);
            db.Users.Add(u2);
            db.Events.Add(e1);
            db.Events.Add(e2);
            db.Events.Add(e3);
            db.SaveChanges();*/
            return View();
        }
        
        public ActionResult Profil(int id = -1)
        {
            try
            {
                id = Int32.Parse(Session["userId"].ToString());
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            //id = Int32.Parse(Session["userId"].ToString());
            dynamic mymodel = new ExpandoObject();
            Profil ktos = new Profil();
            User c = ktos.ZnajdzUsera(id);
            mymodel.User = c;
            mymodel.Events = ktos.GetUserEvent(c);
            return View(mymodel);
        }

        [HttpGet]
        public ActionResult Rejestracja(int id = 0)
        {
            User ktos = new User();
            return View(ktos);
        }

        [HttpPost]
        public ActionResult Rejestracja(User ktos)
        {
            User ktos1 = new User();
            ktos1 = ktos;
            UserAdd user = new UserAdd();
            List<User> lista = user.GetUser();
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                if (lista.Any(u => u.login == ktos.login))
                {
                    ModelState.AddModelError("login", "Ten login jest już zajęty!");
                    return View("Rejestracja", ktos);
                }
                else if (ktos.password.Count() < 8)
                {
                    ModelState.AddModelError("password", "Hasło musi mieć co najmniej 8 znaków!");
                    return View("Rejestracja", ktos);
                }
                else
                {
                    user.AddUser(ktos1);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Logowanie(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Logowanie(User user, string ReturnUrl)
        {
            MeetUpContext baza = new MeetUpContext();
            var daneUsera = baza.Users.Where(k => k.login == user.login && k.password == user.password).FirstOrDefault();
            UserAdd ktos = new UserAdd();
            List<User> lista = ktos.GetUser();
            //if (daneUsera == null)
            //{
            //    ModelState.AddModelError("login", "Błędny login lub hasło");
            //    return View("Logowanie", user);
            //}
            if (!lista.Any(k => k.login == user.login && k.password == user.password))
            {
                ModelState.AddModelError("password", "Błędny login lub hasło");
                return View("Logowanie", user);
            }
            else
            {
                Session["userId"] = daneUsera.userId;
                Session["login"] = daneUsera.login;
                FormsAuthentication.SetAuthCookie(user.login, false);

                if (Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

        }

        public ActionResult Wyloguj()
        {
            int Id = (int)Session["userId"];
            Session.Abandon();
            return RedirectToAction("Logowanie", "Home");
        }
    }
}