using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GesTenis.Controllers
{
    public class AdminController : BaseController
    {
        private gestenis_defEntities db = new gestenis_defEntities();

        // GET: Admin
        public ActionResult Index()
        {
            if (isAdmin())
            {
                return View();
            }
            else
            {
                if (isSocio())
                {
                    return RedirectToAction("Index", "Socio");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult Socios()
        {
            if (isAdmin())
            {
                //Codigo que se ejecuta en caso de Admin
                return View(db.socios.ToList());
            }
            else
            {
                //Codigo que se ejecuta en caso de socio o no auth
                if (isSocio())
                {
                    return RedirectToAction("Index", "Socio");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
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
                socios socios = db.socios.Find(id);
                if (socios == null)
                {
                    addError("El socio seleccionado no existe");
                    saveErrors();
                    return RedirectToAction("Socios", "Admin");
                }
                return View(socios);
            }
            else
            {
                //Codigo que se ejecuta en caso de socio o no auth
                if (isSocio())
                {
                    return RedirectToAction("Index", "Socio");
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
