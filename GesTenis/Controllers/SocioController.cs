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
            if (isSocio())
            {
                string id = Session["UserID"].ToString();
                ViewBag.n_reservas = db.reservas.Where(x => x.socios.id == id).Count().ToString();
                return View();
            }
            else return RedirectToAction("Index", isAdmin() ? "Admin" : "Home");
        }

        #region RESERVAS SOCIO
        //----------------------------------------------------------------

        public ActionResult MisReservas()
        {
            if (isSocio())
            {
                string id_soc = Session["UserID"].ToString();

                // todas mis reservas ordenadas por fecha-hora
                var reservas = db.reservas.Where(x => x.socios.id == id_soc).OrderBy(y => y.hora).ToList();
                return View(reservas);
            }
            else return RedirectToAction("Index", isAdmin() ? "Admin" : "Home");
        }


        public ActionResult NuevaReserva()
        {
            if (isSocio())
            {
                return View();
            }
            else return RedirectToAction("Index", isAdmin() ? "Admin" : "Home");
        }

        // POST: Socio/NuevaReserva
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevaReserva([Bind(Include = "id_rec,fecha,hora")] nuevaReservaSocioViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Recuperamos el socio
                string id_soc = Session["UserID"].ToString();
                socios db_socio = db.socios.Find(id_soc);

                DateTime fecha_baja = db_socio.f_baja ?? default(DateTime);
                if (DateTime.Compare(fecha_baja, DateTime.Today) < 0)
                {
                    addError("No puede realizar la reserva, su cuota ha expirado o aún no ha pagado su primera cuota de socio");
                }

                // Recuperamos el recurso y comprobamos que existe y esta disponible
                recursos db_recurso = db.recursos.Find(model.id_rec);
                if (db_recurso == null)
                {
                    addError("El recurso no existe");
                }
                if (db_recurso.disponible == false)
                {
                    addError("El recurso seleccionado no está disponible");
                }
                reservas reserva = new reservas();
                reserva.fecha = model.fecha;
                reserva.hora = new DateTime(reserva.fecha.Year, reserva.fecha.Month, reserva.fecha.Day, model.hora.Hour, model.hora.Minute, model.hora.Second);
                reserva.pagado = false;
                reserva.precio = 3;

                // Comprobacion que la fecha y/u hora no son anteriores a hoy
                if (DateTime.Compare(reserva.fecha, DateTime.Today) < 0)
                {
                    addError("La fecha de la reserva no puede ser anterior a hoy");
                }
                if (DateTime.Compare(reserva.hora, DateTime.Now) < 0)
                {
                    addError("La hora de la reserva no puede ser anterior a ahora");
                }
                
                // Comprobacion que el socio no ha realizado reserva la fecha seleccionada
                reservas res_soc = db.reservas.Where(x => x.socios.id == id_soc && x.fecha == model.fecha).FirstOrDefault();
                if (res_soc != null)
                {
                    addError("Ya has hecho reserva en esta fecha (solo se te permite una reserva por dia)");
                }

                // Comprobación que el recurso se encuentra libre
                reservas existe_reserva = db.reservas.Where(x => x.recursos.id == model.id_rec && x.hora == reserva.hora).FirstOrDefault();
                if (existe_reserva != null)
                {
                    addError("El recurso se encuentra ocupado en esa fecha y hora");
                }

                if (errors != null)
                {
                    saveErrors();
                    return RedirectToAction("NuevaReserva", "Socio");
                }
                
                // Guardamos los datos en la BBDD
                reserva.socios = db_socio;
                reserva.recursos = db_recurso;
                facturas factura = new facturas();
                factura.xml_factura = "";
                reserva.facturas = factura;
                db.reservas.Add(reserva);
                factura.id_reserva = reserva.id;
                db.facturas.Add(factura);
                db.SaveChanges();
                factura.xml_factura = Tools.generarXmlFactura(factura, reserva);
                db.Entry(factura).State = EntityState.Modified;
                db.SaveChanges();

                // Envio de email al socio con los datos de la reserva
                string subject = "Reserva realizada en Gestenis";
                string body = "<h1>Esto es un mensaje automático del sistema</h1>"
                    + "<p>" + db_socio.nombre + " ha realizado correctamente una reserva en GesTenis.</p>"
                    + "<p>Estos son los datos de su reserva:</p>"
                    + "<p>Nombre del recurso: " + reserva.recursos.nombre_rec + "</p>"
                    + "<p>Día: " + reserva.fecha.Date.ToString() + "</p>"
                    + "<p>Hora: " + reserva.hora.Hour.ToString() + "</p>"
                    + "<p>Una vez realizado el pago en conserjería, podrá visualizar la factura en su área de usuario.</p>";
                Tools.sendEmail(db_socio, subject, body);

                saveMessage("Reserva realizada con éxito");
                return RedirectToAction("MisReservas");
            }

            //ViewBag.id = new SelectList(db.facturas, "id_reserva", "xml_factura");
            return View(model);
        }

        public ActionResult DetallesReserva(int? id)
        {
            if (isSocio())
            {
                //Codigo que se ejecuta en caso de Socio
                if (id == null)
                {
                    addError("Ha de seleccionar una reserva");
                    saveErrors();
                    return RedirectToAction("MisReservas", "Socio");
                }
                reservas reserva = db.reservas.Find(id);
                if (reserva == null)
                {
                    addError("No tiene permiso para visualizar esta reserva");
                    saveErrors();
                    return RedirectToAction("MisReservas", "Socio");
                }

                string id_soc = Session["UserID"].ToString();
                if (reserva.socios.id != id_soc)
                {
                    addError("No tiene permiso para visualizar esta reserva");
                    saveErrors();
                    return RedirectToAction("MisReservas", "Socio");
                }
                return View(reserva);
            }
            else
                //Codigo que se ejecuta en caso de admin o no auth
                return RedirectToAction("Index", isAdmin() ? "Admin" : "Home");
        }

        public ActionResult EliminarReserva(int? id)
        {
            if (isSocio())
            {
                //Codigo que se ejecuta en caso de Socio
                if (id == null)
                {
                    addError("Ha de seleccionar una reserva para eliminar");
                    saveErrors();
                    return RedirectToAction("MisReservas", "Socio");
                }
                reservas reserva = db.reservas.Find(id);
                if (reserva == null)
                {
                    addError("La reserva seleccionada no existe");
                    saveErrors();
                    return RedirectToAction("MisReservas", "Socio");
                }
                string id_soc = Session["UserID"].ToString();
                if (reserva.socios.id != id_soc)
                {
                    addError("No tiene permiso para eliminar esta reserva");
                    saveErrors();
                    return RedirectToAction("MisReservas", "Socio");
                }
                if (DateTime.Compare(reserva.fecha, DateTime.Today) < 0)
                {
                    addError("No puede eliminar una reserva con fecha anterior a hoy");
                    saveErrors();
                    return RedirectToAction("MisReservas", "Socio");
                }

                return View(reserva);
            }
            else
            {
                //Codigo que se ejecuta en caso de socio o no auth
                return RedirectToAction("Index", isAdmin() ? "Admin" : "Home");
            }
        }

        // POST: Socio/EliminarReserva/5
        [HttpPost, ActionName("EliminarReserva")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarReservaConfirmado(int id)
        {
            reservas reserva = db.reservas.Find(id);
            db.reservas.Remove(reserva);
            db.SaveChanges();
            saveMessage("La reserva " + reserva.id + " ha sido eliminada");
            return RedirectToAction("MisReservas");
        }


        //----------------------------------------------------------------
        #endregion RESERVAS SOCIO

        #region DATOS SOCIO
        //--------------------------------------------------------------------------------
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
                //string id = (string)Session["UserID"];
                //var db_socio = db.socios.Where(x => x.id == id).First();
                return View(db.socios.Find((string)Session["UserID"]));
            }
            else return RedirectToAction("Index", isAdmin() ? "Admin" : "Home");
        }

        public ActionResult ModificarDatos()
        {
            if (isSocio())
            {
                //string id = (string)Session["UserID"];
                //var db_socio = db.socios.Where(x => x.id == id).First();
                return View(db.socios.Find((string)Session["UserID"]));
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

        //------------------------------------------------------------------------------------------------
        #endregion DATOS SOCIO

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