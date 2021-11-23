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
    public class TasaCambiosController : Controller
    {
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();

        // GET: TasaCambios
        public ActionResult Index(string buscar)
        {
            var tasacambio = from t in db.vw_tasacambio select t;
            tasacambio = tasacambio.Where(t => t.estado.Equals(1) || t.estado.Equals(2));
            if (!string.IsNullOrEmpty(buscar))
            {
                tasacambio = tasacambio.Where(tc => tc.anio.Equals(buscar) || tc.monedaO.Contains(buscar) || tc.monedaC.Contains(buscar) || tc.mes.Contains(buscar));
            }
            
            return View(tasacambio.ToList());
        }

        // GET: TasaCambios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vw_tasacambio vw_tasacambio = db.vw_tasacambio.Find(id);
            if (vw_tasacambio == null)
            {
                return HttpNotFound();
            }
            return View(vw_tasacambio);
        }

        // GET: TasaCambios/Create
        public ActionResult Create()
        {
            ViewBag.monedaO = new SelectList(db.Moneda, "idMoneda", "nombre");
            ViewBag.monedaC = new SelectList(db.Moneda, "idMoneda", "nombre");
            return View();
        }

        // POST: TasaCambios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idTasaCambio,monedaO,monedaC,mes,anio,estado")] TasaCambio tasaCambio)
        {
            if (ModelState.IsValid)
            {
                tasaCambio.estado = 1;
                db.TasaCambio.Add(tasaCambio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.monedaO = new SelectList(db.Moneda.Where(m => m.estado == 1 || m.estado == 2), "idMoneda", "nombre", tasaCambio.monedaO);
            ViewBag.monedaC = new SelectList(db.Moneda.Where(m => m.estado == 1 || m.estado == 2), "idMoneda", "nombre", tasaCambio.monedaC);
            return View(tasaCambio);
        }

        // GET: TasaCambios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TasaCambio tasaCambio = db.TasaCambio.Find(id);
            if (tasaCambio == null)
            {
                return HttpNotFound();
            }
            ViewBag.monedaO = new SelectList(db.Moneda.Where(m => m.estado == 1 || m.estado == 2), "idMoneda", "nombre", tasaCambio.monedaO);
            ViewBag.monedaC = new SelectList(db.Moneda.Where(m => m.estado == 1 || m.estado == 2), "idMoneda", "nombre", tasaCambio.monedaC);
            return View(tasaCambio);
        }

        // POST: TasaCambios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idTasaCambio,monedaO,monedaC,mes,anio,estado")] TasaCambio tasaCambio)
        {
            if (ModelState.IsValid)
            {
                tasaCambio.estado = 2;
                db.Entry(tasaCambio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.monedaO = new SelectList(db.Moneda.Where(m => m.estado == 1 || m.estado == 2), "idMoneda", "nombre", tasaCambio.monedaO);
            ViewBag.monedaC = new SelectList(db.Moneda.Where(m => m.estado == 1 || m.estado == 2), "idMoneda", "nombre", tasaCambio.monedaC);
            return View(tasaCambio);
        }

        // GET: TasaCambios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vw_tasacambio vw_tasaCambio = db.vw_tasacambio.Find(id);
            if (vw_tasaCambio == null)
            {
                return HttpNotFound();
            }
            return View(vw_tasaCambio);
        }

        // POST: TasaCambios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TasaCambio tasaCambio = db.TasaCambio.Find(id);
            tasaCambio.estado = 3;
            db.Entry(tasaCambio).State = EntityState.Modified;
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
