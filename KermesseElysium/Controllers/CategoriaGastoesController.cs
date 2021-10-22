using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KermesseElysium.Models;
using Microsoft.Reporting.WebForms;

namespace KermesseElysium.Controllers
{
    public class CategoriaGastoesController : Controller
    {
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();

        // GET: CategoriaGastoes
        public ActionResult Index()
        {
            return View(db.CategoriaGasto.ToList());
        }

        // GET: CategoriaGastoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaGasto categoriaGasto = db.CategoriaGasto.Find(id);
            if (categoriaGasto == null)
            {
                return HttpNotFound();
            }
            return View(categoriaGasto);
        }

        // GET: CategoriaGastoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriaGastoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoriaGasto categoriaGasto)
        {
            if (ModelState.IsValid)
            {
                var cg = new CategoriaGasto();
                cg.idCatGasto = 0;
                cg.nombreCategoria = categoriaGasto.nombreCategoria;
                cg.descripcion = categoriaGasto.descripcion;
                cg.estado = 1;

                db.CategoriaGasto.Add(cg);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categoriaGasto);
        }

        // GET: CategoriaGastoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaGasto categoriaGasto = db.CategoriaGasto.Find(id);
            if (categoriaGasto == null)
            {
                return HttpNotFound();
            }
            return View(categoriaGasto);
        }

        // POST: CategoriaGastoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoriaGasto categoriaGasto)
        {
            if (ModelState.IsValid)
            {
                var cg = new CategoriaGasto();
                cg.idCatGasto = categoriaGasto.idCatGasto;
                cg.nombreCategoria = categoriaGasto.nombreCategoria;
                cg.descripcion = categoriaGasto.descripcion;
                cg.estado = 2;

                db.Entry(cg).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoriaGasto);
        }

        // GET: CategoriaGastoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaGasto categoriaGasto = db.CategoriaGasto.Find(id);
            if (categoriaGasto == null)
            {
                return HttpNotFound();
            }
            return View(categoriaGasto);
        }

        // POST: CategoriaGastoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CategoriaGasto categoriaGasto = db.CategoriaGasto.Find(id);
            categoriaGasto.estado = 3;
            db.Entry(categoriaGasto).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ReporteGastos(string tipo)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = System.IO.Path.Combine(Server.MapPath("~/Reportes"), "RGastos.rdlc");

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            List<CategoriaGasto> listacat = new List<CategoriaGasto>();
            listacat = modelo.CategoriaGasto.ToList();

            ReportDataSource rds = new ReportDataSource("DSGastos", listacat);
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
