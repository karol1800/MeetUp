using MeetUp.DAL;
using MeetUp.Models;
using MeetUp.ViewModels;
using System;
using System.Collections.Generic;
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
            //DAL.MeetUpContext db = new DAL.MeetUpContext();
            return View();
        }
        
        public ActionResult Profil(int id)
        {
            Profil ktos = new Profil();
            User c = ktos.ZnajdzUsera(id);
            return View(c);
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