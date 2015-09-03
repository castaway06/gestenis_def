using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GesTenis;

namespace GesTenis.Controllers
{
    public class interReservasController : Controller
    {
        private gestenis_defEntities db = new gestenis_defEntities();

        // GET: interReservas
        public ActionResult Index()
        {
            var reservas = db.reservas.Include(r => r.facturas);
            return View(reservas.ToList());
        }

        // GET: interReservas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            reservas reservas = db.reservas.Find(id);
            if (reservas == null)
            {
                return HttpNotFound();
            }
            return View(reservas);
        }

        // GET: interReservas/Create
        public ActionResult Create()
        {
            ViewBag.id = new SelectList(db.facturas, "id_reserva", "xml_factura");
            return View();
        }

        // POST: interReservas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,fecha,hora,pagado,precio")] reservas reservas)
        {
            if (ModelState.IsValid)
            {
                db.reservas.Add(reservas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id = new SelectList(db.facturas, "id_reserva", "xml_factura", reservas.id);
            return View(reservas);
        }

        // GET: interReservas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            reservas reservas = db.reservas.Find(id);
            if (reservas == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = new SelectList(db.facturas, "id_reserva", "xml_factura", reservas.id);
            return View(reservas);
        }

        // POST: interReservas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,fecha,hora,pagado,precio")] reservas reservas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id = new SelectList(db.facturas, "id_reserva", "xml_factura", reservas.id);
            return View(reservas);
        }

        // GET: interReservas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            reservas reservas = db.reservas.Find(id);
            if (reservas == null)
            {
                return HttpNotFound();
            }
            return View(reservas);
        }

        // POST: interReservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            reservas reservas = db.reservas.Find(id);
            db.reservas.Remove(reservas);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
