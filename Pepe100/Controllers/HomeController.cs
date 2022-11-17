using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pepe100.Functions;
using System.Text;
using System.Net.Mail;
using System.Collections;
using Pepe100.Models;
using Pepe100.ViewModels;

namespace Pepe100.Controllers
{
    public class HomeController : Controller
    {
        private pepeEntities db = new pepeEntities();

        ////// EMAIL POISTETTU GITHUB JULKAISUSTA

        public ActionResult Index()
        {
            ViewBag.LoginError = 0;
            ViewBag.LoginErrorYritys = 0;
            ViewBag.LoginID2 = ViewBag.LoginID;
            ViewBag.EmailError = 0;
            if (Session["LoginID"] != null)
            {
                ViewBag.LoginError = 1;
                var logi = Session["LoginID"].ToString();
                int? logg = int.Parse(logi);
                var tyontekija = (from t in db.tyontekijat2
                                  where logg == t.LoginID
                                  select new TyontekijatViewModel()
                                  {
                                      LoginID = logg,
                                      HenkiloID = t.HenkiloID,
                                      Etunimi = t.Etunimi,
                                      Sukunimi = t.Sukunimi,
                                      Puhelin = t.Puhelin,
                                      Email = t.Email,
                                      Lahihoitaja = t.Lahihoitaja,
                                      Sairaanhoitaja = t.Sairaanhoitaja,
                                      Vanhustenhoito_kotihoito = t.Vanhustenhoito_kotihoito,
                                      Mielenterveys = t.Mielenterveys,
                                      Paivystys_Ensihoito = t.Paivystys_Ensihoito,
                                      Lapset = t.Lapset,
                                      Kehitysvammaiset = t.Kehitysvammaiset,
                                      Hengityshalvaus = t.Hengityshalvaus,
                                      Ensihoitaja_AMK = t.Ensihoitaja_AMK,
                                      Terveydenhoitaja = t.Terveydenhoitaja,
                                      Katilo = t.Katilo,
                                      Osastotyoskentely = t.Osastotyoskentely,
                                      Paivystys = t.Paivystys,
                                      Ensihoito = t.Ensihoito,
                                  }).ToList();
                return RedirectToAction("Index", "Tyontekijat2", tyontekija);
            }
            return View();
        }

        public ActionResult Index2()
        {
            ViewBag.LoginError = 0;
            ViewBag.EmailError = 1;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Authorize(login LoginModel)
        {

            var LoggedUser = db.login.SingleOrDefault(x => x.UserName == LoginModel.UserName && x.Password == LoginModel.Password); // Käyttäjätunnusten tarkistus
            if (LoggedUser != null)
            {
                var crpwd = "";  //salasanan suojaus
                var salt = Hmac.GenerateSalt();
                var hmac1 = Hmac.ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(LoginModel.Password), salt);
                crpwd = (Convert.ToBase64String(hmac1));

                ViewBag.LoginMessage = "Successfull login";
                ViewBag.LoggedStatus = "In";
                ViewBag.LoginError = 0;
                Session["UserName"] = LoggedUser.UserName;
                Session["LoginID"] = LoggedUser.LoginID;
                Session["HenkiloID"] = LoggedUser.HenkiloID;
                ViewBag.LoginID = LoggedUser.LoginID;
                TempData["LoginID"] = LoggedUser.LoginID;

                return RedirectToAction("Index", "Home", ViewBag.LoginID);
            }
            else
            {
                ViewBag.LoginMessage = "Login unsuccessfull";
                ViewBag.LoggedStatus = "Out";
                ViewBag.LoginError = 1;
                LoginModel.LoginErrorMessage = "Tuntematon käyttäjätunnus tai salasana.";
                return View("Index", LoginModel);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuthorizeYritys(loginyritys LoginModel)
        {

            var LoggedUser = db.loginyritys.SingleOrDefault(x => x.UserNameYritys == LoginModel.UserNameYritys && x.PasswordYritys == LoginModel.PasswordYritys); // Käyttäjätunnusten tarkistus
            var YritysID = (from l in db.loginyritys
                            where l.UserNameYritys == LoginModel.UserNameYritys && l.PasswordYritys == LoginModel.PasswordYritys
                            select l.YritysID).FirstOrDefault();  // Käyttäjätunnusten tarkistus
            if (LoggedUser != null)
            {
                var crpwd = "";  //salasanan suojaus
                var salt = Hmac.GenerateSalt();
                var hmac1 = Hmac.ComputeHMAC_SHA256(Encoding.UTF8.GetBytes(LoginModel.PasswordYritys), salt);
                crpwd = (Convert.ToBase64String(hmac1));

                ViewBag.LoginMessage = "Successfull login";
                ViewBag.LoggedStatus = "In";
                ViewBag.LoginError = 0;
                Session["UserName"] = LoggedUser.UserNameYritys;
                Session["YritysID"] = YritysID;
                ViewBag.LoginID = LoggedUser.LoginIDYritys;
                TempData["LoginID"] = LoggedUser.LoginIDYritys;

                return RedirectToAction("Index", "Home", ViewBag.LoginID);
            }
            else
            {
                ViewBag.LoginMessage = "Login unsuccessfull";
                ViewBag.LoggedStatus = "Out";
                ViewBag.LoginError = 1;
                LoginModel.LoginErrorMessageYritys = "Tuntematon käyttäjätunnus tai salasana.";
                return View("Index", LoginModel);
            }
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            ViewBag.LoggedStatus = "Logged Out";
            return RedirectToAction("Index", "Home");
        }

    }
}