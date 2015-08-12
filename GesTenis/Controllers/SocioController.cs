using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GesTenis.Controllers
{
    public class SocioController : Controller
    {

        // GET: Socio
        public ActionResult Index()
        {
            if (isSocio())
            {
                return View();
            }
            else
            {
                if (isAdmin())
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult NoAcceso()
        {
            return View();
        }

        public bool isAdmin()
        {
            if ((object)Session["IsAdmin"] != null)
            {
                if ((bool)Session["IsAdmin"].Equals(true))
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        private bool isSocio()
        {
            if ((object)Session["UserId"] != null)
            {
                if ((bool)Session["IsAdmin"].Equals(false))
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }


    }
}