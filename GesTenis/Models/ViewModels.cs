using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GesTenis.Models
{

    public class nuevaReservaSocioViewModel
    {

        [Display(Name = "Id de recurso")]
        [Required(ErrorMessage = "Hay que proporcionar el id de recurso")]
        public int id_rec { get; set; }

        [Display(Name = "Fecha reserva")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required()]
        public System.DateTime fecha { get; set; }

        [Display(Name = "Hora reserva")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:HH:00}", ApplyFormatInEditMode = true)]
        [Required()]
        public System.DateTime hora { get; set; }
    }


    public class nuevaReservaAdminViewModel
    {
        [Display(Name = "ID de socio")]
        [Required(ErrorMessage = "El ID es obligatorio")]
        public string id_soc { get; set; }

        [Display(Name = "Id de recurso")]
        [Required(ErrorMessage = "Hay que proporcionar el id de recurso")]
        public int id_rec { get; set; }

        [Display(Name = "Fecha reserva")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required()]
        public System.DateTime fecha { get; set; }

        [Display(Name = "Hora reserva")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:HH:00}", ApplyFormatInEditMode = true)]
        [Required()]
        public System.DateTime hora { get; set; }

        [Display(Name = "Pagado")]
        public bool pagado { get; set; }

        [Display(Name = "Precio")]
        public double precio { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        [Display(Name = "Usuario")]
        public string userId { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string userPassword { get; set; }
    }

    //public class RegistroViewModel
    //{
    //    [Display(Name="ID de usuario")]
    //    [Required()]
    //    public string id { get; set; }

    //    [DataType(DataType.Password)]
    //    [Display(Name="Contraseña")]
    //    [Required()]
    //    public string password { get; set; }

    //    [Display(Name="Nombre")]
    //    [Required()]
    //    public string nombre { get; set; }

    //    [Display(Name = "Apellidos")]
    //    [Required()]
    //    public string apellidos { get; set; }

    //    [Display(Name = "DNI o NIE")]
    //    [Required()]
    //    public string nif { get; set; }

    //    [DataType(DataType.EmailAddress)]
    //    [Display(Name = "E-mail")]
    //    [Required()]
    //    public string email { get; set; }

    //    [Display(Name = "Teléfono")]
    //    [DataType(DataType.PhoneNumber)]
    //    public string telefono { get; set; }

    //    [Display(Name = "Calle y número")]
    //    public string direccion1 { get; set; }

    //    [Display(Name = "CP y Localidad")]
    //    public string direccion2 { get; set; }
    //}

    public class RecuperarContrasenaViewModel
    {
        [Display(Name = "Introduce tu email")]
        [EmailAddress(ErrorMessage = "Introduce un email válido")]
        [Required(ErrorMessage = "El email es obligatorio")]
        public string email { get; set; }
    }

    public class CambiarContrasenaViewModel
    {
        [Display(Name = "Contraseña actual")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string oldPass { get; set; }

        [Display(Name = "Contraseña nueva")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string newPass { get; set; }


        [Display(Name = "Repita contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string verifyNewPass { get; set; }
    }


}
