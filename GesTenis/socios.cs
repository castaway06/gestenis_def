//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GesTenis
{
    using System;
    using System.Collections.Generic;
    
    public partial class socios
    {
        public socios()
        {
            this.reservas = new HashSet<reservas>();
        }
    
        public string id { get; set; }
        public string password { get; set; }
        public bool is_admin { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string nif { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }
        public string direccion1 { get; set; }
        public string direccion2 { get; set; }
        public System.DateTime f_alta { get; set; }
        public Nullable<System.DateTime> f_baja { get; set; }
    
        public virtual ICollection<reservas> reservas { get; set; }
    }
}