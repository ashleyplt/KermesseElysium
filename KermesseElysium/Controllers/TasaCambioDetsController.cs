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
    public class TasaCambioDetsController : Controller
    {
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();

        // GET: TasaCambioDets
        public ActionResult Index(string buscar = "")
        {
            var tasaCambioDet = db.TasaCambioDet.Include(t => t.TasaCambio1);
            if (!string.IsNullOrEmpty(buscar))
            {
                tasaCambioDet = tasaCambioDet.Where(tc => tc.fecha.Equals(buscar) || tc.tipoCambio.ToString().Contains(buscar) || tc.TasaCambio1.Moneda.nombre.Contains(buscar) || tc.TasaCambio1.Moneda1.nombre.Contains(buscar));
            }
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
            ViewBag.tasaCambio = new SelectList(db.TasaCambio.Where(tc => tc.estado == 1 || tc.estado == 2), "idTasaCambio", "mes");
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
                tasaCambioDet.estado = 1;
                db.TasaCambioDet.Add(tasaCambioDet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.tasaCambio = new SelectList(db.TasaCambio.Where(tc => tc.estado == 1 || tc.estado == 2), "idTasaCambio", "mes", tasaCambioDet.tasaCambio);
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
            ViewBag.Fecha = tasaCambioDet.fecha.ToString("yyyy-MM-dd");
            ViewBag.tasaCambio = new SelectList(db.TasaCambio.Where(tc => tc.estado == 1 || tc.estado == 2), "idTasaCambio", "mes", tasaCambioDet.tasaCambio);
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
                tasaCambioDet.estado = 2;
                db.Entry(tasaCambioDet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.tasaCambio = new SelectList(db.TasaCambio.Where(tc => tc.estado == 1 || tc.estado == 2), "idTasaCambio", "mes", tasaCambioDet.tasaCambio);
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

            if (tasaCambioDet.estado.Equals(1) || tasaCambioDet.estado.Equals(2))
            {
                tasaCambioDet.estado = 3;
            }
            else
            {
                tasaCambioDet.estado = 2;
            }

            db.Entry(tasaCambioDet).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult VerReporteTasaDetalle(string tipo, string buscar = "")
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;


            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RTasaCambioDet.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <MarginTop>0cm</MarginTop>
                      <MarginLeft>0cm</MarginLeft>
                      <MarginRight>0cm</MarginRight>
                      <MarginBottom>0cm</MarginBottom>
                        <EmbedFonts>None</EmbedFonts>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            List<vw_tasacambiodet> lista = new List<vw_tasacambiodet>();
            var tasaCambioDet = from p in db.vw_tasacambiodet select p;
            tasaCambioDet = tasaCambioDet.Where(tc => tc.estado == 1 || tc.estado == 2);
            if (!string.IsNullOrEmpty(buscar))
            {
                tasaCambioDet = tasaCambioDet.Where(tc => tc.fecha.Equals(buscar) || tc.tipoCambio.ToString().Contains(buscar) || tc.tasaCambio.Contains(buscar));
            }

            lista = tasaCambioDet.ToList();

            ReportDataSource rds = new ReportDataSource("DsTasaCambioDet", lista);
            rpt.DataSources.Add(rds);

            var b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);
        }

        public ActionResult VerReporteTasaDetalleIndiv(int id)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RTasaCambioDetIndiv.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <MarginTop>0cm</MarginTop>
                      <MarginLeft>0cm</MarginLeft>
                      <MarginRight>0cm</MarginRight>
                      <MarginBottom>0cm</MarginBottom>
                        <EmbedFonts>None</EmbedFonts>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            var tasaCambioDet = from p in db.vw_tasacambiodet select p;
            tasaCambioDet = tasaCambioDet.Where(t => t.idTasaCambioDet == id);


            ReportDataSource rds = new ReportDataSource("DsTasaCambioDet", tasaCambioDet.ToList());
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
