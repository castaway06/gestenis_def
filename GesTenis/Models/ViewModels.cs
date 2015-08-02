using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GesTenis.Models
{
    
    public class LoginViewModel
    {
        [Required]
        [Display(Name="Usuario")]
        public string userId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string userPassword { get; set; }

    }

}
