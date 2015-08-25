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
using System.Data.Entity;

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
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            var db_user = (from socio in db.socios where socio.id == model.userId select socio).FirstOrDefault();

            if (db_user != null)
            {
                if (Tools.SHA256Encrypt(model.userPassword) == db_user.password)
                {
                    Session["UserId"] = db_user.id;
                    Session["UserName"] = db_user.nombre;
                    Session["Name"] = db_user.nombre;
                    Session["IsAdmin"] = db_user.is_admin ? true : false;

                    string requestIp = Request.UserHostAddress.ToString();
                    string subject = "Login correcto en Gestenis";
                    string body = "<h1>Esto es un mensaje automático del sistema</h1>"
                        + "<p>Se ha logueado correctamente como " + ((bool)Session["IsAdmin"] ? "ADMINISTRADOR" : "SOCIO") + ": " + db_user.nombre + " " + db_user.apellidos + ".</p>"
                        + "<p>Su nombre de usuario es: " + db_user.id + "</p>"
                        + "<p>Se logueó desde la dirección IP: " + requestIp;
                    Tools.sendEmail(db_user, subject, body);
                    saveMessage("Se ha logueado correctamente como " + db_user.id);
                    return RedirectToAction("Index", (bool)Session["IsAdmin"] ? "Admin" : "Socio");
                }
            }
            addError("Usuario o contraseña incorrectos");
            saveErrors();
            return RedirectToAction("Login", "Home");
        }

        public ActionResult LogOff()
        {
            Session.Clear();
            Session.Abandon();
            saveMessage("Sesión cerrada con exito");
            return View();
        }

        public ActionResult Registro()
        {
            return View();
        }

        // POST: Home/registro
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
                string subject = "REGISTRO correcto en Gestenis";
                string body = "<h1>Esto es un mensaje automático del sistema</h1>"
                    + "<p>Se ha registrado correctamente como SOCIO: " + socio.nombre + " " + socio.apellidos + ".</p>"
                    + "<p>Su nombre de usuario es: " + socio.id + "</p>";
                Tools.sendEmail(socio, subject, body);
                saveMessage("Se ha registrado correctamente en GesTenis");
                return RedirectToAction("Login", "Home");
            }

            return RedirectToAction("Login", "Index");
        }

        public ActionResult RecuperarContrasena()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarContrasena([Bind(Include = "email")] RecuperarContrasenaViewModel model)
        {
            socios db_socio = (from x in db.socios where x.email == model.email select x).FirstOrDefault();
            if (db_socio == null)
            {
                addError("No existe usuario con esta dirección de e-mail.");
                saveErrors();
                return RedirectToAction("RecuperarContrasena", "Home");
            }
            else
            {
                string pass = Tools.randomPassword(6);
                Debug.WriteLine(pass);
                db_socio.password = Tools.SHA256Encrypt(pass);
                db.Entry(db_socio).State = EntityState.Modified;
                db.SaveChanges();
                // Mandar email con nueva contraseña
                string subject = "Contraseña recuperada en Gestenis";
                string body = "<h1>Esto es un mensaje automático del sistema</h1>"
                    + "<p>" + db_socio.nombre + ", ha restaurado correctamente su contraseña en Gestenis.</p>"
                    + "<p>Su nombre de usuario es: " + db_socio.id + "</p>"
                    + "<p>Su nueva contraseña es: " + pass + "</p>"
                    + "<p>Le recomendamos que cambie esta contraseña por una nueva, que recuerde más fácilmente, desde su área de usuario.</p>";
                Tools.sendEmail(db_socio, subject, body);
                string message = "Contraseña nueva enviada a su email: " + "";
                saveMessage(message);
                return RedirectToAction("Login", "Home");
            }
        }


        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}