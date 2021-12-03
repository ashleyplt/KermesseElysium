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
    public class ListaPrecioDetsController : Controller
    {
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();

        // GET: ListaPrecioDets
        public ActionResult Index(string buscar = "")
        {
            var listaPrecioDet = db.ListaPrecioDet.Include(l => l.ListaPrecio1).Include(l => l.Producto1);

            if (!string.IsNullOrEmpty(buscar))
            {
                listaPrecioDet = listaPrecioDet.Where(l  => l.ListaPrecio1.nombre.Contains(buscar));
            }
            return View(listaPrecioDet.ToList());


        }

        // GET: ListaPrecioDets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListaPrecioDet listaPrecioDet = db.ListaPrecioDet.Find(id);
            if (listaPrecioDet == null)
            {
                return HttpNotFound();
            }
            return View(listaPrecioDet);
        }

        // GET: ListaPrecioDets/Create
        public ActionResult Create()
        {
            ViewBag.listaPrecio = new SelectList(db.ListaPrecio, "idListaPrecio", "nombre");
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre");
            return View();
        }

        // POST: ListaPrecioDets/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idListaPrecioDet,listaPrecio,producto,precioVenta")] ListaPrecioDet listaPrecioDet)
        {
            if (ModelState.IsValid)
            {
                db.ListaPrecioDet.Add(listaPrecioDet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.listaPrecio = new SelectList(db.ListaPrecio, "idListaPrecio", "nombre", listaPrecioDet.listaPrecio);
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre", listaPrecioDet.producto);
            return View(listaPrecioDet);
        }

        // GET: ListaPrecioDets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListaPrecioDet listaPrecioDet = db.ListaPrecioDet.Find(id);
            if (listaPrecioDet == null)
            {
                return HttpNotFound();
            }
            ViewBag.listaPrecio = new SelectList(db.ListaPrecio, "idListaPrecio", "nombre", listaPrecioDet.listaPrecio);
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre", listaPrecioDet.producto);
            return View(listaPrecioDet);
        }

        // POST: ListaPrecioDets/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idListaPrecioDet,listaPrecio,producto,precioVenta")] ListaPrecioDet listaPrecioDet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(listaPrecioDet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.listaPrecio = new SelectList(db.ListaPrecio, "idListaPrecio", "nombre", listaPrecioDet.listaPrecio);
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre", listaPrecioDet.producto);
            return View(listaPrecioDet);
        }

        // GET: ListaPrecioDets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListaPrecioDet listaPrecioDet = db.ListaPrecioDet.Find(id);
            if (listaPrecioDet == null)
            {
                return HttpNotFound();
            }
            return View(listaPrecioDet);
        }

        // POST: ListaPrecioDets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ListaPrecioDet listaPrecioDet = db.ListaPrecioDet.Find(id);
            db.ListaPrecioDet.Remove(listaPrecioDet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult VerReporteListaDet(string tipo, string buscar = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RListap.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <MarginTop>0cm</MarginTop>
                      <MarginLeft>0cm</MarginLeft>
                      <MarginRight>0cm</MarginRight>
                      <MarginBottom>0cm</MarginBottom>
                        <EmbedFonts>None</EmbedFonts>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            var Lista = from m in db.vw_listapreciodet select m;

            if (!string.IsNullOrEmpty(buscar))
            {
                Lista = Lista.Where(m => m.listaPrecio.Contains(buscar));
            }

            List<vw_listapreciodet> listap = new List<vw_listapreciodet>();
            listap = Lista.ToList();

            ReportDataSource rds = new ReportDataSource("DSListap", listap);
            rpt.DataSources.Add(rds);

            var b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);
        }
        public ActionResult VerReporteListaIndiv(int id)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RListapi.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <MarginTop>0cm</MarginTop>
                      <MarginLeft>0cm</MarginLeft>
                      <MarginRight>0cm</MarginRight>
                      <MarginBottom>0cm</MarginBottom>
                        <EmbedFonts>None</EmbedFonts>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;


            var Lista = from m in db.vw_listapreciodet select m;


            Lista = Lista.Where(m => m.idListaPrecioDet == id);

            ReportDataSource rds = new ReportDataSource("DSListap", Lista.ToList());
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
