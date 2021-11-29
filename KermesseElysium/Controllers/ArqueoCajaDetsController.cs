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
    public class ArqueoCajaDetsController : Controller
    {
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();

        // GET: ArqueoCajaDets

        
  

        public ActionResult Index(string buscar = "")
        {
            var arqueoCajaDet = db.ArqueoCajaDet.Include(a => a.ArqueoCaja1).Include(a => a.Denominacion1).Include(a => a.Moneda1);
            if (!string.IsNullOrEmpty(buscar))
            {
                arqueoCajaDet = arqueoCajaDet.Where(g => g.Denominacion1.valorLetras.Contains(buscar));
            }
            return View(arqueoCajaDet.ToList());
        }

        // GET: ArqueoCajaDets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArqueoCajaDet arqueoCajaDet = db.ArqueoCajaDet.Find(id);
            if (arqueoCajaDet == null)
            {
                return HttpNotFound();
            }
            return View(arqueoCajaDet);
        }

        // GET: ArqueoCajaDets/Create
        public ActionResult Create()
        {
            ViewBag.arqueoCaja = new SelectList(db.ArqueoCaja, "idArqueoCaja", "idArqueoCaja");
            ViewBag.denominacion = new SelectList(db.Denominacion.Where( d => d.estado == 1 || d.estado == 2), "idDenominacion", "valorLetras");
            ViewBag.moneda = new SelectList(db.Moneda.Where(d => d.estado == 1 || d.estado == 2), "idMoneda", "nombre");
            return View();
        }

        // POST: ArqueoCajaDets/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idArqueoCajaDet,arqueoCaja,moneda,denominacion,cantidad,subtotal")] ArqueoCajaDet arqueoCajaDet)
        {
            if (ModelState.IsValid)
            {
                db.ArqueoCajaDet.Add(arqueoCajaDet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.arqueoCaja = new SelectList(db.ArqueoCaja, "idArqueoCaja", "idArqueoCaja", arqueoCajaDet.arqueoCaja);
            ViewBag.denominacion = new SelectList(db.Denominacion, "idDenominacion", "valorLetras", arqueoCajaDet.denominacion);
            ViewBag.moneda = new SelectList(db.Moneda, "idMoneda", "nombre", arqueoCajaDet.moneda);
            return View(arqueoCajaDet);
        }

        // GET: ArqueoCajaDets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArqueoCajaDet arqueoCajaDet = db.ArqueoCajaDet.Find(id);
            if (arqueoCajaDet == null)
            {
                return HttpNotFound();
            }
            ViewBag.arqueoCaja = new SelectList(db.ArqueoCaja, "idArqueoCaja", "idArqueoCaja", arqueoCajaDet.arqueoCaja);
            ViewBag.denominacion = new SelectList(db.Denominacion.Where(d => d.estado == 1 || d.estado == 2), "idDenominacion", "valorLetras", arqueoCajaDet.denominacion);
            ViewBag.moneda = new SelectList(db.Moneda.Where(d => d.estado == 1 || d.estado == 2), "idMoneda", "nombre", arqueoCajaDet.moneda);
            return View(arqueoCajaDet);
        }

        // POST: ArqueoCajaDets/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idArqueoCajaDet,arqueoCaja,moneda,denominacion,cantidad,subtotal")] ArqueoCajaDet arqueoCajaDet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arqueoCajaDet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.arqueoCaja = new SelectList(db.ArqueoCaja, "idArqueoCaja", "idArqueoCaja", arqueoCajaDet.arqueoCaja);
            ViewBag.denominacion = new SelectList(db.Denominacion, "idDenominacion", "valorLetras", arqueoCajaDet.denominacion);
            ViewBag.moneda = new SelectList(db.Moneda, "idMoneda", "nombre", arqueoCajaDet.moneda);
            return View(arqueoCajaDet);
        }

        // GET: ArqueoCajaDets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArqueoCajaDet arqueoCajaDet = db.ArqueoCajaDet.Find(id);
            if (arqueoCajaDet == null)
            {
                return HttpNotFound();
            }
            return View(arqueoCajaDet);
        }

        // POST: ArqueoCajaDets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArqueoCajaDet arqueoCajaDet = db.ArqueoCajaDet.Find(id);
            db.ArqueoCajaDet.Remove(arqueoCajaDet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Reportearq(string tipo, string buscar = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RArqueoCajaDets.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <MarginTop>0cm</MarginTop>
                      <MarginLeft>0cm</MarginLeft>
                      <MarginRight>0cm</MarginRight>
                      <MarginBottom>0cm</MarginBottom>
                        <EmbedFonts>None</EmbedFonts>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            var Arqueo = from m in db.vw_ArqueoCajaDetalle select m;

            if (!string.IsNullOrEmpty(buscar))
            {
                Arqueo = Arqueo.Where(m => m.kermesse.Contains(buscar));
            }

            List<vw_ArqueoCajaDetalle> listaDets = new List<vw_ArqueoCajaDetalle>();
            listaDets = Arqueo.ToList();

            ReportDataSource rds = new ReportDataSource("DSArqueoCajaDets", listaDets);
            rpt.DataSources.Add(rds);

            var b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);
        }
        public ActionResult ReportearqIndiv(int id)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RArqueoCajaDetsIndiv.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <MarginTop>0cm</MarginTop>
                      <MarginLeft>0cm</MarginLeft>
                      <MarginRight>0cm</MarginRight>
                      <MarginBottom>0cm</MarginBottom>
                        <EmbedFonts>None</EmbedFonts>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

            var arqueo = from m in db.vw_ArqueoCajaDetalle select m;
            arqueo = arqueo.Where(m => m.idArqueoCajaDet == id);

            ReportDataSource rds = new ReportDataSource("DSArqueoCajaDets", arqueo.ToList());
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
