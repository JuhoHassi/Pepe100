using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Pepe100.Models;
using Pepe100.ViewModels;

namespace Pepe100.Controllers
{
    public class TehtaviaController : Controller
    {
        private pepeEntities db = new pepeEntities();

        [HttpGet]
        public ActionResult Index(TyontekijatViewModel tyontekijat)
        {
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
                return View("Index2", tyontekijat);


            }
            return View("Index2", tyontekijat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index2([Bind(Include = "HenkiloID, YritysID, Etunimi, Sukunimi, Puhelin, Email, Luotu, ViimeksiMuokattu, Poistettu, Aktiivinen, EmailMessage, LoginID, Lahihoitaja1, Sairaanhoitaja1, LOP, I_V, LAS, GER, PSYK, ROKOTUS, Tyokokemus, Vanhustenhoito_kotihoito, Mielenterveys, Paivystys_Ensihoito, Lapset, Kehitysvammaiset, Hengityshalvaus, Ensihoitaja_AMK, Terveydenhoitaja, Katilo, Osastotyoskentely, Paivystys, Teho_osasto, Ensihoito, Kommentit")] TyontekijatViewModel tyontekijat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tyontekijat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HenkiloID = new SelectList(db.tyontekijat2, "HenkiloID", "Etunimi", tyontekijat.HenkiloID);
            return View(tyontekijat);
        }

        public ActionResult _Lahihoitajat()
        {
            var hoitajat2 = db.tyontekijat2;
            return View(hoitajat2);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Lahihoitajat([Bind(Include = "HenkiloID, LoginID, Etunimi, Sukunimi, Puhelin, Email, Luotu, ViimeksiMuokattu, Poistettu, Aktiivinen, EmailMessage,  Lahihoitaja, Sairaanhoitaja, LOP, I_V, KIPU,LAS, GER, PSYK, ROKOTUS, Tyokokemus, Vanhustenhoito_kotihoito, Mielenterveys, Paivystys_Ensihoito, Lapset, Kehitysvammaiset, Hengityshalvaus, Ensihoitaja_AMK, Terveydenhoitaja, Katilo, Osastotyoskentely, Paivystys, Teho_osasto, Ensihoito, Kommentit")] TyontekijatViewModel tyontekijat)
        {
            var logi = Session["LoginID"].ToString();
            int logg = int.Parse(logi);
            if (ModelState.IsValid)
            {
                int? idhaku = (from l in db.login
                               where logg == l.LoginID
                               select l.HenkiloID).FirstOrDefault();
                string etu = (from t in db.tyontekijat2
                              where idhaku == t.HenkiloID
                              select t.Etunimi).FirstOrDefault();
                string suku = (from t in db.tyontekijat2
                               where idhaku == t.HenkiloID
                               select t.Sukunimi).FirstOrDefault();
                tyontekijat2 tyontekijat1 = new tyontekijat2()
                {
                    HenkiloID = (int)idhaku,
                    LoginID = int.Parse(logi),
                    Etunimi = etu,
                    Sukunimi = suku,
                    Puhelin = tyontekijat.Puhelin,
                    Email = tyontekijat.Email,
                    Lahihoitaja = tyontekijat.Lahihoitaja,
                    Sairaanhoitaja = tyontekijat.Sairaanhoitaja,
                    LOP = tyontekijat.LOP,
                    I_V = tyontekijat.I_V,
                    KIPU = tyontekijat.KIPU,
                    LAS = tyontekijat.LAS,
                    GER = tyontekijat.GER,
                    PSYK = tyontekijat.PSYK,
                    ROKOTUS = tyontekijat.ROKOTUS,
                    Tyokokemus = tyontekijat.Tyokokemus,
                    Vanhustenhoito_kotihoito = tyontekijat.Vanhustenhoito_kotihoito,
                    Mielenterveys = tyontekijat.Mielenterveys,
                    Paivystys_Ensihoito = tyontekijat.Paivystys_Ensihoito,
                    Lapset = tyontekijat.Lapset,
                    Kehitysvammaiset = tyontekijat.Kehitysvammaiset,
                    Hengityshalvaus = tyontekijat.Hengityshalvaus,
                    Ensihoitaja_AMK = tyontekijat.Ensihoitaja_AMK,
                    Terveydenhoitaja = tyontekijat.Terveydenhoitaja,
                    Katilo = tyontekijat.Katilo,
                    Osastotyoskentely = tyontekijat.Osastotyoskentely,
                    Paivystys = tyontekijat.Paivystys,
                    Teho_osasto = tyontekijat.Teho_osasto,
                    Ensihoito = tyontekijat.Ensihoito,
                    Kommentit = tyontekijat.Kommentit,
                };

                db.tyontekijat2.Add(tyontekijat1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HenkiloID = new SelectList(db.tyontekijat2, "HenkiloID", "Etunimi", tyontekijat.HenkiloID);
            return View(tyontekijat);
        }
        public ActionResult _Sairaanhoitajat()
        {
            var hoitajat2 = db.tyontekijat2;
            return View(hoitajat2);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Sairaanhoitajat([Bind(Include = "HenkiloID, Etunimi, Sukunimi, Puhelin, Email, Luotu, ViimeksiMuokattu, Poistettu, Aktiivinen, EmailMessage, LoginID, Lahihoitaja1, Sairaanhoitaja1, LOP, I_V,KIPU, LAS, GER, PSYK, ROKOTUS, Tyokokemus, Vanhustenhoito_kotihoito, Mielenterveys, Paivystys_Ensihoito, Lapset, Kehitysvammaiset, Hengityshalvaus, Ensihoitaja_AMK, Terveydenhoitaja, Katilo, Osastotyoskentely, Paivystys, Teho_osasto, Ensihoito, Kommentit")] TyontekijatViewModel tyontekijat)
        {
            var logi = Session["LoginID"].ToString();
            int logg = int.Parse(logi);
            if (ModelState.IsValid)
            {
                int? idhaku = (from l in db.login
                               where logg == l.LoginID
                               select l.HenkiloID).FirstOrDefault();
                string etu = (from t in db.tyontekijat2
                              where idhaku == t.HenkiloID
                              select t.Etunimi).FirstOrDefault();
                string suku = (from t in db.tyontekijat2
                               where idhaku == t.HenkiloID
                               select t.Sukunimi).FirstOrDefault();
                tyontekijat2 tyontekijat1 = new tyontekijat2()
                {
                    HenkiloID = (int)idhaku,
                    LoginID = int.Parse(logi),
                    Etunimi = etu,
                    Sukunimi = suku,
                    Puhelin = tyontekijat.Puhelin,
                    Email = tyontekijat.Email,
                    Lahihoitaja = tyontekijat.Lahihoitaja,
                    Sairaanhoitaja = tyontekijat.Sairaanhoitaja,
                    LOP = tyontekijat.LOP,
                    I_V = tyontekijat.I_V,
                    KIPU = tyontekijat.KIPU,
                    LAS = tyontekijat.LAS,
                    GER = tyontekijat.GER,
                    PSYK = tyontekijat.PSYK,
                    ROKOTUS = tyontekijat.ROKOTUS,
                    Tyokokemus = tyontekijat.Tyokokemus,
                    Vanhustenhoito_kotihoito = tyontekijat.Vanhustenhoito_kotihoito,
                    Mielenterveys = tyontekijat.Mielenterveys,
                    Paivystys_Ensihoito = tyontekijat.Paivystys_Ensihoito,
                    Lapset = tyontekijat.Lapset,
                    Kehitysvammaiset = tyontekijat.Kehitysvammaiset,
                    Hengityshalvaus = tyontekijat.Hengityshalvaus,
                    Ensihoitaja_AMK = tyontekijat.Ensihoitaja_AMK,
                    Terveydenhoitaja = tyontekijat.Terveydenhoitaja,
                    Katilo = tyontekijat.Katilo,
                    Osastotyoskentely = tyontekijat.Osastotyoskentely,
                    Paivystys = tyontekijat.Paivystys,
                    Teho_osasto = tyontekijat.Teho_osasto,
                    Ensihoito = tyontekijat.Ensihoito,
                    Kommentit = tyontekijat.Kommentit,
                };
                db.tyontekijat2.Add(tyontekijat1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HenkiloID = new SelectList(db.tyontekijat2, "HenkiloID", "Etunimi", tyontekijat.HenkiloID);
            return View(tyontekijat);
        }

        public ActionResult Create()
        {
            ViewBag.LoginID = new SelectList(db.login, "LoginID", "UserName");
            ViewBag.HenkiloID = new SelectList(db.tyontekijat2, "HenkiloID", "Etunimi");
            ViewBag.YritysID = new SelectList(db.yritys, "YritysID", "YritysNimi");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TyoID,HenkiloID,YritysID,LoginID,Tehtava,TehtavaTaytettu,TehtavaValmis,Sijainti,Etunimi,Sukunimi,Puhelin,Email,EmailMessage,Aloitusaika,Lopetusaika,AikaYhteensa,Luotu,ViimeksiMuokattu,Poistettu,Aktiivinen,Lahihoitaja,Sairaanhoitaja,LOP,KIPU,I_V,LAS,GER,PSYK,ROKOTUS,Tyokokemus,Vanhustenhoito_kotihoito,Mielenterveys,Paivystys_Ensihoito,Lapset,Kehitysvammaiset,Hengityshalvaus,Ensihoitaja_AMK,Terveydenhoitaja,Katilo,Osastotyoskentely,Paivystys,Teho_osasto,Ensihoito,Kommentit,TarjousHinta")] tehtavia tehtavia)
        {
            if (ModelState.IsValid)
            {
                db.tehtavia.Add(tehtavia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LoginID = new SelectList(db.login, "LoginID", "UserName", tehtavia.LoginID);
            ViewBag.HenkiloID = new SelectList(db.tyontekijat2, "HenkiloID", "Etunimi", tehtavia.HenkiloID);
            ViewBag.YritysID = new SelectList(db.yritys, "YritysID", "YritysNimi", tehtavia.YritysID);
            return View(tehtavia);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tehtavia tehtavia = db.tehtavia.Find(id);
            if (tehtavia == null)
            {
                return HttpNotFound();
            }
            ViewBag.LoginID = new SelectList(db.login, "LoginID", "UserName", tehtavia.LoginID);
            ViewBag.HenkiloID = new SelectList(db.tyontekijat2, "HenkiloID", "Etunimi", tehtavia.HenkiloID);
            ViewBag.YritysID = new SelectList(db.yritys, "YritysID", "YritysNimi", tehtavia.YritysID);
            return View(tehtavia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TyoID,HenkiloID,YritysID,LoginID,Tehtava,TehtavaTaytettu,TehtavaValmis,Sijainti,Etunimi,Sukunimi,Puhelin,Email,EmailMessage,Aloitusaika,Lopetusaika,AikaYhteensa,Luotu,ViimeksiMuokattu,Poistettu,Aktiivinen,Lahihoitaja,Sairaanhoitaja,LOP,KIPU,I_V,LAS,GER,PSYK,ROKOTUS,Tyokokemus,Vanhustenhoito_kotihoito,Mielenterveys,Paivystys_Ensihoito,Lapset,Kehitysvammaiset,Hengityshalvaus,Ensihoitaja_AMK,Terveydenhoitaja,Katilo,Osastotyoskentely,Paivystys,Teho_osasto,Ensihoito,Kommentit")] tehtavia tehtavia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tehtavia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LoginID = new SelectList(db.login, "LoginID", "UserName", tehtavia.LoginID);
            ViewBag.HenkiloID = new SelectList(db.tyontekijat2, "HenkiloID", "Etunimi", tehtavia.HenkiloID);
            ViewBag.YritysID = new SelectList(db.yritys, "YritysID", "YritysNimi", tehtavia.YritysID);
            return View(tehtavia);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tehtavia tehtavia = db.tehtavia.Find(id);
            if (tehtavia == null)
            {
                return HttpNotFound();
            }
            return View(tehtavia);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tehtavia tehtavia = db.tehtavia.Find(id);
            db.tehtavia.Remove(tehtavia);
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
