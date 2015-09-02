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

    public partial class recursos
    {
        public recursos()
        {
            this.reservas = new HashSet<reservas>();
        }

        [Display(Name = "Id de recurso")]
        public int id { get; set; }

        [Display(Name = "Tipo del recurso")]
        [Required(ErrorMessage = "El campo tipo es obligatorio")]
        public string tipo { get; set; }

        [Display(Name = "Fecha de alta")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime f_alta { get; set; }

        [Display(Name = "Fecha de baja")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> f_baja { get; set; }

        [Display(Name = "Nombre del recurso")]
        [Required(ErrorMessage = "Nombre del recurso obligatorio")]
        public string nombre_rec { get; set; }

        [Display(Name = "Superficie")]
        [Required(ErrorMessage = "Superficie obligatoria")]
        public string superficie { get; set; }

        [Display(Name = "Disponible")]
        [Required(ErrorMessage="Campo obligatorio")]
        public bool disponible { get; set; }

        public virtual ICollection<reservas> reservas { get; set; }
    }
}
