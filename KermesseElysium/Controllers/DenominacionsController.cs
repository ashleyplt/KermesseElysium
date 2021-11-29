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

        public ActionResult ReporteDenominacions(string tipo, string buscar = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RDenominacion.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <MarginTop>0cm</MarginTop>
                      <MarginLeft>0cm</MarginLeft>
                      <MarginRight>0cm</MarginRight>
                      <MarginBottom>0cm</MarginBottom>
                        <EmbedFonts>None</EmbedFonts>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            var Denominacion = from m in db.vw_denominacion select m;

            if (!string.IsNullOrEmpty(buscar))
            {
                Denominacion = Denominacion.Where(m => m.valorLetras.Contains(buscar));
            }

            List<vw_denominacion> ListaDen = new List<vw_denominacion>();
            ListaDen = Denominacion.ToList();

            ReportDataSource rds = new ReportDataSource("DSDenominacion", ListaDen);
            rpt.DataSources.Add(rds);

            var b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);
        }
        public ActionResult ReporteDenominacionIndiv(int id)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RDenominacionIndiv.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <MarginTop>0cm</MarginTop>
                      <MarginLeft>0cm</MarginLeft>
                      <MarginRight>0cm</MarginRight>
                      <MarginBottom>0cm</MarginBottom>
                        <EmbedFonts>None</EmbedFonts>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

            var Denominacion = from m in db.vw_denominacion select m;
            Denominacion = Denominacion.Where(m => m.idDenominacion == id);

            ReportDataSource rds = new ReportDataSource("DSDenominacion", Denominacion.ToList());
            rpt.DataSources.Add(rds);

            var b = rpt.Render("PDF", deviceInfo, out mt, out enc, out f, out s, out w);

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
