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
        /// <summary>
        /// Instancia del contexto que referencia a la BBDD
        /// </summary>
        private gestenis_defEntities db = new gestenis_defEntities();

        /// <summary>
        /// Instancia del objeto PDFNetLoader necesario para la generacion del pdf en la factura
        /// </summary>
        private static pdftron.PDFNetLoader loader = pdftron.PDFNetLoader.Instance();


        /// <summary>
        /// Devuelve la pagina inicial de la aplicacion
        /// </summary>
        /// <returns>Vista Home/Index</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Devuelve la pagina de login a la aplicacion
        /// </summary>
        /// <returns>Vista Home/login</returns>
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

        /// <summary>
        /// Metodo POST para Home/login. Comprueba que el usuario existe, la contraseña es correcta.
        /// Inicializa las variables de sesion necesarias con los datos del socio.
        /// Envia al socio e-mail de confirmacion de login
        /// </summary>
        /// <param name="model">LoginViewModel para comprobar usuario y contraseña.</param>
        /// <returns>Vista de pagina de inicio de "socio" o "admin" dependiendo de si el socio es admin o no.
        /// Si usuario o contraseña no validos, devuelve la vista /Home/login
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            //var db_user = (from socio in db.socios where socio.id == model.userId select socio).FirstOrDefault();
            try
            {
                var db_user2 = db.socios.Find(model.userId);
            }
            catch (Exception)
            {
                addError("Problema conectando a la BBDD.");
                saveErrors();
                return RedirectToAction("login", "home");
                throw;
            }

            var db_user = db.socios.Find(model.userId);
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

        /// <summary>
        /// Abandona la sesion. borra las variables de sesion y nos redirige a la pagina de login de la aplicacion.
        /// </summary>
        /// <returns>Vista de login de la aplicacion /Home/login</returns>
        public ActionResult LogOff()
        {
            Session.Clear();
            Session.Abandon();
            saveMessage("Sesión cerrada con exito");
            return RedirectToAction("Login", "Home");
        }

        /// <summary>
        /// Redirige a la pagina de registro en la aplicacion.
        /// </summary>
        /// <returns>Vista de registro de la aplicacion</returns>
        public ActionResult Registro()
        {
            return View();
        }

        /// <summary>
        /// Metodo POST para Registro. Si los datos del formulario son correctos, guarda el socio en la BBDD y envía email de confirmacion al socio.
        /// </summary>
        /// <param name="socio">socios model. Modelo de socios.</param>
        /// <returns>Si registro correcto, a la pagina de login /home/login.
        /// Si registro no correcto, a la pagina de registro</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registro([Bind(Include = "id, password,nombre,apellidos,nif,email,telefono,direccion1,direccion2")] socios socio)
        {
            if (ModelState.IsValid)
            {
                //socio.id = socio.nombre.Substring(0, 3) + socio.apellidos.Substring(0, 3) + socio.nif.Substring(0, 3);
                socios db_posible_socio = db.socios.Find(socio.id);
                if (db_posible_socio !=null)
                {
                    addError("Ya existe un socio registrado con este ID. Por favor, elija otro");
                    saveErrors();
                    return RedirectToAction("Registro", "Home");
                }
                
                socio.password = Tools.SHA256Encrypt(socio.password);
                socio.is_admin = false;
                socio.f_alta = DateTime.Today;
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

            return RedirectToAction("Registro", "Home");
        }

        /// <summary>
        /// Devuelve la pagina de recuperar contraseña
        /// </summary>
        /// <returns>Vista /home/recuperarcontrasena</returns>
        public ActionResult RecuperarContrasena()
        {
            return View();
        }

        /// <summary>
        /// Metodo POST para /home/recuperarcontrasena. Recibe el email como parametro y si existe un socio con ese email, envia al mismo una contraseña aleatoria nueva
        /// </summary>
        /// <param name="model">email al que mandar la contraseña nueva</param>
        /// <returns>si cambio contraseña correcto, a pagina de login. En caso contrario a la pagina de recuperar contraseña</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarContrasena([Bind(Include = "email")] RecuperarContrasenaViewModel model)
        {
            socios db_socio = (from x in db.socios where x.email == model.email select x).FirstOrDefault();
            if (db_socio == null)
            {
                addError("No existe usuario la direccion e-mail especificada.");
                saveErrors();
                return RedirectToAction("RecuperarContrasena", "Home");
            }
            else
            {
                string pass = Tools.randomPassword(6);
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
                string message = "Contraseña nueva enviada a su email: " + db_socio.email;
                saveMessage(message);
                return RedirectToAction("Login", "Home");
            }
        }

        /// <summary>
        /// Devuelve la vista con las tarifas del club
        /// </summary>
        /// <returns>Vista /home/tarifas</returns>
        public ActionResult Tarifas()
        {
            return View();
        }

        /// <summary>
        /// Devuelve la vista con el contacto del club
        /// </summary>
        /// <returns>Vista /home/contacto</returns>
        public ActionResult Contacto()
        {
            return View();
        }

        public ActionResult Error()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}