using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GesTenis.Models;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;
using GesTenis.tools;

namespace GesTenis.Controllers
{
    public class HomeController : BaseController
    {
        private gestenis_defEntities db = new gestenis_defEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            if ((object)Session["UserId"] == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", (bool)Session["IsAdmin"] == true ? "Admin" : "Socio");
            }
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            var db_user = (from socio in db.socios where socio.id == model.userId select socio).FirstOrDefault();

            if (db_user != null)
            {
                if (Tools.SHA256Encrypt(model.userPassword) == db_user.password)
                {
                    Session["UserId"] = model.userId;
                    Session["UserName"] = db_user.nombre;
                    Session["Name"] = db_user.nombre;
                    if (db_user.is_admin)
                    {
                        Session["IsAdmin"] = true;
                        string requestIp = Request.UserHostAddress.ToString();
                        string subject = "Login correcto en Gestenis";
                        string body = "<h1>Esto es un mensaje automático del sistema</h1>"
                            + "<p>Se ha logueado correctamente como ADMINISTRADOR: " + db_user.nombre + " " + db_user.apellidos + ".</p>"
                            + "<p>Su nombre de usuario es: " + db_user.id + "</p>"
                            + "<p>Se logueó desde la dirección IP: " + requestIp;
                        Tools.sendEmail(db_user, subject, body);
                        saveMessage("Se ha logueado correctamente como " + db_user.id);
                        return RedirectToAction("Index", "Admin");
                    }
                    Session["IsAdmin"] = false;
                    string subject2 = "Login correcto en Gestenis";
                    string body2 = "<h1>Esto es un mensaje automático del sistema</h1>"
                        + "<p>Se ha logueado correctamente como SOCIO: " + db_user.nombre + " " + db_user.apellidos + ".</p>"
                        + "<p>Su nombre de usuario es: " + db_user.id + "</p>";
                    Tools.sendEmail(db_user, subject2, body2);
                    return RedirectToAction("Index", "Socio");
                }
            }
            addError("Login incorrecto");
            saveErrors();
            return RedirectToAction("Login", "Home");
        }

        public ActionResult LogOff()
        {
            Session.Clear();
            Session.Abandon();
            return View();
        }

        public ActionResult Registro()
        {
            return View();
        }

        // POST: intermedio/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registro([Bind(Include = "password,nombre,apellidos,nif,email,telefono,direccion1,direccion2")] socios socio)
        {
            if (ModelState.IsValid)
            {
                socio.id = socio.nombre.Substring(0, 3) + socio.apellidos.Substring(0, 3) + socio.nif.Substring(0, 3);
                socio.password = Tools.SHA256Encrypt(socio.password);
                socio.is_admin = false;
                socio.f_alta = DateTime.Today;
                //socio.f_baja = null;
                //socio.reservas = null;
                db.socios.Add(socio);
                db.SaveChanges();
                string subject2 = "REGISTRO correcto en Gestenis";
                string body2 = "<h1>Esto es un mensaje automático del sistema</h1>"
                    + "<p>Se ha registrado correctamente como SOCIO: " + socio.nombre + " " + socio.apellidos + ".</p>"
                    + "<p>Su nombre de usuario es: " + socio.id + "</p>";
                Tools.sendEmail(socio, subject2, body2);
                return RedirectToAction("Login");
            }

            return RedirectToAction("Login", "Index");
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