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
            if (Session["SessionUserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("NoAcceso", "Socio");
            }
        }

        public ActionResult NoAcceso()
        {
            return View();
        }

    }
}