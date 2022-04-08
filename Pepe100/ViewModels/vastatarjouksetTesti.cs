using Pepe100.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pepe100.ViewModels
{
    public class vastatarjouksetTesti
    {
        public IEnumerable<vastatarjoukset> vastaTesti { get; set; }
        public vastatarjoukset uusivastatarjoukset { get; set; }
        public int VastaTarjousID { get; set; }
        public Nullable<int> TyoID { get; set; }
        public Nullable<int> YritysID { get; set; }
        public Nullable<int> HenkiloID { get; set; }
        public Nullable<int> TarjousHinta { get; set; }
        public Nullable<int> VastaTarjousHinta { get; set; }
        public Nullable<int> Prosentti { get; set; }
        public Nullable<int> ToteutunutHinta { get; set; }
    }
}