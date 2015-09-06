using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GesTenis.Controllers
{
    [SetupErrors]
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Salva el mensaje en la variable de sesion
        /// </summary>
        /// <param name="message">Mensaje a guardar</param>
        public void saveMessage(string message)
        {
            HttpContext.Session["message"] = message;
        }
       
        /// <summary>
        /// Devuelve en mensaje que se encuentra guardado en la variable de sesion (si existe)
        /// </summary>
        /// <returns>string con el mensaje; si no existe, devuelve null</returns>
        public string loadMessage()
        {
            if (HttpContext.Session["message"] != null)
            {
                string message = HttpContext.Session["message"].ToString();
                HttpContext.Session.Remove("message");
                return message;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Lista de errores
        /// </summary>
        public List<string> errors = null;
        
        /// <summary>
        /// Añade error a la lista de errores, si no hay errores, inicializa la lista de errores y guarda el error.
        /// </summary>
        /// <param name="message">error a guardar</param>
        public void addError(string message)
        {
            if (errors == null)
            {
                errors = new List<string>();
            }
            errors.Add(message);
        }
        
        /// <summary>
        /// Guarda la lista de errores en la variable de sesion como una lista html
        /// </summary>
        public void saveErrors()
        {
            if (errors != null)
            {
                string errorsMessage = "";
                foreach (var error in errors)
                {
                    errorsMessage += "<li>" + error + "</li>";
                }
                Session["errors"] = errorsMessage;
            }

        }
        
        /// <summary>
        /// Devuelve los errores que se encuentran en la variable de sesion. Borra los errores en la variable de sesion.
        /// </summary>
        /// <returns>string con los errores si existen; sino, devuelve null.</returns>
        public string loadErrors()
        {
            if (HttpContext.Session["errors"] != null)
            {
                string errorsMessage = HttpContext.Session["errors"].ToString();
                HttpContext.Session.Remove("errors");
                return errorsMessage;
            }
            else
            {
                return null;
            }
        }
    }

    //Setup Errors and Messages
    public class SetupErrorsAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Se sobreescribe este metodo que se ejecuta antes de la llamada a cada metodo de action 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            BaseController mainController = ((BaseController)filterContext.Controller);
            mainController.ViewData["errors"] = mainController.loadErrors();
            mainController.ViewData["message"] = mainController.loadMessage();
        }

    }
}