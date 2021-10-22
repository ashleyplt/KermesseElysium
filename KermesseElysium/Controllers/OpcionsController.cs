using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KermesseElysium.Models;
using Microsoft.Reporting.WebForms;

namespace KermesseElysium.Controllers
{
    public class OpcionsController : Controller
    {
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();

        // GET: Opcions
        public ActionResult Index()
        {
            return View(db.Opcion.ToList());
        }

        // GET: Opcions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opcion opcion = db.Opcion.Find(id);
            if (opcion == null)
            {
                return HttpNotFound();
            }
            return View(opcion);
        }

        // GET: Opcions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Opcions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Opcion o)
        {
            if (ModelState.IsValid)
            {
                var opcion = new Opcion();
                opcion.idOpcion = 0;
                opcion.opcionDescripcion = o.opcionDescripcion;
                opcion.estado = 1;
                db.Opcion.Add(opcion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(o);
        }

        // GET: Opcions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opcion opcion = db.Opcion.Find(id);
            if (opcion == null)
            {
                return HttpNotFound();
            }
            return View(opcion);
        }

        // POST: Opcions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Opcion opcion)
        {
            if (ModelState.IsValid)
            {
                var o = new Opcion();
                o.idOpcion = opcion.idOpcion;
                o.opcionDescripcion = opcion.opcionDescripcion;
                o.estado = 2;

                db.Entry(o).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(opcion);
        }

        // GET: Opcions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opcion opcion = db.Opcion.Find(id);
            if (opcion == null)
            {
                return HttpNotFound();
            }
            return View(opcion);
        }

        // POST: Opcions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Opcion opcion = db.Opcion.Find(id);
            opcion.estado = 3;

            db.Entry(opcion).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult VerReporteOpciones(string tipo)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "ROpcion.rdlc");

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            List<Opcion> listaOpc = new List<Opcion>();
            listaOpc = modelo.Opcion.ToList();

            ReportDataSource rds = new ReportDataSource("DSOpcion", listaOpc);
            rpt.DataSources.Add(rds);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return File(b, mt);
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
