
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KermesseElysium.Models;

namespace KermesseElysium.Controllers
{
    public class TasaCambioDetsController : Controller
    {
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();

        // GET: TasaCambioDets
        public ActionResult Index()
        {
            var tasaCambioDet = db.TasaCambioDet.Include(t => t.TasaCambio1);
            return View(tasaCambioDet.ToList());
        }

        // GET: TasaCambioDets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TasaCambioDet tasaCambioDet = db.TasaCambioDet.Find(id);
            if (tasaCambioDet == null)
            {
                return HttpNotFound();
            }
            return View(tasaCambioDet);
        }

        // GET: TasaCambioDets/Create
        public ActionResult Create()
        {
            ViewBag.tasaCambio = new SelectList(db.TasaCambio, "idTasaCambio", "mes");
            return View();
        }

        // POST: TasaCambioDets/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idTasaCambioDet,tasaCambio,fecha,tipoCambio,estado")] TasaCambioDet tasaCambioDet)
        {
            if (ModelState.IsValid)
            {
                db.TasaCambioDet.Add(tasaCambioDet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.tasaCambio = new SelectList(db.TasaCambio, "idTasaCambio", "mes", tasaCambioDet.tasaCambio);
            return View(tasaCambioDet);
        }

        // GET: TasaCambioDets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TasaCambioDet tasaCambioDet = db.TasaCambioDet.Find(id);
            if (tasaCambioDet == null)
            {
                return HttpNotFound();
            }
            ViewBag.tasaCambio = new SelectList(db.TasaCambio, "idTasaCambio", "mes", tasaCambioDet.tasaCambio);
            return View(tasaCambioDet);
        }

        // POST: TasaCambioDets/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idTasaCambioDet,tasaCambio,fecha,tipoCambio,estado")] TasaCambioDet tasaCambioDet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tasaCambioDet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.tasaCambio = new SelectList(db.TasaCambio, "idTasaCambio", "mes", tasaCambioDet.tasaCambio);
            return View(tasaCambioDet);
        }

        // GET: TasaCambioDets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TasaCambioDet tasaCambioDet = db.TasaCambioDet.Find(id);
            if (tasaCambioDet == null)
            {
                return HttpNotFound();
            }
            return View(tasaCambioDet);
        }

        // POST: TasaCambioDets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TasaCambioDet tasaCambioDet = db.TasaCambioDet.Find(id);
            db.TasaCambioDet.Remove(tasaCambioDet);
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
