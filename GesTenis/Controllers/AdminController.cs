using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GesTenis.tools;
using GesTenis.Models;
using System.Data.Entity;

namespace GesTenis.Controllers
{
    public class AdminController : BaseController
    {
        private gestenis_defEntities db = new gestenis_defEntities();

        // GET: Admin
        public ActionResult Index()
        {
            if (isAdmin()) return View();
            else return RedirectToAction("Index", isSocio() ? "Socio" : "Home");
        }

        public ActionResult ListadoDeSocios()
        {
            if (isAdmin()) return View();
            else return RedirectToAction("Index", isSocio() ? "Socio" : "Home");
        }

        [HttpPost]
        public ActionResult LoadSocios(string nif)
        {
            List<GesTenis.socios> ret = null;
            if (nif == null || nif == "")
            {
                ret = db.socios.ToList();
            }
            else
            {
                ret = (from socio in db.socios where socio.nif == nif select socio).ToList();

            }
            return PartialView(ret);
        }

        public ActionResult Socios()
        {
            if (isAdmin())
                //Codigo que se ejecuta en caso de Admin
                return View(db.socios.ToList());
            else
                // Codigo que se ejecuta en caso de socio o no auth
                return RedirectToAction("Index", isSocio() ? "Socio" : "Home");
        }

        // GET: Admin/EditarSocio/id
        public ActionResult EditarSocio(string id)
        {
            if (isAdmin())
            {
                //Codigo que se ejecuta en caso de Admin
                if (id == null)
                {
                    addError("Ha de seleccionar un socio para editar");
                    saveErrors();
                    return RedirectToAction("Socios", "Admin");
                }
                socios socio = db.socios.Find(id);
                if (socio == null)
                {
                    addError("El socio seleccionado no existe");
                    saveErrors();
                    return RedirectToAction("Socios", "Admin");
                }
                return View(socio);
            }
            else
                //Codigo que se ejecuta en caso de socio o no auth
                return RedirectToAction("Index", isSocio() ? "Socio" : "Home");
        }

        public ActionResult CambiarContrasena()
        {
            if (isAdmin()) return View();
            else return RedirectToAction("Index", isSocio() ? "Socio" : "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarContrasena(CambiarContrasenaViewModel model)
        {
            if (model.newPass != model.verifyNewPass)
            {
                addError("Las contraseñas nuevas no coinciden");
                saveErrors();
                return RedirectToAction("CambiarContrasena", "Admin");
            }

            var id = (string)Session["UserID"];

            var db_socio = (from x in db.socios where x.id == id select x).FirstOrDefault();

            if (Tools.SHA256Encrypt(model.oldPass) != db_socio.password)
            {
                addError("La contraseña actual no es correcta");
                saveErrors();
                return RedirectToAction("CambiarContrasena", "Admin");
            }
            else
            {
                db_socio.password = Tools.SHA256Encrypt(model.newPass);
                db.Entry(db_socio).State = EntityState.Modified;
                db.SaveChanges();
                saveMessage("La contraseña se ha cambiado correctamente");
                return RedirectToAction("Index", "Admin");
            }
        }

        public ActionResult MisDatos()
        {
            if (isAdmin())
            {
                string id = (string)Session["UserID"];
                var db_socio = db.socios.Where(x => x.id == id).First();
                return View(db_socio);
            }
            else return RedirectToAction("Index", isSocio() ? "Socio" : "Home");

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
