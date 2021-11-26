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
    public class DenominacionsController : Controller
    {
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();

        // GET: Denominacions
        public ActionResult Index(string buscar = "")

        {
            var denominacion = from g in db.Denominacion select g;

            if (!string.IsNullOrEmpty(buscar))
            {
                denominacion = denominacion.Where(g => g.valorLetras.Equals(buscar));
            }

            return View(denominacion.ToList());
        }

        // GET: Denominacions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Denominacion denominacion = db.Denominacion.Find(id);
            if (denominacion == null)
            {
                return HttpNotFound();
            }
            return View(denominacion);
        }

        // GET: Denominacions/Create
        public ActionResult Create()
        {
            ViewBag.moneda = new SelectList(db.Moneda.Where(d => d.estado == 1 || d.estado == 2), "idMoneda", "nombre");
            return View();
        }

        // POST: Denominacions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idDenominacion,moneda,valor,valorLetras,estado")] Denominacion denominacion)
        {
            if (ModelState.IsValid)
            {
                denominacion.estado = 1;
                db.Denominacion.Add(denominacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.moneda = new SelectList(db.Moneda, "idMoneda", "nombre", denominacion.moneda);
            return View(denominacion);
        }

        // GET: Denominacions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Denominacion denominacion = db.Denominacion.Find(id);
            if (denominacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.moneda = new SelectList(db.Moneda.Where(d => d.estado == 1 || d.estado == 2), "idMoneda", "nombre", denominacion.moneda);
            return View(denominacion);
        }

        // POST: Denominacions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idDenominacion,moneda,valor,valorLetras,estado")] Denominacion denominacion)
        {
            if (ModelState.IsValid)
            {
                denominacion.estado = 2;
                db.Entry(denominacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.moneda = new SelectList(db.Moneda, "idMoneda", "nombre", denominacion.moneda);
            return View(denominacion);
        }

        // GET: Denominacions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Denominacion denominacion = db.Denominacion.Find(id);
            if (denominacion == null)
            {
                return HttpNotFound();
            }
            return View(denominacion);
        }

        // POST: Denominacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Denominacion denominacion = db.Denominacion.Find(id);
            denominacion.estado = 3;
            db.Entry(denominacion).State = EntityState.Modified;
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
