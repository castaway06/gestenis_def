using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GesTenis.Models
{

    /// <summary>
    /// ViewModel para nueva reserva desde area de socio
    /// </summary>
    public class nuevaReservaSocioViewModel
    {
        /// <summary>
        /// Contiene el id del recurso a reservar
        /// </summary>
        [Display(Name = "Id de recurso")]
        [Required(ErrorMessage = "Hay que proporcionar el id de recurso")]
        public int id_rec { get; set; }

        /// <summary>
        /// Contiene la fecha de la reserva
        /// </summary>
        [Display(Name = "Fecha reserva")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required()]
        public System.DateTime fecha { get; set; }

        /// <summary>
        /// Contiene la hora de la reserva
        /// </summary>
        [Display(Name = "Hora reserva")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:HH:00}", ApplyFormatInEditMode = true)]
        [Required()]
        public System.DateTime hora { get; set; }
    }


    /// <summary>
    /// ViewMOdel para nueva reserva hecha por el administrador
    /// </summary>
    public class nuevaReservaAdminViewModel
    {
        /// <summary>
        /// Contiene el id del socio para el cual realizar la reserva
        /// </summary>
        [Display(Name = "ID de socio")]
        [Required(ErrorMessage = "El ID es obligatorio")]
        public string id_soc { get; set; }

        /// <summary>
        /// Contiene el id del recurso a reservar
        /// </summary>
        [Display(Name = "Id de recurso")]
        [Required(ErrorMessage = "Hay que proporcionar el id de recurso")]
        public int id_rec { get; set; }

        /// <summary>
        /// Contiene la fecha de la reserva
        /// </summary>
        [Display(Name = "Fecha reserva")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required()]
        public System.DateTime fecha { get; set; }

        /// <summary>
        /// Contiene la hora de la reserva
        /// </summary>
        [Display(Name = "Hora reserva")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:HH:00}", ApplyFormatInEditMode = true)]
        [Required()]
        public System.DateTime hora { get; set; }

        /// <summary>
        /// Bool que contiene si el socio ha pagado o no la reserva
        /// </summary>
        [Display(Name = "Pagado")]
        public bool pagado { get; set; }

    }

    /// <summary>
    /// ViewModel para login en la aplicacion
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Contiene el id del usuario a loguear
        /// </summary>
        [Required(ErrorMessage = "El usuario es obligatorio")]
        [Display(Name = "Usuario")]
        public string userId { get; set; }

        /// <summary>
        /// Contiene la contraseña del usuario a loguear
        /// </summary>
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string userPassword { get; set; }
    }

    /// <summary>
    /// ViewModel para recuperar la contraseña
    /// </summary>
    public class RecuperarContrasenaViewModel
    {
        /// <summary>
        /// Contiene el email sobre el que recuperar la contraseña
        /// </summary>
        [Display(Name = "Introduce tu email")]
        [EmailAddress(ErrorMessage = "Introduce un email válido")]
        [Required(ErrorMessage = "El email es obligatorio")]
        public string email { get; set; }
    }

    /// <summary>
    /// ViewModel para cambiar la contraseña
    /// </summary>
    public class CambiarContrasenaViewModel
    {
        /// <summary>
        /// Contiene la contraseña vieja
        /// </summary>
        [Display(Name = "Contraseña actual")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string oldPass { get; set; }

        /// <summary>
        /// Contiene la contraseña nueva
        /// </summary>
        [Display(Name = "Contraseña nueva")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string newPass { get; set; }

        /// <summary>
        /// Contiene la contraseña nueva, repetida
        /// </summary>
        [Display(Name = "Repita contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string verifyNewPass { get; set; }
    }


}
