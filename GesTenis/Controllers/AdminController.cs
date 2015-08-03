using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GesTenis.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            if ((object)Session["IsAdmin"] != null)
            {
                if ((bool)Session["IsAdmin"].Equals(true))
                {
                    return View();
                }
                return RedirectToAction("NoAcceso", "Admin");
            }
            else
            {
                return RedirectToAction("NoAcceso","Admin");
            }
            
        }

        public ActionResult NoAcceso()
        {
            return View();
        }
    }
}
