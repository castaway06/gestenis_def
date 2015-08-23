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
        public void saveMessage(string message)
        {
            HttpContext.Session["message"] = message;
        }
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

        public List<string> errors = null;
        public void addError(string message)
        {
            if (errors == null)
            {
                errors = new List<string>();
            }
            errors.Add(message);
        }
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
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            BaseController mainController = ((BaseController)filterContext.Controller);
            mainController.ViewData["errors"] = mainController.loadErrors();
            mainController.ViewData["message"] = mainController.loadMessage();
        }

        //public override void OnResultExecuting(ResultExecutingContext filterContext)
        //{
        //    BaseController mainController = ((BaseController)filterContext.Controller);
        //    mainController.ViewData["errors"] = mainController.loadErrors();
        //    mainController.ViewData["message"] = mainController.loadMessage();
        //}
    }
}