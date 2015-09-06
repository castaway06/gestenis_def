using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GesTenis.tools;
using GesTenis.Models;
using System.Data.Entity;
using GesTenis.tools;

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
                ViewBag.n_socios = db.socios.Count().ToString();
                ViewBag.n_recursos = db.recursos.Count().ToString();
                ViewBag.n_reservas = db.reservas.Count().ToString();
                ViewBag.n_reservas_hoy = db.reservas.Where(x => x.fecha == DateTime.Today).Count().ToString();
                return View();
            }
            else return RedirectToAction("Index", isSocio() ? "Socio" : "Home");
        }

        #region SOCIOS

        //-----------------------------------------------------------------------------------------------------

        public ActionResult ListadoDeSocios()
        {
            if (isAdmin()) return View(db.socios.ToList());
            else return RedirectToAction("Index", isSocio() ? "Socio" : "Home");
        }

        public ActionResult NuevoSocio()
        {
            if (isAdmin()) return View();
            else return RedirectToAction("Index", isSocio() ? "Socio" : "Home");
        }

        // POST: Admin/NuevoSocio
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevoSocio([Bind(Include = "id,password,is_admin,nombre,apellidos,nif,email,direccion1,direccion2,telefono")] socios socio)
        {
            if (ModelState.IsValid)
            {
                // Comprobar si el id de socio ya existe
                socios db_posible_socio = db.socios.Find(socio.id);
                if (db_posible_socio != null)
                {
                    addError("Ya existe un socio registrado con este ID. Por favor, elija otro");
                    saveErrors();
                    return View(socio);
                    //return RedirectToAction("ListadoDeSocios", "Admin");
                }

                string pass = Tools.randomPassword(6);
                socio.password = Tools.SHA256Encrypt(pass);
                socio.f_alta = DateTime.Today;
                db.socios.Add(socio);
                db.SaveChanges();
                // Envío de email de alta al socio
                string subject = "REGISTRO correcto en Gestenis";
                string body = "<h1>Esto es un mensaje automático del sistema</h1>"
                    + "<p>El administrador de sistema le ha registrado correctamente como SOCIO: " + socio.nombre + " " + socio.apellidos + ".</p>"
                    + "<p>Su nombre de usuario es: " + socio.id + "</p>"
                    + "<p>Su contraseña es: " + pass
                    + "<p>Le recomendamos que cambie esta contraseña por una nueva, que recuerde más fácilmente, desde su área de usuario.</p>";
                Tools.sendEmail(socio, subject, body);
                saveMessage("El socio " + socio.id + " se ha registrado correctamente en GesTenis");
                return RedirectToAction("ListadoDeSocios", "Admin");
            }
            addError("el Binder ha fallado");
            saveErrors();
            return View(socio);
        }


        [HttpPost]
        public ActionResult LoadSocios(string nif)
        {
            List<GesTenis.socios> ret = null;
            if (nif == null || nif == "")
            {
                ret = null;
            }
            else if (nif == "all")
            {
                ret = db.socios.ToList();
            }
            else
            {
                ret = (from socio in db.socios where socio.nif.Contains(nif) select socio).ToList();

            }
            return PartialView(ret);
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
                    return RedirectToAction("ListadoDeSocios", "Admin");
                }
                socios socio = db.socios.Find(id);
                if (socio == null)
                {
                    addError("El socio seleccionado no existe");
                    saveErrors();
                    return RedirectToAction("ListadoDeSocios", "Admin");
                }
                return View(socio);
            }
            else
                //Codigo que se ejecuta en caso de socio o no auth
                return RedirectToAction("Index", isSocio() ? "Socio" : "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarSocio([Bind(Include = "id,password,is_admin,nombre,apellidos,nif,email,telefono,direccion1,direccion2,f_alta,f_baja")]socios socio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(socio).State = EntityState.Modified;
                db.SaveChanges();
                saveMessage("Los datos del socio " + socio.id + " han sido modificados correctamente");
                return RedirectToAction("DetallesSocio", new { controller = "Admin", id = socio.id });
            }

            return RedirectToAction("ListadoDeSocios", "Admin");
        }

        public ActionResult DetallesSocio(string id)
        {
            if (isAdmin())
            {
                //Codigo que se ejecuta en caso de Admin
                if (id == null)
                {
                    addError("Ha de seleccionar un socio");
                    saveErrors();
                    return RedirectToAction("ListadoDeSocios", "Admin");
                }
                socios socio = db.socios.Find(id);
                if (socio == null)
                {
                    addError("El socio seleccionado no existe");
                    saveErrors();
                    return RedirectToAction("ListadoDeSocios", "Admin");
                }
                return View(socio);
            }
            else
                //Codigo que se ejecuta en caso de socio o no auth
                return RedirectToAction("Index", isSocio() ? "Socio" : "Home");
        }

        public ActionResult EliminarSocio(string id)
        {
            if (isAdmin())
            {
                //Codigo que se ejecuta en caso de Admin
                if (id == null)
                {
                    addError("Ha de seleccionar un socio para eliminar");
                    saveErrors();
                    return RedirectToAction("ListadoDeSocios", "Admin");
                }
                socios socio = db.socios.Find(id);
                if (socio == null)
                {
                    addError("El socio seleccionado no existe");
                    saveErrors();
                    return RedirectToAction("ListadoDeSocios", "Admin");
                }
                return View(socio);
            }
            else
                //Codigo que se ejecuta en caso de socio o no auth
                return RedirectToAction("Index", isSocio() ? "Socio" : "Home");
        }

        // POST: intermedio/Delete/5
        [HttpPost, ActionName("EliminarSocio")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarSocioConfirmado(string id)
        {
            socios socio = db.socios.Find(id);
            db.socios.Remove(socio);
            db.SaveChanges();
            saveMessage("El socio " + socio.id + " ha sido eliminado.");
            return RedirectToAction("ListadoDeSocios", "Admin");
        }

        //------------------------------------------------------------------------------------------

        #endregion SOCIOS


        #region RECURSOS
        public ActionResult ListadoDeRecursos()
        {
            if (isAdmin()) return View(db.recursos.ToList());
            else return RedirectToAction("Index", isSocio() ? "Socio" : "Home");
        }

        public ActionResult NuevoRecurso()
        {
            if (isAdmin()) return View();
            else return RedirectToAction("Index", isSocio() ? "Socio" : "Home");
        }

        // POST: admin/NuevoRecurso
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevoRecurso([Bind(Include = "tipo,f_alta,nombre_rec,superficie")] recursos recurso) //falta id, f_baja y disponible en el bindado
        {
            if (ModelState.IsValid)
            {
                recurso.f_baja = null;
                recurso.disponible = true;
                db.recursos.Add(recurso);
                db.SaveChanges();
                saveMessage("El recurso " + recurso.nombre_rec + " ha sido creado con éxito");
                return RedirectToAction("ListadoDeRecursos");
            }

            return View(recurso);
        }

        public ActionResult EditarRecurso(string id)
        {
            int id2;
            Int32.TryParse(id, out id2);
            if (isAdmin())
            {
                //Codigo que se ejecuta en caso de Admin
                if (id2 == 0)
                {
                    addError("Ha de seleccionar un recurso para editar");
                    saveErrors();
                    return RedirectToAction("ListadoDeRecursos", "Admin");
                }
                recursos recurso = db.recursos.Find(id2);
                if (recurso == null)
                {
                    addError("El recurso seleccionado no existe");
                    saveErrors();
                    return RedirectToAction("ListadoDeRecursos", "Admin");
                }
                return View(recurso);
            }
            else
                //Codigo que se ejecuta en caso de socio o no auth
                return RedirectToAction("Index", isSocio() ? "Socio" : "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarRecurso([Bind(Include = "id,tipo,f_alta,f_baja,nombre_rec,superficie,disponible")] recursos recurso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recurso).State = EntityState.Modified;
                db.SaveChanges();
                saveMessage("Los datos del recurso " + recurso.nombre_rec + " han sido modificados correctamente");
                return RedirectToAction("DetallesRecurso", new { controller = "Admin", id = recurso.id });
            }

            return RedirectToAction("ListadoDeSocios", "Admin");
        }

        public ActionResult DetallesRecurso(string id)
        {
            int id2;
            Int32.TryParse(id, out id2);
            if (isAdmin())
            {
                //Codigo que se ejecuta en caso de Admin
                if (id2 == 0)
                {
                    addError("Ha de seleccionar un recurso.");
                    saveErrors();
                    return RedirectToAction("ListadoDeRecursos", "Admin");
                }
                recursos recurso = db.recursos.Find(id2);
                if (recurso == null)
                {
                    addError("El recurso seleccionado no existe");
                    saveErrors();
                    return RedirectToAction("ListadoDeRecursos", "Admin");
                }
                return View(recurso);
            }
            else
                //Codigo que se ejecuta en caso de socio o no auth
                return RedirectToAction("Index", isSocio() ? "Socio" : "Home");
        }

        public ActionResult EliminarRecurso(string id)
        {
            int id2;
            Int32.TryParse(id, out id2);
            if (isAdmin())
            {
                //Codigo que se ejecuta en caso de Admin
                if (id2 == 0)
                {
                    addError("Ha de seleccionar un recurso para eliminar");
                    saveErrors();
                    return RedirectToAction("ListadoDeRecursos", "Admin");
                }
                recursos recurso = db.recursos.Find(id2);
                if (recurso == null)
                {
                    addError("El recurso seleccionado no existe");
                    saveErrors();
                    return RedirectToAction("ListadoDeRecursos", "Admin");
                }
                return View(recurso);
            }
            else
                //Codigo que se ejecuta en caso de socio o no auth
                return RedirectToAction("Index", isSocio() ? "Socio" : "Home");
        }

        // POST: intermedio/Delete/5
        [HttpPost, ActionName("EliminarRecurso")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarRecursoConfirmado(string id)
        {
            int id2;
            Int32.TryParse(id, out id2);
            recursos recurso = db.recursos.Find(id2);
            db.recursos.Remove(recurso);
            db.SaveChanges();
            saveMessage("El recurso " + recurso.nombre_rec + " ha sido eliminado.");
            return RedirectToAction("ListadoDeRecursos", "Admin");
        }


        #endregion RECURSOS

        #region RESERVAS
        //---------------------------------------------------------------------------------------

        public ActionResult ListadoDeReservas()
        {
            if (isAdmin())
            {
                // todas las reservas ordenadas por recurso y por fecha-hora
                var reservas = db.reservas.OrderBy(x => x.recursos.id).ThenBy(y => y.hora);
                return View(reservas.ToList());
            }
            else return RedirectToAction("Index", isSocio() ? "Socio" : "Home");
        }

        [HttpPost]
        public ActionResult LoadReservas(DateTime fecha)
        {
            List<GesTenis.reservas> ret = null;
            if (fecha == null)
            {
                ret = null;
            }
            else
            {
                ret = db.reservas.Where(x => x.fecha == fecha).ToList();
            }
            return PartialView(ret);
        }


        public ActionResult NuevaReserva()
        {
            if (isAdmin())
            {
                ViewBag.id = new SelectList(db.facturas, "id_reserva", "xml_factura");
                return View();
            }
            else return RedirectToAction("Index", isSocio() ? "Socio" : "Home");
        }

        // POST: Admin/NuevaReserva
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevaReserva([Bind(Include = "id_soc,id_rec,fecha,hora,pagado")] nuevaReservaAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Comprobacion existencia de socio, si su cuota está expirada y disponibilidad del recurso
                socios db_socio = db.socios.Find(model.id_soc);
                if (db_socio == null)
                {
                    addError("El socio no existe");
                }
                DateTime fecha_baja = db_socio.f_baja ?? default(DateTime);
                if (DateTime.Compare(fecha_baja, DateTime.Today) < 0)
                {
                    addError("El socio tiene expirada su cuota de socio o aún no ha sido activado");
                }

                // Recuperamos el recurso y comprobamos si existe y esta disponible
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
                reserva.pagado = model.pagado;
                reserva.precio = 3;

                // Comprobacion que la fecha y/u hora no son anteriores a hoy
                if (DateTime.Compare(reserva.fecha, DateTime.Today) <0)
                {
                    addError("La fecha de la reserva no puede ser anterior a hoy");
                }
                if (DateTime.Compare(reserva.hora, DateTime.Now) <0)
                {
                    addError("La hora de la reserva no puede ser anterior a ahora");
                }

                // Comprobacion que el socio no ha realizado reserva la fecha seleccionada
                reservas res_soc = db.reservas.Where(x => x.socios.id == model.id_soc && x.fecha == model.fecha).FirstOrDefault();
                if (res_soc != null)
                {
                    addError("El socio ya ha hecho reserva en esta fecha (solo se permite una reserva por dia)");
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
                    return RedirectToAction("NuevaReserva","Admin");
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
                    + "<p>" +db_socio.nombre +" el administrador de sistema le ha realizado correctamente una reserva en GesTenis.</p>"
                    + "<p>Estos son los datos de la reserva:</p>"
                    + "<p>Nombre del recurso: " + reserva.recursos.nombre_rec + "</p>"
                    + "<p>Día: " + reserva.fecha.Date.ToString() + "</p>"
                    + "<p>Hora: " + reserva.hora.Hour.ToString() + "</p>"
                    + "<p>Una vez realizado el pago en conserjería, podrá visualizar la factura en su área de usuario.</p>";
                Tools.sendEmail(db_socio, subject, body);

                saveMessage("La reserva se ha realizado con éxito.");
                return RedirectToAction("ListadoDeReservas");
            }

            return View(model);
        }


        public ActionResult EditarReserva(int? id)
        {
            if (isAdmin())
            {
                if (id == null)
                {
                    addError("Ha de seleccionar una reserva para editar");
                    saveErrors();
                    return RedirectToAction("ListadoDeReservas", "Admin");
                }
                reservas reserva = db.reservas.Find(id);
                if (reserva == null)
                {
                    addError("La reserva seleccionada no existe");
                    saveErrors();
                    return RedirectToAction("ListadoDeReservas", "Admin");
                }
                //ViewBag.id = new SelectList(db.facturas, "id_reserva", "xml_factura", reserva.id);
                return View(reserva);
            }
            else
            {
                //Codigo que se ejecuta en caso de socio o no auth
                return RedirectToAction("Index", isSocio() ? "Socio" : "Home");
            }
        }

        // POST: Admin/EditarReserva/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarReserva([Bind(Include = "id,fecha,hora,pagado,precio")] reservas reserva)
        {
            if (ModelState.IsValid)
            {
                reserva.hora = new DateTime(reserva.fecha.Year, reserva.fecha.Month, reserva.fecha.Day, reserva.hora.Hour, reserva.hora.Minute, reserva.hora.Second);
                db.Entry(reserva).State = EntityState.Modified;
                db.SaveChanges();
                saveMessage("Los datos de la reserva " + reserva.id + " han sido modificados correctamente");
                return RedirectToAction("ListadoDeReservas");
            }
            ViewBag.id = new SelectList(db.facturas, "id_reserva", "xml_factura", reserva.id);
            return View(reserva);
        }


        public ActionResult DetallesReserva(int? id)
        {
            if (isAdmin())
            {
                //Codigo que se ejecuta en caso de Admin
                if (id == null)
                {
                    addError("Ha de seleccionar una reserva");
                    saveErrors();
                    return RedirectToAction("ListadoDeReservas", "Admin");
                }
                reservas reserva = db.reservas.Find(id);
                if (reserva == null)
                {
                    addError("La reserva seleccionada no existe");
                    saveErrors();
                    return RedirectToAction("ListadoDeReservas", "Admin");
                }
                return View(reserva);
            }
            else
                //Codigo que se ejecuta en caso de socio o no auth
                return RedirectToAction("Index", isSocio() ? "Socio" : "Home");
        }

        public ActionResult EliminarReserva(int? id)
        {
            if (isAdmin())
            {
                //Codigo que se ejecuta en caso de Admin
                if (id == null)
                {
                    addError("Ha de seleccionar una reserva para eliminar");
                    saveErrors();
                    return RedirectToAction("ListadoDeReservas", "Admin");
                }
                reservas reserva = db.reservas.Find(id);
                if (reserva == null)
                {
                    addError("La reserva seleccionada no existe");
                    saveErrors();
                    return RedirectToAction("ListadoDeReservas", "Admin");
                }
                return View(reserva);
            }
            else
            {
                //Codigo que se ejecuta en caso de socio o no auth
                return RedirectToAction("Index", isSocio() ? "Socio" : "Home");
            }
        }

        // POST: Admin/EliminarReserva/5
        [HttpPost, ActionName("EliminarReserva")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarReservaConfirmado(int id)
        {
            reservas reserva = db.reservas.Find(id);
            db.reservas.Remove(reserva);
            db.SaveChanges();
            saveMessage("La reserva " + reserva.id + " ha sido eliminada");
            return RedirectToAction("ListadoDeReservas");
        }



        //---------------------------------------------------------------------------------------
        #endregion RESERVAS


        #region Datos Admin
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
                //string id = (string)Session["UserID"];
                //var db_socio = db.socios.Where(x => x.id == id).First();
                return View(db.socios.Find((string)Session["UserID"]));
            }
            else return RedirectToAction("Index", isSocio() ? "Socio" : "Home");

        }

        public ActionResult ModificarDatos()
        {
            if (isAdmin())
            {
                //string id = (string)Session["UserID"];
                //var db_socio = db.socios.Where(x => x.id == id).First();
                return View(db.socios.Find((string)Session["UserID"]));
            }
            else return RedirectToAction("Index", isSocio() ? "Socio" : "Home");
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
                return RedirectToAction("MisDatos", "Admin");
            }

            return RedirectToAction("ModificarDatos", "Admin", socio);
        }

        #endregion Datos Admin

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
