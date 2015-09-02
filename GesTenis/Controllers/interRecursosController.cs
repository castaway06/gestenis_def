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
    public class interRecursosController : Controller
    {
        private gestenis_defEntities db = new gestenis_defEntities();

        // GET: interRecursos
        public ActionResult Index()
        {
            return View(db.recursos.ToList());
        }

        // GET: interRecursos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            recursos recursos = db.recursos.Find(id);
            if (recursos == null)
            {
                return HttpNotFound();
            }
            return View(recursos);
        }

        // GET: interRecursos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: interRecursos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,tipo,f_alta,f_baja,nombre_rec,superficie,disponible")] recursos recursos)
        {
            if (ModelState.IsValid)
            {
                db.recursos.Add(recursos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(recursos);
        }

        // GET: interRecursos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            recursos recursos = db.recursos.Find(id);
            if (recursos == null)
            {
                return HttpNotFound();
            }
            return View(recursos);
        }

        // POST: interRecursos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,tipo,f_alta,f_baja,nombre_rec,superficie,disponible")] recursos recursos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recursos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(recursos);
        }

        // GET: interRecursos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            recursos recursos = db.recursos.Find(id);
            if (recursos == null)
            {
                return HttpNotFound();
            }
            return View(recursos);
        }

        // POST: interRecursos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            recursos recursos = db.recursos.Find(id);
            db.recursos.Remove(recursos);
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
