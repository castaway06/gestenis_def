using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GesTenis.Models;

namespace GesTenis.Controllers
{
    public class HomeController : Controller
    {
        private gestenis_defEntities db = new gestenis_defEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model)
        {
            var db_user = (from socio in db.socios where socio.id == model.userId select socio).FirstOrDefault();

            if (db_user !=null)
            {
                string db_password = db_user.password;
                if (model.userPassword == db_password)
                {
                    bool db_user_is_admin = db_user.is_admin;
                    if (db_user_is_admin)
                    {
                        Session["SessionUserId"] = model.userId;
                        Session["SessionUserName"] = db_user.nombre;
                        Session["IsAdmin"] = true;
                        return RedirectToAction("Index", "Admin");
                    }
                    Session["SessionUserId"] = model.userId;
                    Session["SessionUserName"] = db_user.nombre;
                    Session["IsAdmin"] = false;
                    return RedirectToAction("Index", "Socio");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOff()
        {
            Session.Clear();
            Session.Abandon();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}