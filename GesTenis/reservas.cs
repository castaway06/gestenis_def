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
    using System.ComponentModel.DataAnnotations;
    
    public partial class reservas
    {
        [Display(Name = "Id reserva")]
        public int id { get; set; }

        [Display(Name = "Fecha reserva")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required()]
        public System.DateTime fecha { get; set; }

        [Display(Name = "Hora reserva")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:HH}", ApplyFormatInEditMode = true)]
        [Required()]
        public System.DateTime hora { get; set; }

        [Display(Name = "Pagado")]
        public bool pagado { get; set; }
        
        [Display(Name = "Precio")]
        public double precio { get; set; }
    
        public virtual facturas facturas { get; set; }
        public virtual recursos recursos { get; set; }
        public virtual socios socios { get; set; }
    }
}
