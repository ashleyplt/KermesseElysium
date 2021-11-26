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
    public class ComunidadsController : Controller
    {
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();

        // GET: Comunidads
        public ActionResult Index(string buscar)
        {
            var comunidad = from c in db.Comunidad select c;
            if (!string.IsNullOrEmpty(buscar))
            {
                comunidad = comunidad.Where(c => c.nombre.Contains(buscar) || c.responsble.Contains(buscar));
            }
            return View(comunidad.ToList());
        }

        // GET: Comunidads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comunidad comunidad = db.Comunidad.Find(id);
            if (comunidad == null)
            {
                return HttpNotFound();
            }
            return View(comunidad);
        }

        // GET: Comunidads/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Comunidads/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Comunidad comunidad)
        {
            if (ModelState.IsValid)
            {
                comunidad.estado = 1;
                db.Comunidad.Add(comunidad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(comunidad);
        }

        // GET: Comunidads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comunidad comunidad = db.Comunidad.Find(id);
            if (comunidad == null)
            {
                return HttpNotFound();
            }
            return View(comunidad);
        }

        // POST: Comunidads/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Comunidad comunidad)
        {
            if (ModelState.IsValid)
            {
                comunidad.estado = 2;
                db.Entry(comunidad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comunidad);
        }

        // GET: Comunidads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comunidad comunidad = db.Comunidad.Find(id);
            if (comunidad == null)
            {
                return HttpNotFound();
            }
            return View(comunidad);
        }

        // POST: Comunidads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            
            Comunidad comunidad = db.Comunidad.Find(id);
            comunidad.estado = 3;
            db.Entry(comunidad).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult VerReporteComunidad(string tipo, string buscar = "")
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;


            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RComunidad.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <MarginTop>0cm</MarginTop>
                      <MarginLeft>0cm</MarginLeft>
                      <MarginRight>0cm</MarginRight>
                      <MarginBottom>0cm</MarginBottom>
                        <EmbedFonts>None</EmbedFonts>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            List<Comunidad> lista = new List<Comunidad>();
            var comunidad = from c in db.Comunidad select c;

            if (!string.IsNullOrEmpty(buscar))
            {
                comunidad = comunidad.Where(c => c.nombre.Contains(buscar) || c.responsble.Contains(buscar));
            }

            lista = comunidad.ToList();

            ReportDataSource rds = new ReportDataSource("DsComunidad", lista);
            rpt.DataSources.Add(rds);

            var b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);
        }

        public ActionResult VerReporteComunidadIndiv(int id)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RComunidadIndiv.rdlc"); 
            string deviceInfo = @"<DeviceInfo>
                      <MarginTop>0cm</MarginTop>
                      <MarginLeft>0cm</MarginLeft>
                      <MarginRight>0cm</MarginRight>
                      <MarginBottom>0cm</MarginBottom>
                        <EmbedFonts>None</EmbedFonts>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            var comunidad = from c in db.Comunidad select c;
            comunidad = comunidad.Where(t => t.idComunidad == id);

            ReportDataSource rds = new ReportDataSource("DsComunidadIndiv", comunidad.ToList());
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
