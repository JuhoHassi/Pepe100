using Pepe100.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pepe100.ViewModels
{
    public partial class TyontekijatViewModel
    {
        public TyontekijatViewModel()
        {
            this.Login = new HashSet<login>();
        }
        public Nullable<int> TyoID { get; set; }
        public Nullable<int> VastaTarjousHinta { get; set; }

        public int HenkiloID { get; set; }
        public string Etunimi { get; set; }
        public string Sukunimi { get; set; }
        public string Puhelin { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> Luotu { get; set; }
        public Nullable<System.DateTime> ViimeksiMuokattu { get; set; }
        public Nullable<System.DateTime> Poistettu { get; set; }
        public Nullable<bool> Aktiivinen { get; set; }
        public string EmailMessage { get; set; }
        public Nullable<int> TarjousHinta { get; set; }
        public Nullable<int> ToteutunutHinta { get; set; }
        public Nullable<int> LoginID { get; set; }
        public Nullable<bool> Lahihoitaja { get; set; }
        public Nullable<bool> Sairaanhoitaja { get; set; }
        public Nullable<bool> LOP { get; set; }
        public Nullable<bool> I_V { get; set; }
        public Nullable<bool> LAS { get; set; }
        public Nullable<bool> GER { get; set; }
        public Nullable<bool> PSYK { get; set; }
        public Nullable<bool> ROKOTUS { get; set; }
        public int? Tyokokemus { get; set; }
        public Nullable<bool> Vanhustenhoito_kotihoito { get; set; }
        public Nullable<bool> Mielenterveys { get; set; }
        public Nullable<bool> Paivystys_Ensihoito { get; set; }
        public Nullable<bool> Lapset { get; set; }
        public Nullable<bool> Kehitysvammaiset { get; set; }
        public Nullable<bool> Hengityshalvaus { get; set; }
        public Nullable<bool> Ensihoitaja_AMK { get; set; }
        public Nullable<bool> Terveydenhoitaja { get; set; }
        public Nullable<bool> Katilo { get; set; }
        public Nullable<bool> Osastotyoskentely { get; set; }
        public Nullable<bool> Paivystys { get; set; }
        public Nullable<bool> Teho_osasto { get; set; }
        public Nullable<bool> Ensihoito { get; set; }
        public string Kommentit { get; set; }
        public byte[] Kuva { get; set; }
        public Nullable<bool> KIPU { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<login> Login { get; set; }
    }
}