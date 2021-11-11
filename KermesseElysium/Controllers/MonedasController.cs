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
    public class MonedasController : Controller
    {
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();
        // GET: Monedas
        public ActionResult Index(string buscar = "")
        {
            var moneda = from m in db.Moneda select m;

            moneda = moneda.Where(m => m.estado.Equals(2) || m.estado.Equals(1));
            if (!string.IsNullOrEmpty(buscar))
            {
                moneda = moneda.Where(m => m.nombre.Contains(buscar));
            }

            return View(moneda.ToList());
        }

        // GET: Monedas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Moneda moneda = db.Moneda.Find(id);
            if (moneda == null)
            {
                return HttpNotFound();
            }
            return View(moneda);
        }

        // GET: Monedas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Monedas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Moneda m)
        {
            if(ModelState.IsValid)
            {
                var moneda = new Moneda();
                moneda.idMoneda = 0;
                moneda.nombre = m.nombre;
                moneda.simbolo = m.simbolo;
                moneda.estado = 1;
                db.Moneda.Add(moneda);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(m);
        }

        // GET: Monedas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Moneda moneda = db.Moneda.Find(id);
            if (moneda == null)
            {
                return HttpNotFound();
            }
            return View(moneda);
        }

        // POST: Monedas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Moneda m)
        {
            if (ModelState.IsValid)
            {
                var moneda = new Moneda();
                moneda.idMoneda = m.idMoneda;
                moneda.nombre = m.nombre;
                moneda.simbolo = m.simbolo;
                moneda.estado = 2;
                db.Entry(moneda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(m);
        }

        // GET: Monedas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Moneda moneda = db.Moneda.Find(id);
            if (moneda == null)
            {
                return HttpNotFound();
            }
            return View(moneda);
        }

        // POST: Monedas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Moneda moneda = db.Moneda.Find(id);
            moneda.estado = 3;

            db.Entry(moneda).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult VerReporteMoneda(string tipo, string buscar = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RMoneda.rdlc");

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            var moneda = from m in db.Moneda select m;
            moneda = moneda.Where(m => m.estado.Equals(2) || m.estado.Equals(1));
            if (!string.IsNullOrEmpty(buscar))
            {
                moneda = moneda.Where(m => m.nombre.Contains(buscar));
            }

            List<Moneda> listaMon = new List<Moneda>();
            listaMon = moneda.ToList();

            ReportDataSource rds = new ReportDataSource("DSMoneda", listaMon);
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
