//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Pepe100.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class loginyritys
    {
        public int LoginIDYritys { get; set; }
        public Nullable<int> YritysID { get; set; }
        public string UserNameYritys { get; set; }
        public string PasswordYritys { get; set; }
        public string LoginErrorMessageYritys { get; set; }
    
        public virtual yritys yritys { get; set; }
    }
}
