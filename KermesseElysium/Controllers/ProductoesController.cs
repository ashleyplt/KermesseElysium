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
    public class ProductoesController : Controller
    {
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();

        // GET: Productoes
        public ActionResult Index(string buscar = "")
        {

            var producto = from p in db.vw_producto select p;
            if(!string.IsNullOrEmpty(buscar))
            {
                producto = producto.Where(p => p.nombre.Contains(buscar) || p.descripcion.Contains(buscar));
            }
            return View(producto.ToList());
        }

        // GET: Productoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vw_producto producto = db.vw_producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: Productoes/Create
        public ActionResult Create()
        {
            ViewBag.catProd = new SelectList(db.CategoriaProducto.Where(o => o.estado ==1 || o.estado == 2), "idCatProd", "nombre");
            ViewBag.comunidad = new SelectList(db.Comunidad.Where(c => c.estado == 1 || c.estado == 2), "idComunidad", "nombre");
            return View();
        }

        // POST: Productoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idProducto,comunidad,catProd,nombre,descripcion,cantidad,precioVSugerido,estado")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                producto.estado = 1;
                db.Producto.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.catProd = new SelectList(db.CategoriaProducto.Where(o => o.estado == 1 || o.estado == 2), "idCatProd", "nombre", producto.catProd);
            ViewBag.comunidad = new SelectList(db.Comunidad.Where(c => c.estado == 1 || c.estado == 2), "idComunidad", "nombre", producto.comunidad);
            return View(producto);
        }

        // GET: Productoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.catProd = new SelectList(db.CategoriaProducto.Where(o => o.estado == 1 || o.estado == 2), "idCatProd", "nombre", producto.catProd);
            ViewBag.comunidad = new SelectList(db.Comunidad.Where(c => c.estado == 1 || c.estado == 2), "idComunidad", "nombre", producto.comunidad);
            return View(producto);
        }

        // POST: Productoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProducto,comunidad,catProd,nombre,descripcion,cantidad,precioVSugerido,estado")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                producto.estado = 2;
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.catProd = new SelectList(db.CategoriaProducto.Where(o => o.estado == 1 || o.estado == 2), "idCatProd", "nombre", producto.catProd);
            ViewBag.comunidad = new SelectList(db.Comunidad.Where(c => c.estado == 1 || c.estado == 2), "idComunidad", "nombre", producto.comunidad);
            return View(producto);
        }

        // GET: Productoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vw_producto producto = db.vw_producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Productoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Producto.Find(id);
            producto.estado = 3;
            db.Entry(producto).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult VerReporteProducto(string tipo, string buscar = "")
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RProducto.rdlc");

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            List<vw_producto> lista = new List<vw_producto>();
            var producto = from p in db.vw_producto select p;

            if (!string.IsNullOrEmpty(buscar))
            {
                producto = producto.Where(p => p.nombre.Contains(buscar) || p.descripcion.Contains(buscar));
            }

            lista = producto.ToList();

            ReportDataSource rds = new ReportDataSource("DsProducto", lista);
            rpt.DataSources.Add(rds);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return File(b, mt);
        }

        public ActionResult VerReporteProductoIndiv(int id)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RProductoIndiv.rdlc");

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            var producto = from p in db.vw_producto select p;
            producto = producto.Where(o => o.idProducto == id);

            ReportDataSource rds = new ReportDataSource("DsProducto", producto.ToList());
            rpt.DataSources.Add(rds);

            var b = rpt.Render("PDF", null, out mt, out enc, out f, out s, out w);

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
