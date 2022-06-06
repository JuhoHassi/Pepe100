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

        // GET: Yritys
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
            //return View(db.Yritys.ToList());
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
            //var teht = (from t in db.tyontekijat2 where t.HenkiloID == id2 select t);
            string aika = DateTime.Now.ToString();
            DateTime aika3 = DateTime.ParseExact(aika, "dd/MM/yyyy HH:mm:ss", null);

            var testi = (from t in db.tehtavia
                         where t.YritysID == id2 && 
                         t.TehtavaTaytetty != true &&
                         t.Aloitusaika >= aika3
                         orderby t.Aloitusaika
                         select t);
            //var tarjous = (from v in db.vastatarjoukset
            //               where );

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
                                        //join tu in db.Tyoaika on ti.TehtavaID equals tu.TehtavaID
                                        select t);
                                        //new tehtavia
                                        //{
                                        //    TehtavaID = ti.TehtavaID,
                                        //    TyömaaNro = ti.TyömaaNro,
                                        //    Maaraaika = ti.Maaraaika,
                                        //    Aloitusaika = tu.Aloitusaika,
                                        //    Lopetusaika = tu.Lopetusaika,
                                        //    TyoaikaYhteensa = tu.TyoaikaYhteensa
                                        //};

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


            //return View(testi.ToList());
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


        //public ActionResult KatsoTarjouksia2(List<vastatarjoukset> Vastatarjoukset)
        //{

        //    //Hae työntekijän tiedot, Vertaa tehtäviin, jos tiedot on samat tai "paremmat" palauta tehtävät näkymään
        //    //var id = Session["YritysID"];
        //    //int? id2 = (int?)id;
        //    ////var teht = (from t in db.tyontekijat2 where t.HenkiloID == id2 select t);

        //    //var testi = (from v in db.vastatarjoukset
        //    //             where v.YritysID == id2 && v.TyoID == id1 && v.HenkiloID != null
        //    //             select v);

        //    return View();
        //}
        public ActionResult YleisetAvoimetVuorot()
        {
            //Hae työntekijän tiedot, Vertaa tehtäviin, jos tiedot on samat tai "paremmat" palauta tehtävät näkymään
            var id = Session["YritysID"];
            int? id2 = (int?)id;
            //var teht = (from t in db.tyontekijat2 where t.HenkiloID == id2 select t);

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
            //var hoitajat2 = db.Hoitajat.Include(h => h.Tyontekijat);
            //return View(hoitajat2.ToList());
        }


        // GET: Yritys/Details/5
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

        // POST: Yritys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "HenkiloID, LoginID, Etunimi, Sukunimi, Puhelin, Email, Luotu, ViimeksiMuokattu, Poistettu, Aktiivinen, EmailMessage,  Lahihoitaja, Sairaanhoitaja, LOP, I_V, KIPU,LAS, GER, PSYK, ROKOTUS, Tyokokemus, Vanhustenhoito_kotihoito, Mielenterveys, Paivystys_Ensihoito, Lapset, Kehitysvammaiset, Hengityshalvaus, Ensihoitaja_AMK, Terveydenhoitaja, Katilo, Osastotyoskentely, Paivystys, Teho_osasto, Ensihoito, Kommentit")] TyontekijatViewModel tyontekijat)
        public ActionResult LahihoitajaTilaus([Bind(Include = "YritysID, HenkiloID, LoginID,Tehtava, Etunimi, Sukunimi, Puhelin, Email, Luotu, ViimeksiMuokattu, Poistettu, Aktiivinen, EmailMessage,  Lahihoitaja, Sairaanhoitaja, LOP, I_V, KIPU, LAS, GER, PSYK, ROKOTUS, Tyokokemus, Aloitusaika, Lopetusaika, Vanhustenhoito_kotihoito, Mielenterveys, Paivystys_Ensihoito, Lapset, Kehitysvammaiset, Hengityshalvaus, Ensihoitaja_AMK, Terveydenhoitaja, Katilo, Osastotyoskentely, Paivystys, Teho_osasto, Ensihoito, Kommentit,TarjousHinta,Kuukausipalkkalainen,Tuntipalkkalainen,Sijainti")] tehtavia tyontekijat)
        {
            //YRITYSID pitää olla
            if (ModelState.IsValid)
            {
                if (tyontekijat.Lopetusaika > tyontekijat.Aloitusaika)
                {

                var yid = Session["YritysID"];
                int? yid2 = (int?)yid;
                    if (yid != null)
                    {

                        var sposti = (from y in db.yritys
                                      where yid2 == y.YritysID
                                      select y.YritysEmail).FirstOrDefault();

                        string aika = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        DateTime aika3 = DateTime.ParseExact(aika, "dd/MM/yyyy HH:mm:ss", null);

                        tehtavia teh = new tehtavia()
                        {
                            Tehtava = tyontekijat.Tehtava,
                            Kuukausipalkkalainen = tyontekijat.Kuukausipalkkalainen,
                            Tuntipalkkalainen = tyontekijat.Tuntipalkkalainen,
                            Sijainti = tyontekijat.Sijainti,
                            YritysID = yid2,
                            Email = sposti,
                            Lahihoitaja = true,
                            Sairaanhoitaja = tyontekijat.Sairaanhoitaja,
                            LOP = tyontekijat.LOP,
                            I_V = tyontekijat.I_V,
                            KIPU = tyontekijat.KIPU,
                            LAS = tyontekijat.LAS,
                            GER = tyontekijat.GER,
                            PSYK = tyontekijat.PSYK,
                            ROKOTUS = tyontekijat.ROKOTUS,
                            Tyokokemus = tyontekijat.Tyokokemus,
                            Aloitusaika = tyontekijat.Aloitusaika,
                            Lopetusaika = tyontekijat.Lopetusaika,
                            AikaYhteensa = (tyontekijat.Lopetusaika - tyontekijat.Aloitusaika).ToString(),
                            Luotu = aika3,
                            Vanhustenhoito_kotihoito = tyontekijat.Vanhustenhoito_kotihoito,
                            Mielenterveys = tyontekijat.Mielenterveys,
                            Paivystys_Ensihoito = tyontekijat.Paivystys_Ensihoito,
                            Paivystys = tyontekijat.Paivystys,
                            Lapset = tyontekijat.Lapset,
                            Kehitysvammaiset = tyontekijat.Kehitysvammaiset,
                            Hengityshalvaus = tyontekijat.Hengityshalvaus,
                            Ensihoitaja_AMK = tyontekijat.Ensihoitaja_AMK,
                            Terveydenhoitaja = tyontekijat.Terveydenhoitaja,
                            Katilo = tyontekijat.Katilo,
                            Osastotyoskentely = tyontekijat.Osastotyoskentely,
                            Teho_osasto = tyontekijat.Teho_osasto,
                            Ensihoito = tyontekijat.Ensihoito,
                            Kommentit = tyontekijat.Kommentit,
                            TarjousMaara = 0
                            //TarjousHinta = tyontekijat.TarjousHinta
                        };
                        db.tehtavia.Add(teh);
                        db.SaveChanges();

                        var id = (from t in db.tehtavia
                                  where
                                  t.Luotu == teh.Luotu
                                  select
                                  t.TyoID).FirstOrDefault();

                        vastatarjoukset tarjoukset = new vastatarjoukset()
                        {
                            TyoID = id,
                            YritysID = yid2,
                            TarjousHinta = tyontekijat.TarjousHinta
                        };
                        db.vastatarjoukset.Add(tarjoukset);
                        db.SaveChanges();

                        var testi = (from t in db.tyontekijat2
                                     where
                                     (t.Lahihoitaja == tyontekijat.Lahihoitaja || t.Lahihoitaja == true) &&
                                     (t.LOP == tyontekijat.LOP || t.LOP == true) &&
                                     (t.I_V == tyontekijat.I_V || t.I_V == true) &&
                                     (t.KIPU == tyontekijat.KIPU || t.KIPU == true) &&
                                     (t.LAS == tyontekijat.LAS || t.LAS == true) &&
                                     (t.GER == tyontekijat.GER || t.GER == true) &&
                                     (t.PSYK == tyontekijat.PSYK || t.PSYK == true) &&
                                     (t.Tyokokemus >= tyontekijat.Tyokokemus) &&
                                     (t.Vanhustenhoito_kotihoito == tyontekijat.Vanhustenhoito_kotihoito || t.Vanhustenhoito_kotihoito == true) &&
                                     (t.Mielenterveys == tyontekijat.Mielenterveys || t.Mielenterveys == true) &&
                                     (t.Paivystys_Ensihoito == tyontekijat.Paivystys_Ensihoito || t.Paivystys_Ensihoito == true) &&
                                     (t.Lapset == tyontekijat.Lapset || t.Lapset == true) &&
                                     (t.Kehitysvammaiset == tyontekijat.Kehitysvammaiset || t.Kehitysvammaiset == true) &&
                                     (t.Hengityshalvaus == tyontekijat.Hengityshalvaus || t.Hengityshalvaus == true) &&
                                     (t.Sijainti.Contains(tyontekijat.Sijainti))
                                     select
                                     t.Email).ToArray();

                        for (int i = 0; i < testi.Length; i++)
                        {
                            try
                            {
                                MailMessage mail = new MailMessage();
                                SmtpClient SmtpServer = new SmtpClient("smtp-mail.outlook.com");

                                mail.From = new MailAddress("juhoh-@hotmail.com", "NurseBid");
                                mail.To.Add(new MailAddress(testi[i]));
                                mail.Subject = "Avoin työvuoro";
                                mail.Body = "Avoin työvuoro." + "Aloitusaika: " + tyontekijat.Aloitusaika +
                                    "Lopetusaika: " + tyontekijat.Lopetusaika +
                                    "Yhteensä: " + (tyontekijat.Lopetusaika - tyontekijat.Aloitusaika) +
                                    "Tarjous: " + tyontekijat.TarjousHinta +
                                    "Hyväksy vuoro tai jätä vastatarjous täällä https://localhost:44344/";

                                SmtpServer.Host = "smtp-mail.outlook.com";
                                SmtpServer.Port = 587;
                                SmtpServer.EnableSsl = true;
                                SmtpServer.UseDefaultCredentials = false;
                                SmtpServer.Credentials = new System.Net.NetworkCredential("juhoh-@hotmail.com", "KarpalO");
                                SmtpServer.Send(mail);



                                //MailMessage mail = new MailMessage();
                                //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                                //mail.From = new MailAddress("juho.hassi@gmail.com", "NurseBid");
                                //mail.To.Add(new MailAddress(testi[i]));
                                //mail.Subject = "Avoin työvuoro";
                                //mail.Body = "Avoin työvuoro." + "Aloitusaika: " + tyontekijat.Aloitusaika +
                                //    "Lopetusaika: " + tyontekijat.Lopetusaika +
                                //    "Yhteensä: " + (tyontekijat.Lopetusaika - tyontekijat.Aloitusaika) +
                                //    "Tarjous: " + tyontekijat.TarjousHinta +
                                //    "Hyväksy vuoro tai jätä vastatarjous täällä https://localhost:44344/";







                                //string fileName = FilePathResult.
                                //string fileName = FilePathResult
                                //mail.Attachments.Add(new Attachment());
                                //String filePath = "Sopimusdocx.docx";
                                //var doc = PdfReader.Open(ControllerContext.HttpContext.Server.MapPath("C:\\Users\\juhoh\\source\\repos\\Pepe100\\Pepe100\\Sopimus\\Sopmiustesti.pdf"), PdfDocumentOpenMode.Modify);


                                //doc.AcroForm.Elements.SetValue("yy", new PdfString("Teesti"));

                                //doc.AcroForm.Fields["1005"].ReadOnly = false;
                                //doc.AcroForm.Elements.SetString("1005","Teesti");



                                //Response.ContentType = "application/ms-word";

                                //--------------------------------------------------


                                //HYVIÄ ESIMERKKEJÄ

                                //https://csharp.hotexamples.com/examples/PdfSharp.Pdf/PdfDocument/Save/php-pdfdocument-save-method-examples.html


                                //--------------------------------------------------
                                //PdfDocument doc = new PdfDocument();


                                //var doc2 = PdfReader.Open(ControllerContext.HttpContext.Server.MapPath("C:\\Users\\juhoh\\source\\repos\\Pepe100\\Pepe100\\Sopimus\\TestiSoppari34.pdf"), PdfDocumentOpenMode.Modify);
                                //var doc2 = PdfReader.Open(ControllerContext.HttpContext.Server.MapPath("~/Sopimus/Sopimusluonnos.pdf"), PdfDocumentOpenMode.Modify);


                                //var outputDoc = new PdfDocument();
                                //outputDoc.AddPage(doc2.Pages[0]);

                                //var page = outputDoc.Pages[0];

                                //PdfTextField Vastuuhenkilo = (PdfTextField)(doc2.AcroForm.Fields["Vastuuhenkilö"]);
                                //Vastuuhenkilo.Text = "TOIMISKO JO?";
                                //Vastuuhenkilo.ReadOnly = true;
                                //doc2.Save(Server.MapPath("~/Sopimus/TestiSoppari1212.pdf"));


                                //PdfPage page = doc2.AddPage();
                                //XGraphics gfx = XGraphics.FromPdfPage(page);

                                //XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
                                //XFont font = new XFont("Arial", 20, XFontStyle.Bold);
                                //gfx.DrawString("Testiteksti", font, XBrushes.Black,
                                //                new XRect(0, 0, page.Width, page.Height),
                                //                XStringFormats.BottomCenter);

                                //gfx.DrawString(aika, font, XBrushes.Black,
                                //                new XRect(0, 0, page.Width, page.Height),
                                //                XStringFormats.TopLeft);

                                //XGraphics.DrawString("Testiteksti", font,XBrushes.Red);


                                //if (doc2 != null)
                                //{
                                //    if (doc2.AcroForm != null)
                                //    {
                                //        MessageBox.Show("Acroform is object", "SUCCEEDED");
                                //        //pass acroform to some function for processing
                                //        doc2.Save(@"C:\\Users\\juhoh\\source\\repos\\Pepe100\\Pepe100\\Sopimus\\NEWCOPY.pdf");
                                //    }
                                //    else
                                //    {
                                //        MessageBox.Show("Acroform is null", "FAILED");
                                //    }
                                //}
                                //else
                                //{
                                //    MessageBox.Show("Uknown error opening document", "FAILED");
                                //}



                                //string templateDocPath = Server.MapPath("~/Sopimus/Sopimusluonnos.pdf");
                                //PdfDocument myTemplate = PdfReader.Open(templateDocPath, PdfDocumentOpenMode.Modify);
                                //PdfAcroForm form = myTemplate.AcroForm;


                                //if (form.Elements.ContainsKey("/NeedAppearances"))
                                //{
                                //    form.Elements["/NeedAppearances"] = new PdfSharp.Pdf.PdfBoolean(true);
                                //}
                                //else
                                //{
                                //    form.Elements.Add("/NeedAppearances", new PdfSharp.Pdf.PdfBoolean(true));
                                //}

                                //PdfTextField testField = (PdfTextField)(form.Fields["Vastuuhenkilö"]);
                                //testField.Text = "012345";

                                ////doc2.AcroForm.Fields["Vastuuhenkilö"].Value = new PdfString("abc");


                                ////doc2.AcroForm.Elements.SetValue("/Vastuuhenkilö", new PdfString("JOTAIN"));

                                //string filename = "C:\\Users\\juhoh\\source\\repos\\Pepe100\\Pepe100\\Sopimus\\TestiSoppari" + DateTime.Now.Minute + ".pdf";
                                //doc2.Save(filename);
                                //doc2.Save(Server.MapPath("C:\\Users\\juhoh\\source\\repos\\Pepe100\\Pepe100\\Sopimus\\TestiSoppari1212.pdf"));
                                ////Process.Start(filename);
                                ///
                                // the initial file




                                //string file = Server.MapPath("~/Sopimus/Sopimusluonnos.pdf");

                                //// load the pdf form manager
                                //PdfFormManager form = new PdfFormManager();
                                //form.Load(file);


                                //PdfFormFieldTextBox name =
                                //form.Fields["f1_1"] as PdfFormFieldTextBox;
                                //name.Text = "This is my name";

                                //// save pdf document
                                //byte[] pdf = form.Save();

                                //// close pdf document
                                //form.Close();

                                //// return resulted pdf document
                                //FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                                //fileResult.FileDownloadName = "Document.pdf";
                                ////return fileResult;


                                //SmtpServer.Host = "smtp.gmail.com";
                                //SmtpServer.Port = 587;
                                //SmtpServer.EnableSsl = true;
                                //SmtpServer.UseDefaultCredentials = false;
                                //SmtpServer.Credentials = new System.Net.NetworkCredential("juho.hassi@gmail.com", "Hasa6666");

                                //SmtpServer.Send(mail);
                                //filePath.Insert(0, "TESTI INSERTTI");
                                

                            }
                            catch (Exception)
                            {
                                throw;
                            }

                        }

                        //TempData["Message"] = "Sahkoposti on lahetetty"; // Viesti alertoituu kun palataan Index sivulle
                        return RedirectToAction("Index", "Home");
                            //return RedirectToAction("Index");
                        }
                    else
                    {
                        ViewBag.AikaError = "Kirjaudu sisään";
                        return View();

                    }
                }

                else
                {
                    ViewBag.AikaError = "Tarkista Aloitus- ja Lopetusajat ";
                    //return RedirectToAction("LahihoitajaTilaus", "Yritys");
                    //return View("LahihoitajaTilaus", "Yritys");
                    return View();
                }

            }

            ViewBag.HenkiloID = new SelectList(db.tyontekijat2, "HenkiloID", "Etunimi", tyontekijat.HenkiloID);
            return View(tyontekijat);
        }

        public ActionResult SairaanhoitajaTilaus()
        {
            return View();
        }

        // POST: Yritys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "HenkiloID, LoginID, Etunimi, Sukunimi, Puhelin, Email, Luotu, ViimeksiMuokattu, Poistettu, Aktiivinen, EmailMessage,  Lahihoitaja, Sairaanhoitaja, LOP, I_V, KIPU,LAS, GER, PSYK, ROKOTUS, Tyokokemus, Vanhustenhoito_kotihoito, Mielenterveys, Paivystys_Ensihoito, Lapset, Kehitysvammaiset, Hengityshalvaus, Ensihoitaja_AMK, Terveydenhoitaja, Katilo, Osastotyoskentely, Paivystys, Teho_osasto, Ensihoito, Kommentit")] TyontekijatViewModel tyontekijat)
        public ActionResult SairaanhoitajaTilaus([Bind(Include = "YritysID, HenkiloID, LoginID,Tehtava, Etunimi, Sukunimi, Puhelin, Email, Luotu, ViimeksiMuokattu, Poistettu, Aktiivinen, EmailMessage,  Lahihoitaja, Sairaanhoitaja, LOP, I_V, KIPU, LAS, GER, PSYK, ROKOTUS, Tyokokemus, Aloitusaika, Lopetusaika, Vanhustenhoito_kotihoito, Mielenterveys, Paivystys_Ensihoito, Lapset, Kehitysvammaiset, Hengityshalvaus, Ensihoitaja_AMK, Terveydenhoitaja, Katilo, Osastotyoskentely, Paivystys, Teho_osasto, Ensihoito, Kommentit,TarjousHinta,Kuukausipalkkalainen,Tuntipalkkalainen,Sijainti")] tehtavia tyontekijat)
        {
            if (ModelState.IsValid)
            {
                if (tyontekijat.Lopetusaika > tyontekijat.Aloitusaika)
                {
                    var yid = Session["YritysID"];
                    int? yid2 = (int?)yid;
                    if (yid != null)
                    {
                        var sposti = (from y in db.yritys
                                      where yid2 == y.YritysID
                                      select y.YritysEmail).FirstOrDefault();

                        var infoCulture = new CultureInfo("fi-FI");
                        string aika = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        //DateTime aika3 = DateTime.ParseExact(aika, "dd-MM-yyyy HH:mm:ss", null);
                        //DateTime aika3 = DateTime.Parse(aika, "dd-MM-yyyy HH:mm:ss");
                        DateTime aika3 = DateTime.ParseExact(aika, "dd/MM/yyyy HH:mm:ss", null);
                        //DateTime aika3 = DateTime.ParseExact(aika, "dd/MM/yyyy", null);
                        //string aika3 = DateTime.ParseExact(aika, "dd/MM/yyyy", null).ToString();

                        //DateTime aika3 = DateTime.ParseExact(aika, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        //DateTime aika3 = DateTime.ParseExact(aika, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        tehtavia teh = new tehtavia()
                        {
                            Sairaanhoitaja = true,
                            YritysID = yid2,
                            Email = sposti,
                            Tehtava = tyontekijat.Tehtava,
                            Sijainti = tyontekijat.Sijainti,
                            Aloitusaika = tyontekijat.Aloitusaika,
                            Lopetusaika = tyontekijat.Lopetusaika,
                            AikaYhteensa = (tyontekijat.Lopetusaika - tyontekijat.Aloitusaika).ToString(),
                            Luotu = aika3,
                            Kuukausipalkkalainen = tyontekijat.Kuukausipalkkalainen,
                            Tuntipalkkalainen = tyontekijat.Tuntipalkkalainen,
                            LOP = tyontekijat.LOP,
                            I_V = tyontekijat.I_V,
                            KIPU = tyontekijat.KIPU,
                            LAS = tyontekijat.LAS,
                            GER = tyontekijat.GER,
                            PSYK = tyontekijat.PSYK,
                            ROKOTUS = tyontekijat.ROKOTUS,
                            Ensihoitaja_AMK = tyontekijat.Ensihoitaja_AMK,
                            Terveydenhoitaja = tyontekijat.Terveydenhoitaja,
                            Katilo = tyontekijat.Katilo,
                            Tyokokemus = tyontekijat.Tyokokemus, // työkokemus INTiksi
                            Vanhustenhoito_kotihoito = tyontekijat.Vanhustenhoito_kotihoito,
                            Osastotyoskentely = tyontekijat.Osastotyoskentely,
                            Paivystys = tyontekijat.Paivystys,
                            Teho_osasto = tyontekijat.Teho_osasto,
                            Ensihoito = tyontekijat.Ensihoito,
                            Mielenterveys = tyontekijat.Mielenterveys,
                            Lapset = tyontekijat.Lapset,
                            Hengityshalvaus = tyontekijat.Hengityshalvaus,
                            Kommentit = tyontekijat.Kommentit,
                            TarjousMaara = 0
                        };
                        db.tehtavia.Add(teh);
                        db.SaveChanges();

                        var id = (from t in db.tehtavia
                                  where
                                  t.Luotu == teh.Luotu
                                  select
                                  t.TyoID).FirstOrDefault();

                        vastatarjoukset tarjoukset = new vastatarjoukset()
                        {
                            TyoID = id,
                            YritysID = yid2,
                            TarjousHinta = tyontekijat.TarjousHinta
                        };
                        db.vastatarjoukset.Add(tarjoukset);
                        db.SaveChanges();

                        var testi = (from t in db.tyontekijat2
                                     where
                                     (t.Sairaanhoitaja == tyontekijat.Sairaanhoitaja || t.Sairaanhoitaja == true) &&
                                     (t.LOP == tyontekijat.LOP || t.LOP == true) &&
                                     (t.I_V == tyontekijat.I_V || t.I_V == true) &&
                                     (t.KIPU == tyontekijat.KIPU || t.KIPU == true) &&
                                     (t.LAS == tyontekijat.LAS || t.LAS == true) &&
                                     (t.GER == tyontekijat.GER || t.GER == true) &&
                                     (t.PSYK == tyontekijat.PSYK || t.PSYK == true) &&
                                     (t.ROKOTUS == tyontekijat.ROKOTUS || t.ROKOTUS == true) &&
                                     (t.Ensihoitaja_AMK == tyontekijat.Ensihoitaja_AMK || t.Ensihoitaja_AMK == true) &&
                                     (t.Terveydenhoitaja == tyontekijat.Terveydenhoitaja || t.Terveydenhoitaja == true) &&
                                     (t.Katilo == tyontekijat.Katilo || t.Katilo == true) &&
                                     (t.Tyokokemus >= tyontekijat.Tyokokemus) &&
                                     (t.Vanhustenhoito_kotihoito == tyontekijat.Vanhustenhoito_kotihoito || t.Vanhustenhoito_kotihoito == true) &&
                                     (t.Osastotyoskentely == tyontekijat.Osastotyoskentely || t.Osastotyoskentely == true) &&
                                     (t.Paivystys == tyontekijat.Paivystys || t.Paivystys == true) &&
                                     (t.Teho_osasto == tyontekijat.Teho_osasto || t.Teho_osasto == true) &&
                                     (t.Ensihoito == tyontekijat.Ensihoito || t.Ensihoito == true) &&
                                     (t.Mielenterveys == tyontekijat.Mielenterveys || t.Mielenterveys == true) &&
                                     (t.Lapset == tyontekijat.Lapset || t.Lapset == true) &&
                                     (t.Hengityshalvaus == tyontekijat.Hengityshalvaus || t.Hengityshalvaus == true) &&
                                     (t.Sijainti.Contains(tyontekijat.Sijainti))
                                     select
                                     t.Email).ToArray();

                        for (int i = 0; i < testi.Length; i++)
                        {
                            try
                            {
                                MailMessage mail = new MailMessage();
                                SmtpClient SmtpServer = new SmtpClient("smtp-mail.outlook.com");

                                mail.From = new MailAddress("juhoh-@hotmail.com", "NurseBid");
                                mail.To.Add(new MailAddress(testi[i]));
                                mail.Subject = "Avoin työvuoro";
                                mail.Body = "Avoin työvuoro." + "Aloitusaika: " + tyontekijat.Aloitusaika +
                                    "Lopetusaika: " + tyontekijat.Lopetusaika +
                                    "Yhteensä: " + (tyontekijat.Lopetusaika - tyontekijat.Aloitusaika) +
                                    "Tarjous: " + tyontekijat.TarjousHinta +
                                    "Hyväksy vuoro tai jätä vastatarjous täällä https://localhost:44344/";

                                SmtpServer.Host = "smtp-mail.outlook.com";
                                SmtpServer.Port = 587;
                                SmtpServer.EnableSsl = true;
                                SmtpServer.UseDefaultCredentials = false;
                                SmtpServer.Credentials = new System.Net.NetworkCredential("juhoh-@hotmail.com", "KarpalO");
                                SmtpServer.Send(mail);


                                //MailMessage mail = new MailMessage();
                                //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                                //mail.From = new MailAddress("juho.hassi@gmail.com", "NurseBid");
                                //mail.To.Add(new MailAddress(testi[i]));
                                //mail.Subject = "Avoin työvuoro";
                                //mail.Body = "Avoin työvuoro." + "Aloitusaika: " + tyontekijat.Aloitusaika +
                                //    "Lopetusaika: " + tyontekijat.Lopetusaika +
                                //    "Yhteensä: " + (tyontekijat.Lopetusaika - tyontekijat.Aloitusaika) +
                                //    "Tarjous: " + tyontekijat.TarjousHinta +
                                //    "Hyväksy vuoro tai jätä vastatarjous täällä https://localhost:44344/";

                                //SmtpServer.Host = "smtp.gmail.com";
                                //SmtpServer.Port = 587;
                                //SmtpServer.EnableSsl = true;
                                //SmtpServer.UseDefaultCredentials = false;
                                //SmtpServer.Credentials = new System.Net.NetworkCredential("juho.hassi@gmail.com", "Hasa6666");
                                //SmtpServer.Send(mail);
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                        //TempData["Message"] = "Sahkoposti on lahetetty"; // Viesti alertoituu kun palataan Index sivulle
                        return RedirectToAction("Index", "Home");
                        //return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.AikaError = "Kirjaudu sisään";
                        return View();
                    }
                }
                else
                {
                    ViewBag.AikaError = "Tarkista Aloitus- ja Lopetusajat ";
                    return View();
                }
            }

            ViewBag.HenkiloID = new SelectList(db.tyontekijat2, "HenkiloID", "Etunimi", tyontekijat.HenkiloID);
            return View(tyontekijat);
        }
        public ActionResult TulevatVuorotYritys()
        {
            //Hae työntekijän tiedot, Vertaa tehtäviin, jos tiedot on samat tai "paremmat" palauta tehtävät näkymään
            var id = Session["YritysID"];
            int? id2 = (int?)id;
            //var teht = (from t in db.tyontekijat2 where t.HenkiloID == id2 select t);

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
            //var teht = (from t in db.tyontekijat2 where t.HenkiloID == id2 select t);

            var testi = (from t in db.tehtavia
                         where t.TyoID == tehtavia.TyoID
                         select t).FirstOrDefault();

            testi.TehtavaValmis = true;
            db.SaveChanges();
            return RedirectToAction("TulevatVuorotYritys");

            //return View(testi.ToList());
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
            //var id2 = Session["YritysID"];
            //int? id3 = (int?)id;
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
                //ViewBag.VuoroKesken = "Vuoro ei ole vielä loppunut";
                TempData["VuoroOnKesken"] = "Vuoro ei ole vielä loppunut!";
                return RedirectToAction("TulevatVuorotYritys");
                //return View();
            }
        }
        //public ActionResult _HyvaksyTarjous(vastatarjoukset tarjoukset)
        public ActionResult _HyvaksyTarjous(int? tyoid,int? henid, int? vhinta)
        {
            var id = Session["YritysID"];
            int? id2 = (int?)id;

            //var testi = (from v in db.vastatarjoukset
            //             where 
            //             v.YritysID == id2 &&
            //             v.TyoID == tyoid &&
            //             v.HenkiloID == henid
            //             select v);


            vastatarjoukset tarjoukset = (from v in db.vastatarjoukset
                                          where tyoid == v.TyoID &&
                                          henid == v.HenkiloID
                                          select v).FirstOrDefault();

            tarjoukset.ToteutunutHinta = vhinta;
            db.SaveChanges();


            tehtavia teht = (from t in db.tehtavia
                             where tyoid == t.TyoID
                             select t).FirstOrDefault();

            teht.HenkiloID = henid;
            teht.TehtavaTaytetty = true;
            teht.ToteutunutHinta = vhinta;
            db.SaveChanges();

            tyontekijat2 tyy = (from ty in db.tyontekijat2
                                where henid == ty.HenkiloID
                                select ty).FirstOrDefault();


            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("juho.hassi@gmail.com", "NurseBid");
            mail.To.Add(new MailAddress(tyy.Email));
            mail.Subject = "Avoin työvuoro";
            mail.Body = "Tarjouksesi on hyväksytty. Liitteenä sopimus";

            byte [] sopimus3 = (from y in db.yritys
                           where id2 == y.YritysID
                           select y.Sopimus).FirstOrDefault();

            MemoryStream stream = new MemoryStream(sopimus3);

            var doc = PdfReader.Open(stream,PdfDocumentOpenMode.Modify);

            if (doc.AcroForm.Elements.ContainsKey("/NeedAppearances") == false)
            {
                doc.AcroForm.Elements.Add("/NeedAppearances", new PdfBoolean(true));
            }
            else
            {
                doc.AcroForm.Elements.SetValue("/NeedAppearances", new PdfBoolean(true));
            }

            doc.AcroForm.Fields["Etunimi"].Value = new PdfString(vhinta.ToString());
            doc.Info.Title = "UUSI SOPIMUS TÄSÄ";

            MemoryStream aa = new MemoryStream();
            doc.Save(aa, false);
            byte[] bytes = aa.ToArray();
            doc.Close();

            yritys yri = (from yy in db.yritys
                          where id2 == yy.YritysID
                          select yy).FirstOrDefault();
            //Convert.ToBase64String(doc);
            yri.Sopimus = bytes;
            string sopimustasa= "sopimuss.pdf";
            db.SaveChanges();
            Attachment att = new Attachment(new MemoryStream(bytes),sopimustasa);
            mail.Attachments.Add(att);


        //https://stackoverflow.com/questions/2583982/how-to-add-an-email-attachment-from-a-byte-array


            SmtpServer.Host = "smtp.gmail.com";
            SmtpServer.Port = 587;
            SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential("juho.hassi@gmail.com", "Hasa6666");

            SmtpServer.Send(mail);

            return RedirectToAction("AvoimetVuorotYritysTesti");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _HyvaksyTarjous(vastatarjoukset tarjoukset)
        {
            var id = Session["YritysID"];
            int? id2 = (int?)id;

            //var testi = (from v in db.vastatarjoukset
            //             where v.YritysID == id2 &&
            //              v.TyoID == tyoid &&
            //              v.HenkiloID != null
            //             select v);


            //return View(testi);
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
                //var testi2 = (from t in db.tyontekijat2
                //              where t.HenkiloID == henkid
                //              select t);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KatsoTarjouksia( [Bind(Include = "TyoID,HenkiloID,TarjousHinta,VastaTarjousHinta,ToteutunutHinta")] vastatarjoukset tarjoukset, int? henkid)
        //public ActionResult KatsoTarjouksia(vastatarjoukset tarjoukset)
        {

            var id = Session["YritysID"];
            int? id2 = (int?)id;

            //if (henkid != null)
            //{
            //    ViewBag.TarjousError = 1;
            //    var testi2 = (from t in db.tehtavia
            //                  where t.HenkiloID == henkid
            //                  select t);
            //    return PartialView("_InfoRuutu", testi2);
            //}
            if (ModelState.IsValid)
            {
                vastatarjoukset vastatarjoukset = new vastatarjoukset()
                {
                    TyoID = tarjoukset.TyoID,
                    HenkiloID = tarjoukset.HenkiloID,
                    VastaTarjousHinta = tarjoukset.VastaTarjousHinta,
                    ToteutunutHinta = tarjoukset.VastaTarjousHinta,
                };
                db.vastatarjoukset.Add(vastatarjoukset);
                db.SaveChanges();

                tehtavia teht = (from t in db.tehtavia
                                 where tarjoukset.TyoID == t.TyoID
                                 select t).FirstOrDefault();

                teht.HenkiloID = tarjoukset.HenkiloID;
                teht.TehtavaTaytetty = true;
                teht.ToteutunutHinta = tarjoukset.VastaTarjousHinta;
                db.SaveChanges();
            }

            try
            {
                var email2 = (from t in db.tyontekijat2
                              where t.HenkiloID == tarjoukset.HenkiloID
                              select t.Email).ToArray();

                //email.ToString();
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("juho.hassi@gmail.com", "NurseBid");
                mail.To.Add(new MailAddress(email2[0]));
                mail.Subject = "Työvuoro hyväksytty";
                mail.Body = "Työvuorosi on hyväksytty  https://localhost:44344/ hinnalla: " + tarjoukset.VastaTarjousHinta;

                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.Port = 587;
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("juho.hassi@gmail.com", "Hasa6666");

                SmtpServer.Send(mail);
            }
            catch (Exception)
            {
                throw;
            }
            try
            {
                var henkid2 = (from v in db.vastatarjoukset
                              where tarjoukset.TyoID == v.TyoID &&
                              tarjoukset.HenkiloID != v.HenkiloID
                              select v.HenkiloID).ToArray();

                // Looppaa email4 email3:n määrällä?
                foreach (var item in henkid2)
                {
                    var email4 = (from t in db.tyontekijat2
                                  where t.HenkiloID == item
                                  select t.Email).ToArray();

                    foreach (var email in email4)
                    {

                        //email.ToString();
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                        mail.From = new MailAddress("juho.hassi@gmail.com", "NurseBid");
                        mail.To.Add(new MailAddress(email.ToString()));
                        mail.Subject = "Tarjoustasi ei hyväksytty";
                        mail.Body = "Tarjoustasi ei hyväksytty";

                        SmtpServer.Host = "smtp.gmail.com";
                        SmtpServer.Port = 587;
                        SmtpServer.EnableSsl = true;
                        SmtpServer.UseDefaultCredentials = false;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("juho.hassi@gmail.com", "Hasa6666");

                        SmtpServer.Send(mail);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("AvoimetVuorotYritys");
            //return View("KatsoTarjouksia");

            //return View(testi.ToList());
        }
        public ActionResult HyvaksyVuoro(int? id, int? henkid)
        {
            vastatarjoukset tarjoukset = db.vastatarjoukset.Find(id);
            //var tehtavia = (from t in db.tehtavia
            //                where t.TyoID == tarjoukset.TyoID
            //                select t).ToList();
            return View(tarjoukset);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HyvaksyVuoro([Bind(Include = "TyoID,HenkiloID,TarjousHinta,VastaTarjousHinta,ToteutunutHinta")] vastatarjoukset tarjoukset)
        {

            var id = Session["YritysID"];
            int? id2 = (int?)id;

            //DateTime? aika = (from t in db.tehtavia
            //                 where tarjoukset.TyoID == t.TyoID
            //                 select t.Lopetusaika).FirstOrDefault();

            //if (aika >= DateTime.Now)
            //{

            if (ModelState.IsValid)
            {
                vastatarjoukset vastatarjoukset = new vastatarjoukset()
                {
                    TyoID = tarjoukset.TyoID,
                    HenkiloID = tarjoukset.HenkiloID,
                    VastaTarjousHinta = tarjoukset.VastaTarjousHinta,
                    ToteutunutHinta = tarjoukset.VastaTarjousHinta,
                };
                db.vastatarjoukset.Add(vastatarjoukset);
                db.SaveChanges();

                tehtavia teht = (from t in db.tehtavia
                                 where tarjoukset.TyoID == t.TyoID
                                 select t).FirstOrDefault();

                teht.HenkiloID = tarjoukset.HenkiloID;
                teht.TehtavaTaytetty = true;
                teht.ToteutunutHinta = tarjoukset.VastaTarjousHinta;
                db.SaveChanges();
            }

            try
            {
                var email2 = (from t in db.tyontekijat2
                              where t.HenkiloID == tarjoukset.HenkiloID
                              select t.Email).ToArray();

                //email.ToString();
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("juho.hassi@gmail.com", "NurseBid");
                mail.To.Add(new MailAddress(email2[0]));
                mail.Subject = "Työvuoro hyväksytty";
                mail.Body = "Työvuorosi on hyväksytty  https://localhost:44344/ hinnalla: " + tarjoukset.VastaTarjousHinta;

                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.Port = 587;
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("juho.hassi@gmail.com", "Hasa6666");

                SmtpServer.Send(mail);
            }
            catch (Exception)
            {
                throw;
            }
            try
            {
                var henkid2 = (from v in db.vastatarjoukset
                              where tarjoukset.TyoID == v.TyoID &&
                              tarjoukset.HenkiloID != v.HenkiloID
                              select v.HenkiloID).ToArray();

                // Looppaa email4 email3:n määrällä?
                foreach (var item in henkid2)
                {
                    var email4 = (from t in db.tyontekijat2
                                  where t.HenkiloID == item
                                  select t.Email).ToArray();

                    foreach (var email in email4)
                    {

                        //email.ToString();
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                        mail.From = new MailAddress("juho.hassi@gmail.com", "NurseBid");
                        mail.To.Add(new MailAddress(email.ToString()));
                        mail.Subject = "Tarjoustasi ei hyväksytty";
                        mail.Body = "Tarjoustasi ei hyväksytty";

                        SmtpServer.Host = "smtp.gmail.com";
                        SmtpServer.Port = 587;
                        SmtpServer.EnableSsl = true;
                        SmtpServer.UseDefaultCredentials = false;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("juho.hassi@gmail.com", "Hasa6666");

                        SmtpServer.Send(mail);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("AvoimetVuorotYritys");
            //}
            //else
            //{
            //    ViewBag.VuoroKesken = "Vuoro ei ole vielä loppunut";
            //    return RedirectToAction("AvoimetVuorotYritys");
            //}
        }



        // GET: Yritys/Edit/5
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

        // POST: Yritys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Yritys/Delete/5
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

        // POST: Yritys/Delete/5
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

        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Yritys/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        ////public ActionResult Create([Bind(Include = "HenkiloID, LoginID, Etunimi, Sukunimi, Puhelin, Email, Luotu, ViimeksiMuokattu, Poistettu, Aktiivinen, EmailMessage,  Lahihoitaja, Sairaanhoitaja, LOP, I_V, KIPU,LAS, GER, PSYK, ROKOTUS, Tyokokemus, Vanhustenhoito_kotihoito, Mielenterveys, Paivystys_Ensihoito, Lapset, Kehitysvammaiset, Hengityshalvaus, Ensihoitaja_AMK, Terveydenhoitaja, Katilo, Osastotyoskentely, Paivystys, Teho_osasto, Ensihoito, Kommentit")] TyontekijatViewModel tyontekijat)
        //public ActionResult Create([Bind(Include = "YritysID, HenkiloID, LoginID, Etunimi, Sukunimi, Puhelin, Email, Luotu, ViimeksiMuokattu, Poistettu, Aktiivinen, EmailMessage,  Lahihoitaja, Sairaanhoitaja, LOP, I_V, KIPU, LAS, GER, PSYK, ROKOTUS, Tyokokemus, Aloitusaika, Lopetusaika, Vanhustenhoito_kotihoito, Mielenterveys, Paivystys_Ensihoito, Lapset, Kehitysvammaiset, Hengityshalvaus, Ensihoitaja_AMK, Terveydenhoitaja, Katilo, Osastotyoskentely, Paivystys, Teho_osasto, Ensihoito, Kommentit,TarjousHinta")] tehtavia tyontekijat)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var yid = Session["YritysID"];
        //        int? yid2 = (int?)yid;
        //        var sposti = (from y in db.Yritys
        //                      where yid2 == y.YritysID
        //                      select y.YritysEmail).FirstOrDefault();
        //        string aika = DateTime.Now.ToString();
        //        DateTime aika3 = DateTime.ParseExact(aika, "dd/MM/yyyy HH:mm:ss", null);

        //        tehtavia teh = new tehtavia()
        //        {
        //            YritysID = yid2,
        //            Email = sposti,
        //            Lahihoitaja = tyontekijat.Lahihoitaja,
        //            Sairaanhoitaja = tyontekijat.Sairaanhoitaja,
        //            LOP = tyontekijat.LOP,
        //            I_V = tyontekijat.I_V ,
        //            KIPU = tyontekijat.KIPU, 
        //            LAS = tyontekijat.LAS,
        //            GER = tyontekijat.GER,
        //            PSYK = tyontekijat.PSYK,
        //            ROKOTUS = tyontekijat.ROKOTUS,
        //            Tyokokemus = tyontekijat.Tyokokemus, // työkokemus INTiksi
        //            Aloitusaika = tyontekijat.Aloitusaika, 
        //            Lopetusaika = tyontekijat.Lopetusaika,
        //            AikaYhteensa = (tyontekijat.Lopetusaika - tyontekijat.Aloitusaika).ToString(),
        //            Luotu = aika3,
        //            Vanhustenhoito_kotihoito = tyontekijat.Vanhustenhoito_kotihoito,
        //            Mielenterveys = tyontekijat.Mielenterveys,
        //            Paivystys = tyontekijat.Paivystys,
        //            Lapset = tyontekijat.Lapset,
        //            Kehitysvammaiset = tyontekijat.Kehitysvammaiset,
        //            Hengityshalvaus = tyontekijat.Hengityshalvaus,
        //            Ensihoitaja_AMK = tyontekijat.Ensihoitaja_AMK,
        //            Terveydenhoitaja = tyontekijat.Terveydenhoitaja,
        //            Katilo = tyontekijat.Katilo,
        //            Osastotyoskentely = tyontekijat.Osastotyoskentely,
        //            Teho_osasto = tyontekijat.Teho_osasto,
        //            Ensihoito = tyontekijat.Ensihoito,
        //            Kommentit = tyontekijat.Kommentit,
        //            TarjousHinta = tyontekijat.TarjousHinta
        //        };
        //        db.tehtavia.Add(teh);
        //        db.SaveChanges();

        //        var id = (from t in db.tehtavia
        //                  where
        //                  t.Luotu == teh.Luotu
        //                  select
        //                  t.TyoID).FirstOrDefault();

        //        vastatarjoukset tarjoukset = new vastatarjoukset()
        //        {

        //            TyoID = id,
        //            YritysID = yid2,
        //            TarjousHinta = tyontekijat.TarjousHinta
        //        };
        //        db.vastatarjoukset.Add(tarjoukset);
        //        db.SaveChanges();

        //        var testi = (from t in db.tyontekijat2
        //                     where
        //                     (t.Lahihoitaja == tyontekijat.Lahihoitaja || t.Lahihoitaja == true) &&
        //                     (t.Sairaanhoitaja == tyontekijat.Sairaanhoitaja || t.Sairaanhoitaja == true) &&
        //                     (t.LOP == tyontekijat.LOP || t.LOP == true) &&
        //                     (t.I_V == tyontekijat.I_V || t.I_V == true) &&
        //                     (t.KIPU == tyontekijat.KIPU || t.KIPU == true) &&
        //                     (t.LAS == tyontekijat.LAS || t.LAS == true) &&
        //                     (t.GER == tyontekijat.GER || t.GER == true) &&
        //                     (t.PSYK == tyontekijat.PSYK || t.PSYK == true) &&
        //                     (t.ROKOTUS == tyontekijat.ROKOTUS || t.ROKOTUS == true) &&
        //                     (t.Tyokokemus >= tyontekijat.Tyokokemus) && 
        //                     (t.Vanhustenhoito_kotihoito == tyontekijat.Vanhustenhoito_kotihoito || t.Vanhustenhoito_kotihoito == true) &&
        //                     (t.Mielenterveys == tyontekijat.Mielenterveys || t.Mielenterveys == true) &&
        //                     (t.Paivystys == tyontekijat.Paivystys || t.Paivystys == true) &&
        //                     (t.Lapset == tyontekijat.Lapset || t.Lapset == true) &&
        //                     (t.Kehitysvammaiset == tyontekijat.Kehitysvammaiset || t.Kehitysvammaiset == true) &&
        //                     (t.Hengityshalvaus == tyontekijat.Hengityshalvaus || t.Hengityshalvaus == true) &&
        //                     (t.Ensihoitaja_AMK == tyontekijat.Ensihoitaja_AMK || t.Ensihoitaja_AMK == true) &&
        //                     (t.Terveydenhoitaja == tyontekijat.Terveydenhoitaja || t.Terveydenhoitaja == true) &&
        //                     (t.Katilo == tyontekijat.Katilo || t.Katilo == true) &&
        //                     (t.Osastotyoskentely == tyontekijat.Osastotyoskentely || t.Osastotyoskentely == true) &&
        //                     (t.Paivystys == tyontekijat.Paivystys || t.Paivystys == true) &&
        //                     (t.Teho_osasto == tyontekijat.Teho_osasto || t.Teho_osasto == true) &&
        //                     (t.Ensihoito == tyontekijat.Ensihoito || t.Ensihoito == true)
        //                     select
        //                     t.Email).ToArray();

        //        for (int i = 0; i < testi.Length; i++)
        //        {
        //            try
        //            {
        //                MailMessage mail = new MailMessage();
        //                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

        //                mail.From = new MailAddress("juho.hassi@gmail.com", "NurseBid");
        //                mail.To.Add(new MailAddress(testi[i]));
        //                mail.Subject = "Avoin työvuoro";
        //                mail.Body = "Avoin työvuoro." + "Aloitusaika: " + tyontekijat.Aloitusaika +
        //                    "Lopetusaika: " + tyontekijat.Lopetusaika +
        //                    "Yhteensä: " + (tyontekijat.Lopetusaika - tyontekijat.Aloitusaika) +
        //                    "Tarjous: " + tyontekijat.TarjousHinta +
        //                    "Hyväksy vuoro tai jätä vastatarjous täällä https://localhost:44344/";

        //                SmtpServer.Host = "smtp.gmail.com";
        //                SmtpServer.Port = 587;
        //                SmtpServer.EnableSsl = true;
        //                SmtpServer.UseDefaultCredentials = false;
        //                SmtpServer.Credentials = new System.Net.NetworkCredential("juho.hassi@gmail.com", "Hasa6666");

        //                SmtpServer.Send(mail);
        //            }
        //            catch (Exception)
        //            {

        //                throw;
        //            }

        //        }

        //        //TempData["Message"] = "Sahkoposti on lahetetty"; // Viesti alertoituu kun palataan Index sivulle
        //        return RedirectToAction("Index", "Home");
        //        //return RedirectToAction("Index");
        //    }

        //    ViewBag.HenkiloID = new SelectList(db.tyontekijat2, "HenkiloID", "Etunimi", tyontekijat.HenkiloID);
        //    return View(tyontekijat);
        //}



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult HyvaksyVuoro([Bind(Include = "TyoID,HenkiloID,YritysID,LoginID,Tehtava,TehtavaTaytettu,TehtavaValmis,Sijainti,Etunimi,Sukunimi,Puhelin,Email,EmailMessage,Aloitusaika,Lopetusaika,AikaYhteensa,Luotu,ViimeksiMuokattu,Poistettu,Aktiivinen,Lahihoitaja,Sairaanhoitaja,LOP,KIPU,I_V,LAS,GER,PSYK,ROKOTUS,Tyokokemus,Vanhustenhoito_kotihoito,Mielenterveys,Paivystys_Ensihoito,Lapset,Kehitysvammaiset,Hengityshalvaus,Ensihoitaja_AMK,Terveydenhoitaja,Katilo,Osastotyoskentely,Paivystys,Teho_osasto,Ensihoito,Kommentit")] tehtavia tehtavia)
        //{
        //    var id = Session["YritysID"];
        //    int? id2 = (int?)id;

        //    tehtavia teht = (from t in db.tehtavia
        //                     where tehtavia.TyoID == t.TyoID
        //                     select t).FirstOrDefault();
        //    teht.YritysID = id2;
        //    teht.TehtavaTaytettu = true;
        //    db.SaveChanges();
        //    try
        //    {
        //        int? jotain = (from t in db.tehtavia
        //                       where t.TyoID == tehtavia.TyoID
        //                       select t.HenkiloID).FirstOrDefault();

        //        var email = (from y in db.tyontekijat2
        //                     where y.HenkiloID == jotain
        //                     select y.Email).ToArray();

        //        //email.ToString();
        //        MailMessage mail = new MailMessage();
        //        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

        //        mail.From = new MailAddress("juho.hassi@gmail.com", "NurseBid");
        //        mail.To.Add(new MailAddress(email[0]));
        //        mail.Subject = "Työvuoro hyväksytty";
        //        mail.Body = "Työvuorosi on hyväksytty  https://localhost:44344/";

        //        SmtpServer.Host = "smtp.gmail.com";
        //        SmtpServer.Port = 587;
        //        SmtpServer.EnableSsl = true;
        //        SmtpServer.UseDefaultCredentials = false;
        //        SmtpServer.Credentials = new System.Net.NetworkCredential("juho.hassi@gmail.com", "Hasa6666");

        //        SmtpServer.Send(mail);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return RedirectToAction("YleisetAvoimetVuorot");
        //}
    }
}
