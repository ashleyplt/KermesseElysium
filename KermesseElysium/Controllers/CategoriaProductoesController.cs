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
    public class CategoriaProductoesController : Controller
    {
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();

        // GET: CategoriaProductoes
        public ActionResult Index()
        {
            return View(db.CategoriaProducto.ToList());
        }

        // GET: CategoriaProductoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaProducto categoriaProducto = db.CategoriaProducto.Find(id);
            if (categoriaProducto == null)
            {
                return HttpNotFound();
            }
            return View(categoriaProducto);
        }

        // GET: CategoriaProductoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriaProductoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoriaProducto categoriaProducto)
        {
            if (ModelState.IsValid)
            {
                var cp = new CategoriaProducto();
                cp.idCatProd = 0;
                cp.nombre = categoriaProducto.nombre;
                cp.descripcion = categoriaProducto.descripcion;
                cp.estado = 1;
                db.CategoriaProducto.Add(cp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categoriaProducto);
        }

        // GET: CategoriaProductoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaProducto categoriaProducto = db.CategoriaProducto.Find(id);
            if (categoriaProducto == null)
            {
                return HttpNotFound();
            }
            return View(categoriaProducto);
        }

        // POST: CategoriaProductoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoriaProducto categoriaProducto)
        {
            if (ModelState.IsValid)
            {
                var cp = new CategoriaProducto();
                cp.idCatProd = categoriaProducto.idCatProd;
                cp.nombre = categoriaProducto.nombre;
                cp.descripcion = categoriaProducto.descripcion;
                cp.estado = 2;
                db.Entry(cp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoriaProducto);
        }

        // GET: CategoriaProductoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaProducto categoriaProducto = db.CategoriaProducto.Find(id);
            if (categoriaProducto == null)
            {
                return HttpNotFound();
            }
            return View(categoriaProducto);
        }

        // POST: CategoriaProductoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CategoriaProducto categoriaProducto = db.CategoriaProducto.Find(id);
            categoriaProducto.estado = 3;
            db.Entry(categoriaProducto).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult VerReporteCatProducto(string tipo)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RCatProducto.rdlc");

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            List<CategoriaProducto> listaProduct = new List<CategoriaProducto>();
            listaProduct = modelo.CategoriaProducto.ToList();

            ReportDataSource rds = new ReportDataSource("DSCatProducto", listaProduct);
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
