using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GesTenis.Models
{

    public class LoginViewModel
    {
        [Required()]
        [Display(Name = "Usuario")]
        public string userId { get; set; }

        [Required()]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string userPassword { get; set; }
    }

    public class RegistroViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name="Contraseña")]
        [Required()]
        public string password { get; set; }
        
        [Display(Name="Nombre")]
        [Required()]
        public string nombre { get; set; }
        
        [Display(Name = "Apellidos")]
        [Required()]
        public string apellidos { get; set; }
        
        [Display(Name = "DNI o NIE")]
        [Required()]
        public string nif { get; set; }
        
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail")]
        [Required()]
        public string email { get; set; }
        
        [Display(Name = "Teléfono")]
        [DataType(DataType.PhoneNumber)]
        public string telefono { get; set; }
        
        [Display(Name = "Calle y número")]
        public string direccion1 { get; set; }
        
        [Display(Name = "CP y Localidad")]
        public string direccion2 { get; set; }
    }

}
