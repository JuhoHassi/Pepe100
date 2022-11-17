using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.AcroForms;
using PdfSharp.Pdf.Content;
using PdfSharp.Pdf.IO;
using Pepe100.Models;
using Pepe100.ViewModels;

namespace Pepe10.Controllers
{
    public class YritysController : Controller
    {
        private pepeEntities db = new pepeEntities();

        ////// LAHIHOITAJATILAUS, SAIRAANHOITAJATILAUS, _HYVAKSYTARJOUS, KATSOTARJOUKSIA, HYVAKSYVUORO JA PDFTÄYTÖT JA MUOKKAUKSET POISTETTU GITHUBI JULKAISUSTA

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

        public ActionResult OmatVuorotYritys()
        {
            var id = Session["YritysID"];
            int? id2 = (int?)id;
            var teht = (from t in db.tehtavia
                        where t.YritysID == id2 && t.TehtavaValmis == true
                        orderby t.Lopetusaika
                        select t);
            return View(teht.ToList());
        }

        public ActionResult AvoimetVuorotYritys()
        {
            var id = Session["YritysID"];
            int? id2 = (int?)id;
            string aika = DateTime.Now.ToString();
            DateTime aika3 = DateTime.ParseExact(aika, "dd/MM/yyyy HH:mm:ss", null);

            var testi = (from t in db.tehtavia
                         where t.YritysID == id2 && 
                         t.TehtavaTaytetty != true &&
                         t.Aloitusaika >= aika3
                         orderby t.Aloitusaika
                         select t);

            return View(testi.ToList());
        }
        public ActionResult _AvoimetVuorotYritys(int? tehtavaid)
        {
            if ((tehtavaid == null) || (tehtavaid == 0))
            {
                return HttpNotFound();
            }
            else
            {
                var tilausRivejaLista = (from t in db.tehtavia
                                        where t.TyoID == tehtavaid
                                        select t);
                return PartialView(tilausRivejaLista);
            }
        }
        public ActionResult AvoimetVuorotYritysTesti()
        {
            var id = Session["YritysID"];
            int? id2 = (int?)id;
            string aika = DateTime.Now.ToString();
            DateTime aika3 = DateTime.ParseExact(aika, "dd/MM/yyyy HH:mm:ss", null);

            var testi = (from t in db.tehtavia
                         where t.YritysID == id2 &&
                         t.TehtavaTaytetty != true &&
                         t.Aloitusaika >= aika3
                         orderby t.Aloitusaika
                         select t);

            return View(testi);
        }
        public ActionResult _AvoimetVuorotYritysTesti(int? tyoid)
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

        public ActionResult YleisetAvoimetVuorot()
        {
            //Hae työntekijän tiedot, Vertaa tehtäviin, jos tiedot on samat tai "paremmat" palauta tehtävät näkymään
            var id = Session["YritysID"];
            int? id2 = (int?)id;
            var testi = (from t in db.tehtavia
                         where t.YritysID == null && t.TehtavaTaytetty != true && t.HenkiloID != null
                         select t);

            return View(testi.ToList());
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


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            yritys yrity = db.yritys.Find(id);
            if (yrity == null)
            {
                return HttpNotFound();
            }
            return View(yrity);
        }

        // GET: Yritys/Create


        public ActionResult LahihoitajaTilaus()
        {
            return View();
        }




        public ActionResult SairaanhoitajaTilaus()
        {
            return View();
        }


        public ActionResult TulevatVuorotYritys()
        {
            //Hae työntekijän tiedot, Vertaa tehtäviin, jos tiedot on samat tai "paremmat" palauta tehtävät näkymään
            var id = Session["YritysID"];
            int? id2 = (int?)id;

            var testi = (from t in db.tehtavia
                         where t.YritysID == id2 && t.TehtavaTaytetty == true && t.TehtavaValmis == null
                         orderby t.Aloitusaika
                         select t);

            return View(testi.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TulevatVuorotYritys([Bind(Include = "TyoID,HenkiloID,YritysID,LoginID,Tehtava,TehtavaTaytettu,TehtavaValmis,Sijainti,Etunimi,Sukunimi,Puhelin,Email,EmailMessage,Aloitusaika,Lopetusaika,AikaYhteensa,Luotu,ViimeksiMuokattu,Poistettu,Aktiivinen,Lahihoitaja,Sairaanhoitaja,LOP,KIPU,I_V,LAS,GER,PSYK,ROKOTUS,Tyokokemus,Vanhustenhoito_kotihoito,Mielenterveys,Paivystys_Ensihoito,Lapset,Kehitysvammaiset,Hengityshalvaus,Ensihoitaja_AMK,Terveydenhoitaja,Katilo,Osastotyoskentely,Paivystys,Teho_osasto,Ensihoito,Kommentit,TarjousHinta")] tehtavia tehtavia)
        {
            //Hae työntekijän tiedot, Vertaa tehtäviin, jos tiedot on samat tai "paremmat" palauta tehtävät näkymään
            var id = Session["YritysID"];
            int? id2 = (int?)id;

            var testi = (from t in db.tehtavia
                         where t.TyoID == tehtavia.TyoID
                         select t).FirstOrDefault();

            testi.TehtavaValmis = true;
            db.SaveChanges();
            return RedirectToAction("TulevatVuorotYritys");
        }
        public ActionResult TulevatVuorotHyvaksyYritys(int? id)
        {
            tehtavia tehtavia = db.tehtavia.Find(id);
            return View(tehtavia);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TulevatVuorotHyvaksyYritys([Bind(Include = "Arvostelu")] tehtavia tehtavia,int id, int? rate)
        {
            DateTime? aika = (from t in db.tehtavia
                              where id == t.TyoID
                              select t.Lopetusaika).FirstOrDefault();

            if (aika < DateTime.Now)
            {
                tehtavia teht = (from t in db.tehtavia
                                 where t.TyoID == id
                                 select t).FirstOrDefault();
                teht.Arvostelu = rate;
                teht.TehtavaValmis = true;
                db.SaveChanges();


                tyontekijat2 tyontekijat = (from t in db.tyontekijat2
                                            where teht.HenkiloID == t.HenkiloID
                                            select t).FirstOrDefault();

                Double? arvostelu = ((tyontekijat.Arvostelu * tyontekijat.ArvosteluMaara) + rate) / (tyontekijat.ArvosteluMaara + 1);


                tyontekijat.Arvostelu = arvostelu;
                tyontekijat.ArvosteluMaara++;
                db.SaveChanges();

                return RedirectToAction("TulevatVuorotYritys");
            }
            else
            {
                TempData["VuoroOnKesken"] = "Vuoro ei ole vielä loppunut!";
                return RedirectToAction("TulevatVuorotYritys");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _HyvaksyTarjous(vastatarjoukset tarjoukset)
        {
            var id = Session["YritysID"];
            int? id2 = (int?)id;
            return View();
        }

        public ActionResult KatsoTarjouksia(int? id1,int? henkid)
        {
            if (henkid != null)
            {
                ViewBag.TarjousError = 1;
                var testi3 = (from t in db.tyontekijat2
                              where t.HenkiloID == henkid
                              select t);
                return PartialView("_InfoRuutu", testi3);
            }
            else
            {

            var id = Session["YritysID"];
            int? id2 = (int?)id;

            var testi = (from v in db.vastatarjoukset
                         where v.YritysID == id2 &&
                          v.TyoID == id1 &&
                          v.HenkiloID != null
                         select v);

            var testi4 = (from t in db.tyontekijat2
                          from v in db.vastatarjoukset.Where(x => t.HenkiloID == x.HenkiloID &&
                          x.YritysID == id2 &&
                          x.TyoID == id1)
                          select new TyontekijatViewModel
                          {
                              HenkiloID = t.HenkiloID,
                              TyoID = v.TyoID,
                              VastaTarjousHinta = v.VastaTarjousHinta,
                              Etunimi = t.Etunimi,
                              Sukunimi = t.Sukunimi,
                              Email = t.Email,
                              Lahihoitaja = t.Lahihoitaja,
                              Sairaanhoitaja = t.Sairaanhoitaja,
                              LOP = t.LOP,
                              I_V = t.I_V,
                              KIPU = t.KIPU,
                              LAS = t.LAS,
                              GER = t.GER,
                              PSYK = t.PSYK,
                              ROKOTUS = t.ROKOTUS,
                              Tyokokemus = t.Tyokokemus,
                              Vanhustenhoito_kotihoito = t.Vanhustenhoito_kotihoito,
                              Mielenterveys = t.Mielenterveys,
                              Paivystys_Ensihoito = t.Paivystys_Ensihoito,
                              Paivystys = t.Paivystys,
                              Lapset = t.Lapset,
                              Kehitysvammaiset = t.Kehitysvammaiset,
                              Hengityshalvaus = t.Hengityshalvaus,
                              Ensihoitaja_AMK = t.Ensihoitaja_AMK,
                              Terveydenhoitaja = t.Terveydenhoitaja,
                              Katilo = t.Katilo,
                              Osastotyoskentely = t.Osastotyoskentely,
                              Teho_osasto = t.Teho_osasto,
                              Ensihoito = t.Ensihoito,
                              Kommentit = t.Kommentit,
                              Kuva = t.Kuva,
                              Arvostelu = Math.Round((double)t.Arvostelu,2)
                          });

            return View(testi4);
            }

        }

        public ActionResult HyvaksyVuoro(int? id, int? henkid)
        {
            vastatarjoukset tarjoukset = db.vastatarjoukset.Find(id);
            return View(tarjoukset);
        }




        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            yritys yrity = db.yritys.Find(id);
            if (yrity == null)
            {
                return HttpNotFound();
            }
            return View(yrity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "YritysID,YritysNimi,YritysEmail")] yritys yrity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(yrity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(yrity);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            yritys yrity = db.yritys.Find(id);
            if (yrity == null)
            {
                return HttpNotFound();
            }
            return View(yrity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            yritys yrity = db.yritys.Find(id);
            db.yritys.Remove(yrity);
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
