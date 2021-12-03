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
    public class IngresoComunidadDetsController : Controller
    {
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();

        // GET: IngresoComunidadDets
        public ActionResult Index(string buscar = "")
        {
            var ingresoComunidadDet = from ic in db.vw_ingresocomunidaddet select ic;

            if (!string.IsNullOrEmpty(buscar))
            {
                ingresoComunidadDet = ingresoComunidadDet.Where(ic => ic.ingresoComunidad.Contains(buscar) || ic.bono.Contains(buscar) || ic.denominacion.Contains(buscar) || ic.bono.Contains(buscar));
            }

            return View(ingresoComunidadDet.ToList());
        }

        // GET: IngresoComunidadDets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vw_ingresocomunidaddet ingresoComunidadDet = db.vw_ingresocomunidaddet.Find(id);
            if (ingresoComunidadDet == null)
            {
                return HttpNotFound();
            }
            return View(ingresoComunidadDet);
        }

        // GET: IngresoComunidadDets/Create
        public ActionResult Create()
        {
            ViewBag.bono = new SelectList(db.ControlBono.Where(ic => ic.estado == 1 || ic.estado == 2), "idBono", "nombre");
            ViewBag.ingresoComunidad = new SelectList(db.IngresoComunidad, "idIngresoComunidad", "idIngresoComunidad");
            return View();
        }

        // POST: IngresoComunidadDets/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idIngresoComunidadDet,ingresoComunidad,bono,denominacion,cantidad,subTotalBono")] IngresoComunidadDet ingresoComunidadDet)
        {
            if (ModelState.IsValid)
            {
                db.IngresoComunidadDet.Add(ingresoComunidadDet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.bono = new SelectList(db.ControlBono.Where(ic => ic.estado == 1 || ic.estado == 2), "idBono", "nombre", ingresoComunidadDet.bono);
            ViewBag.ingresoComunidad = new SelectList(db.IngresoComunidad, "idIngresoComunidad", "idIngresoComunidad", ingresoComunidadDet.ingresoComunidad);
            return View(ingresoComunidadDet);
        }

        // GET: IngresoComunidadDets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IngresoComunidadDet ingresoComunidadDet = db.IngresoComunidadDet.Find(id);
            if (ingresoComunidadDet == null)
            {
                return HttpNotFound();
            }
            ViewBag.bono = new SelectList(db.ControlBono.Where(ic => ic.estado == 1 || ic.estado == 2), "idBono", "nombre", ingresoComunidadDet.bono);
            ViewBag.ingresoComunidad = new SelectList(db.IngresoComunidad, "idIngresoComunidad", "idIngresoComunidad", ingresoComunidadDet.ingresoComunidad);
            return View(ingresoComunidadDet);
        }

        // POST: IngresoComunidadDets/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idIngresoComunidadDet,ingresoComunidad,bono,denominacion,cantidad,subTotalBono")] IngresoComunidadDet ingresoComunidadDet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ingresoComunidadDet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.bono = new SelectList(db.ControlBono.Where(ic => ic.estado == 1 || ic.estado == 2), "idBono", "nombre", ingresoComunidadDet.bono);
            ViewBag.ingresoComunidad = new SelectList(db.IngresoComunidad, "idIngresoComunidad", "idIngresoComunidad", ingresoComunidadDet.ingresoComunidad);
            return View(ingresoComunidadDet);
        }

        // GET: IngresoComunidadDets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vw_ingresocomunidaddet ingresoComunidadDet = db.vw_ingresocomunidaddet.Find(id);
            if (ingresoComunidadDet == null)
            {
                return HttpNotFound();
            }
            return View(ingresoComunidadDet);
        }

        // POST: IngresoComunidadDets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IngresoComunidadDet ingresoComunidadDet = db.IngresoComunidadDet.Find(id);
            db.IngresoComunidadDet.Remove(ingresoComunidadDet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult VerReporteIngresoComunidadDet(string tipo, string buscar = "")
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RIngresoComunidadDet.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <MarginTop>0cm</MarginTop>
                      <MarginLeft>0cm</MarginLeft>
                      <MarginRight>0cm</MarginRight>
                      <MarginBottom>0cm</MarginBottom>
                        <EmbedFonts>None</EmbedFonts>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            List<vw_ingresocomunidaddet> lista = new List<vw_ingresocomunidaddet>();
            var ingcomunidad = from ic in db.vw_ingresocomunidaddet select ic;

            if (!string.IsNullOrEmpty(buscar))
            {
                ingcomunidad = ingcomunidad.Where(ic => ic.ingresoComunidad.Contains(buscar) || ic.bono.Contains(buscar) || ic.denominacion.Contains(buscar) || ic.bono.Contains(buscar));
            }

            lista = ingcomunidad.ToList();

            ReportDataSource rds = new ReportDataSource("DSIngresoComunidadDet", lista);
            rpt.DataSources.Add(rds);

            var b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);
        }

        public ActionResult VerReporteIngresoComunidadDetIndiv(int id)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RIngresoComunidadDetIndiv.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <MarginTop>0cm</MarginTop>
                      <MarginLeft>0cm</MarginLeft>
                      <MarginRight>0cm</MarginRight>
                      <MarginBottom>0cm</MarginBottom>
                        <EmbedFonts>None</EmbedFonts>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            var ingresocomunidad = from ic in db.vw_ingresocomunidaddet select ic;
            ingresocomunidad = ingresocomunidad.Where(ic => ic.idIngresoComunidadDet == id);

            ReportDataSource rds = new ReportDataSource("DSIngresoComunidad", ingresocomunidad.ToList());
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
