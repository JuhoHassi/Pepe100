using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Pepe100.Models;
using Pepe100.ViewModels;

namespace Pepe100.Controllers
{
    public class tyontekijat2Controller : Controller
    {
        private pepeEntities db = new pepeEntities();

        ////// JÄTÄTARJOUS JA VASTATARJOUS POISTETTU GITHUBI JULKAISUSTA

        public ActionResult Index()
        {
            var id = Session["LoginID"];
            int? id2 = (int?)id;
            var tyontekijat2 = (from t in db.tyontekijat2 where t.LoginID == id2 select t);
            return View(tyontekijat2.ToList());
        }


        public ActionResult OmatVuorot()
        {
            var id = Session["HenkiloID"];
            int? id2 = (int?)id;
            var teht = (from t in db.tehtavia
                        where t.HenkiloID == id2 && t.TehtavaValmis == true
                        select t);
            return View(teht.ToList());
        }
        public ActionResult OmatTulevatVuorot()
        {
            var id = Session["HenkiloID"];
            int? id2 = (int?)id;
            var teht = (from t in db.tehtavia
                        where
                        t.HenkiloID == id2 && t.TehtavaValmis == null && t.TehtavaTaytetty == true
                        select t);
            return View(teht.ToList());
        }
        public ActionResult OmatTarjoukset()
        {
            var id = Session["HenkiloID"];
            int? id2 = (int?)id;

            var teht = (from t in db.tehtavia
                        where t.TehtavaTaytetty != true
                        from va in db.vastatarjoukset
                        where va.HenkiloID == id2 &&
                        va.TyoID == t.TyoID
                        select new Tarjoukset
                        {
                            YritysID = t.YritysID,
                            Tehtava = t.Tehtava,
                            TarjousHinta = t.TarjousHinta,
                            Aloitusaika = t.Aloitusaika,
                            Lopetusaika = t.Lopetusaika,
                            AikaYhteensa = t.AikaYhteensa,
                            TyoID = t.TyoID,
                            VastaTarjousHinta = va.VastaTarjousHinta,
                            Sijainti = t.Sijainti,
                            Puhelin = t.Puhelin,
                            Email = t.Email,
                            Luotu = t.Luotu,
                            Lahihoitaja = t.Lahihoitaja,
                            Sairaanhoitaja = t.Sairaanhoitaja,
                            LOP = t.LOP,
                            KIPU = t.KIPU,
                            I_V = t.I_V,
                            LAS = t.LAS,
                            GER = t.GER,
                            PSYK = t.PSYK,
                            ROKOTUS = t.ROKOTUS,
                            Tyokokemus = t.Tyokokemus,
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
                            Teho_osasto = t.Teho_osasto,
                            Ensihoito = t.Ensihoito,
                            Kommentit = t.Kommentit
                        });

            return View(teht.ToList());
        }
        public ActionResult AvoimetVuorotTesti(int? id3)
        {
            if (id3 != null)
            {
                ViewBag.TarjousError = 1;
                var testi2 = (from t in db.tehtavia
                              where t.TyoID == id3
                              select t);
                return PartialView("_JataTarjous", testi2);
            }
            var id = Session["HenkiloID"];
            int? id2 = (int?)id;

            string aika = DateTime.Now.ToString();
            DateTime aika3 = DateTime.ParseExact(aika, "dd/MM/yyyy HH:mm:ss", null);

            var joo = (from va in db.vastatarjoukset
                       where
                       va.HenkiloID == id2 &&
                       va.VastaTarjousHinta != null
                       select
                       va.TyoID).ToArray();

            var testi = (from t in db.tehtavia
                         from ty in db.tyontekijat2
                         where ty.HenkiloID == id2
                         where !joo.Contains(t.TyoID)
                         where
                         (t.Lahihoitaja == ty.Lahihoitaja || ty.Lahihoitaja == true) &&
                         (t.Sairaanhoitaja == ty.Sairaanhoitaja || ty.Sairaanhoitaja == true) &&
                         (t.LOP == ty.LOP || ty.LOP == true) &&
                         (t.I_V == ty.I_V || ty.I_V == true) &&
                         (t.KIPU == ty.KIPU || ty.KIPU == true) &&
                         (t.LAS == ty.LAS || ty.LAS == true) &&
                         (t.GER == ty.GER || ty.GER == true) &&
                         (t.PSYK == ty.PSYK || ty.PSYK == true) &&
                         (t.ROKOTUS == ty.ROKOTUS || ty.ROKOTUS == true) &&
                         (t.Tyokokemus <= ty.Tyokokemus) &&
                         (t.Vanhustenhoito_kotihoito == ty.Vanhustenhoito_kotihoito || ty.Vanhustenhoito_kotihoito == true) &&
                         (t.Mielenterveys == ty.Mielenterveys || ty.Mielenterveys == true) &&
                         (t.Paivystys == ty.Paivystys || ty.Paivystys == true) &&
                         (t.Lapset == ty.Lapset || ty.Lapset == true) &&
                         (t.Kehitysvammaiset == ty.Kehitysvammaiset || ty.Kehitysvammaiset == true) &&
                         (t.Hengityshalvaus == ty.Hengityshalvaus || ty.Hengityshalvaus == true) &&
                         (t.Ensihoitaja_AMK == ty.Ensihoitaja_AMK || ty.Ensihoitaja_AMK == true) &&
                         (t.Terveydenhoitaja == ty.Terveydenhoitaja || ty.Terveydenhoitaja == true) &&
                         (t.Katilo == ty.Katilo || ty.Katilo == true) &&
                         (t.Osastotyoskentely == ty.Osastotyoskentely || ty.Osastotyoskentely == true) &&
                         (t.Paivystys == ty.Paivystys || ty.Paivystys == true) &&
                         (t.Teho_osasto == ty.Teho_osasto || ty.Teho_osasto == true) &&
                         (t.Ensihoito == ty.Ensihoito || ty.Ensihoito == true) &&
                         (t.TehtavaTaytetty != true) &&
                         (t.YritysID != null) &&
                         (t.Aloitusaika >= aika3)
                         orderby t.Aloitusaika
                         select t
                         );

            return View(testi);
        }
        public ActionResult AvoimetVuorot()
        {
            //Hae työntekijän tiedot, Vertaa tehtäviin, jos tiedot on samat tai "paremmat" palauta tehtävät näkymään
            var id = Session["HenkiloID"];
            int? id2 = (int?)id;

            string aika = DateTime.Now.ToString();
            DateTime aika3 = DateTime.ParseExact(aika, "dd/MM/yyyy HH:mm:ss", null);

            var joo = (from va in db.vastatarjoukset
                       where
                       va.HenkiloID == id2 &&
                       va.VastaTarjousHinta != null
                       select
                       va.TyoID).ToArray();

            var testi = (from t in db.tehtavia
                         from ty in db.tyontekijat2
                         where ty.HenkiloID == id2
                         where !joo.Contains(t.TyoID)
                         where
                         (t.Lahihoitaja == ty.Lahihoitaja || ty.Lahihoitaja == true) &&
                         (t.Sairaanhoitaja == ty.Sairaanhoitaja || ty.Sairaanhoitaja == true) &&
                         (t.LOP == ty.LOP || ty.LOP == true) &&
                         (t.I_V == ty.I_V || ty.I_V == true) &&
                         (t.KIPU == ty.KIPU || ty.KIPU == true) &&
                         (t.LAS == ty.LAS || ty.LAS == true) &&
                         (t.GER == ty.GER || ty.GER == true) &&
                         (t.PSYK == ty.PSYK || ty.PSYK == true) &&
                         (t.ROKOTUS == ty.ROKOTUS || ty.ROKOTUS == true) &&
                         (t.Tyokokemus <= ty.Tyokokemus) &&
                         (t.Vanhustenhoito_kotihoito == ty.Vanhustenhoito_kotihoito || ty.Vanhustenhoito_kotihoito == true) &&
                         (t.Mielenterveys == ty.Mielenterveys || ty.Mielenterveys == true) &&
                         (t.Paivystys == ty.Paivystys || ty.Paivystys == true) &&
                         (t.Lapset == ty.Lapset || ty.Lapset == true) &&
                         (t.Kehitysvammaiset == ty.Kehitysvammaiset || ty.Kehitysvammaiset == true) &&
                         (t.Hengityshalvaus == ty.Hengityshalvaus || ty.Hengityshalvaus == true) &&
                         (t.Ensihoitaja_AMK == ty.Ensihoitaja_AMK || ty.Ensihoitaja_AMK == true) &&
                         (t.Terveydenhoitaja == ty.Terveydenhoitaja || ty.Terveydenhoitaja == true) &&
                         (t.Katilo == ty.Katilo || ty.Katilo == true) &&
                         (t.Osastotyoskentely == ty.Osastotyoskentely || ty.Osastotyoskentely == true) &&
                         (t.Paivystys == ty.Paivystys || ty.Paivystys == true) &&
                         (t.Teho_osasto == ty.Teho_osasto || ty.Teho_osasto == true) &&
                         (t.Ensihoito == ty.Ensihoito || ty.Ensihoito == true) &&
                         (t.TehtavaTaytetty != true) &&
                         (t.YritysID != null) &&
                         (t.Aloitusaika >= aika3)
                         select t
                         );

            return View(testi);
        }
        public ActionResult _AvoimetVuorot(int? tyoid)
        {
            if ((tyoid == null) || (tyoid == 0))
            {
                return HttpNotFound();
            }
            else
            {
                var tilausRivejaLista = (from t in db.tehtavia
                                         where t.TyoID == tyoid
                                         select t);

                return PartialView(tilausRivejaLista);
            }
        }

        public ActionResult VastaTarjous(int? id)
        {
            tehtavia tehtavia = db.tehtavia.Find(id);
            return View(tehtavia);
        }


        public ActionResult Index2(int? tyoid)
        {
            tehtavia tehtavia = db.tehtavia.Find(tyoid);

            ViewBag.TarjousError = 1;
            return View(tehtavia);
        }




        public ActionResult Create()
        {
            ViewBag.LoginID = new SelectList(db.login, "LoginID", "UserName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HenkiloID,LoginID,Etunimi,Sukunimi,Puhelin,Email,EmailMessage,Luotu,ViimeksiMuokattu,Poistettu,Aktiivinen,Lahihoitaja,Sairaanhoitaja,LOP,KIPU,I_V,LAS,GER,PSYK,ROKOTUS,Tyokokemus,Vanhustenhoito_kotihoito,Mielenterveys,Paivystys_Ensihoito,Lapset,Kehitysvammaiset,Hengityshalvaus,Ensihoitaja_AMK,Terveydenhoitaja,Katilo,Osastotyoskentely,Paivystys,Teho_osasto,Ensihoito,Aloitusaika,Lopetusaika,Sijainti,Kommentit")] tehtavia tyontekijat2)
        {
            if (ModelState.IsValid)
            {
                var id = Session["HenkiloID"];
                int? id2 = (int?)id;


                tyontekijat2 lisa = (from t in db.tyontekijat2
                                     where t.HenkiloID == id2
                                     select t).FirstOrDefault();

                tehtavia tehtavia = new tehtavia()
                {
                    HenkiloID = id2,
                    TehtavaTaytetty = false,
                    TehtavaValmis = false,
                    Sijainti = tyontekijat2.Sijainti,
                    Etunimi = lisa.Etunimi,
                    Sukunimi = lisa.Sukunimi,
                    Puhelin = lisa.Puhelin,
                    Email = lisa.Email,
                    Aloitusaika = tyontekijat2.Aloitusaika,
                    Lopetusaika = tyontekijat2.Lopetusaika,
                    AikaYhteensa = (tyontekijat2.Lopetusaika - tyontekijat2.Aloitusaika).ToString(),
                    Luotu = DateTime.Now,
                    ViimeksiMuokattu = DateTime.Now,
                    Aktiivinen = true,
                    Lahihoitaja = lisa.Lahihoitaja,
                    Sairaanhoitaja = lisa.Sairaanhoitaja,
                    LOP = lisa.LOP,
                    KIPU = lisa.KIPU,
                    I_V = lisa.I_V,
                    LAS = lisa.LAS,
                    GER = lisa.GER,
                    PSYK = lisa.PSYK,
                    ROKOTUS = lisa.ROKOTUS,
                    Tyokokemus = lisa.Tyokokemus,
                    Vanhustenhoito_kotihoito = lisa.Vanhustenhoito_kotihoito,
                    Mielenterveys = lisa.Mielenterveys,
                    Paivystys_Ensihoito = lisa.Paivystys_Ensihoito,
                    Lapset = lisa.Lapset,
                    Kehitysvammaiset = lisa.Kehitysvammaiset,
                    Hengityshalvaus = lisa.Hengityshalvaus,
                    Ensihoitaja_AMK = lisa.Ensihoitaja_AMK,
                    Terveydenhoitaja = lisa.Terveydenhoitaja,
                    Katilo = lisa.Katilo,
                    Osastotyoskentely = lisa.Osastotyoskentely,
                    Paivystys = lisa.Paivystys,
                    Teho_osasto = lisa.Teho_osasto,
                    Ensihoito = lisa.Ensihoito,
                    Kommentit = tyontekijat2.Kommentit
                };

                db.tehtavia.Add(tehtavia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LoginID = new SelectList(db.login, "LoginID", "UserName", tyontekijat2.LoginID);
            return View(tyontekijat2);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tyontekijat2 tyontekijat2 = db.tyontekijat2.Find(id);
            if (tyontekijat2 == null)
            {
                return HttpNotFound();
            }
            ViewBag.LoginID = new SelectList(db.login, "LoginID", "UserName", tyontekijat2.LoginID);
            return View(tyontekijat2);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HenkiloID,LoginID,Etunimi,Sukunimi,Puhelin,Email,EmailMessage,Luotu,ViimeksiMuokattu,Poistettu,Aktiivinen,Lahihoitaja,Sairaanhoitaja,LOP,KIPU,I_V,LAS,GER,PSYK,ROKOTUS,Tyokokemus,Vanhustenhoito_kotihoito,Mielenterveys,Paivystys_Ensihoito,Lapset,Kehitysvammaiset,Hengityshalvaus,Ensihoitaja_AMK,Terveydenhoitaja,Katilo,Osastotyoskentely,Paivystys,Teho_osasto,Ensihoito,Kommentit")] tyontekijat2 tyontekijat2)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tyontekijat2).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LoginID = new SelectList(db.login, "LoginID", "UserName", tyontekijat2.LoginID);
            return View(tyontekijat2);
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
