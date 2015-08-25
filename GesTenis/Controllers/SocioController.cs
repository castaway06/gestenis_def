using GesTenis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GesTenis.tools;
using System.Data.Entity;

namespace GesTenis.Controllers
{
    public class SocioController : BaseController
    {

        private gestenis_defEntities db = new gestenis_defEntities();
        // GET: Socio
        public ActionResult Index()
        {
            if (isSocio()) return View();
            else return RedirectToAction("Index", isAdmin() ? "Admin" : "Home");
        }

        public ActionResult CambiarContrasena()
        {
            if (isSocio()) return View();
            else return RedirectToAction("Index", isAdmin() ? "Admin" : "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarContrasena(CambiarContrasenaViewModel model)
        {
            if (model.newPass != model.verifyNewPass)
            {
                addError("Las contraseñas nuevas no coinciden");
                saveErrors();
                return RedirectToAction("CambiarContrasena", "Socio");
            }

            var id = (string)Session["UserID"];

            var db_socio = (from x in db.socios where x.id == id select x).FirstOrDefault();
            
            if (Tools.SHA256Encrypt(model.oldPass) != db_socio.password)
            {
                addError("La contraseña actual no es correcta");
                saveErrors();
                return RedirectToAction("CambiarContrasena", "Socio");
            }
            else
            {
                db_socio.password = Tools.SHA256Encrypt(model.newPass);
                db.Entry(db_socio).State = EntityState.Modified;
                db.SaveChanges();
                saveMessage("La contraseña se ha cambiado correctamente");
                return RedirectToAction("Index", "Socio");
            }
        }

        public ActionResult MisDatos()
        {
            if (isSocio())
            {
                string id = (string)Session["UserID"];
                var db_socio = db.socios.Where(x => x.id == id).First();
                return View(db_socio);
            }
            else return RedirectToAction("Index", isAdmin() ? "Admin" : "Home");
        }

        public ActionResult ModificarDatos()
        {
            if (isSocio())
            {
                string id = (string)Session["UserID"];
                var db_socio = db.socios.Where(x => x.id == id).First();
                return View(db_socio);
            }
            else return RedirectToAction("Index", isAdmin() ? "Admin" : "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificarDatos([Bind(Include = "id,password,is_admin,nombre,apellidos,nif,email,telefono,direccion1,direccion2,f_alta,f_baja")]socios socio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(socio).State = EntityState.Modified;
                db.SaveChanges();
                string subject = "Datos de usuario modificados en Gestenis";
                string body = "<h1>Esto es un mensaje automático del sistema</h1>"
                    + "<p>" + socio.nombre + ", ha cambiado correctamente sus datos en GesTenis.</p>"
                    + "<p>Le recordamos su nombre de usuario: " + socio.id + "</p>";
                Tools.sendEmail(socio, subject, body);
                saveMessage("Sus datos han sido modificados correctamente");
                return RedirectToAction("MisDatos", "Socio");
            }

            return RedirectToAction("ModificarDatos", "Socio", socio);
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