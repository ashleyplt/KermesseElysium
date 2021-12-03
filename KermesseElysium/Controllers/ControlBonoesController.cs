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
    public class ControlBonoesController : Controller
    {
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();

        // GET: Gastoes
        public ActionResult Index(string buscar = "")
        {
            var gasto = from g in db.ControlBono select g;

            if (!string.IsNullOrEmpty(buscar))
            {
                gasto = gasto.Where(g => g.nombre.Contains(buscar));
            }
            return View(gasto.ToList());
        }

        // GET: ControlBonoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControlBono controlBono = db.ControlBono.Find(id);
            if (controlBono == null)
            {
                return HttpNotFound();
            }
            return View(controlBono);
        }

        // GET: ControlBonoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ControlBonoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idBono,nombre,valor,estado")] ControlBono controlBono)
        {
            if (ModelState.IsValid)
            {
                controlBono.estado = 1;
                db.ControlBono.Add(controlBono);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(controlBono);
        }

        // GET: ControlBonoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControlBono controlBono = db.ControlBono.Find(id);
            if (controlBono == null)
            {
                return HttpNotFound();
            }
            return View(controlBono);
        }

        // POST: ControlBonoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ControlBono c)
        {
            if (ModelState.IsValid)
            {

                var control = db.ControlBono.Find(c.idBono);
                control.nombre = c.nombre;
                control.valor = c.valor;
                control.estado = 2;
                db.Entry(control).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(c);
        }

        // GET: ControlBonoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControlBono controlBono = db.ControlBono.Find(id);
            if (controlBono == null)
            {
                return HttpNotFound();
            }
            return View(controlBono);
        }

        // POST: ControlBonoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ControlBono controlBono = db.ControlBono.Find(id);
            db.ControlBono.Remove(controlBono);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ReporteControlBono(string tipo, string buscar = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RControlBono.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <MarginTop>0cm</MarginTop>
                      <MarginLeft>0cm</MarginLeft>
                      <MarginRight>0cm</MarginRight>
                      <MarginBottom>0cm</MarginBottom>
                        <EmbedFonts>None</EmbedFonts>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            var CBono = from m in db.ControlBono select m;

            if (!string.IsNullOrEmpty(buscar))
            {
                CBono = CBono.Where(m => m.nombre.Contains(buscar));
            }

            List<ControlBono> listaCBono = new List<ControlBono>();
            listaCBono = CBono.ToList();

            ReportDataSource rds = new ReportDataSource("DSControlBono", listaCBono);
            rpt.DataSources.Add(rds);

            var b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);
        }

        public ActionResult ReporteControlBonoIndiv(int id)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RControlBonoIndiv.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <MarginTop>0cm</MarginTop>
                      <MarginLeft>0cm</MarginLeft>
                      <MarginRight>0cm</MarginRight>
                      <MarginBottom>0cm</MarginBottom>
                        <EmbedFonts>None</EmbedFonts>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

            var CBono = from m in db.ControlBono select m;
            CBono = CBono.Where(m => m.idBono == id);

            ReportDataSource rds = new ReportDataSource("DSCBono", CBono.ToList());
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
