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
    public class ListaPreciosController : Controller
    {
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();

        // GET: ListaPrecios
        public ActionResult Index()
        {
            var listaprecio = from lp in db.ListaPrecio select lp;

            listaprecio = listaprecio.Where(lp => lp.estado.Equals(2) || lp.estado.Equals(1));

            return View(listaprecio.ToList());
            //var listaPrecio = db.ListaPrecio.Include(l => l.Kermesse1);
            //return View(listaPrecio.ToList());
        }

        // GET: ListaPrecios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListaPrecio listaPrecio = db.ListaPrecio.Find(id);
            if (listaPrecio == null)
            {
                return HttpNotFound();
            }
            return View(listaPrecio);
        }

        // GET: ListaPrecios/Create
        public ActionResult Create()
        {
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre");
            return View();
        }

        // POST: ListaPrecios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ListaPrecio listaPrecio)
        {
            if (ModelState.IsValid)
            {
                var lp = new ListaPrecio();
                lp.idListaPrecio = 0;
                lp.nombre = listaPrecio.nombre;
                lp.descripcion = listaPrecio.descripcion;
                lp.estado = 1;
                lp.Kermesse1.idKermesse = listaPrecio.Kermesse1.idKermesse;

                db.ListaPrecio.Add(lp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", listaPrecio.kermesse);
            return View(listaPrecio);
        }

        // GET: ListaPrecios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListaPrecio listaPrecio = db.ListaPrecio.Find(id);
            if (listaPrecio == null)
            {
                return HttpNotFound();
            }
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", listaPrecio.kermesse);
            return View(listaPrecio);
        }

        // POST: ListaPrecios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ListaPrecio listaPrecio)
        {
            if (ModelState.IsValid)
            {
                var lp = new ListaPrecio();
                lp.idListaPrecio = listaPrecio.idListaPrecio;
                lp.nombre = listaPrecio.nombre;
                lp.descripcion = listaPrecio.descripcion;
                lp.estado = 2;
                lp.Kermesse1.idKermesse = listaPrecio.Kermesse1.idKermesse;

                db.Entry(lp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", listaPrecio.kermesse);
            return View(listaPrecio);
        }

        // GET: ListaPrecios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListaPrecio listaPrecio = db.ListaPrecio.Find(id);
            if (listaPrecio == null)
            {
                return HttpNotFound();
            }
            return View(listaPrecio);
        }

        // POST: ListaPrecios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ListaPrecio listaPrecio = db.ListaPrecio.Find(id);

            listaPrecio.estado = 3;

            db.Entry(listaPrecio).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult VerReporteListaPrecio(string tipo)
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RListaPrecio.rdlc");

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            List<ListaPrecio> listaPrec = new List<ListaPrecio>();
            var listaprecio = from lp in db.ListaPrecio select lp;
            listaprecio = listaprecio.Where(lp => lp.estado.Equals(2) || lp.estado.Equals(1));
            listaPrec = listaprecio.ToList();

            ReportDataSource rds = new ReportDataSource("DSListaPrecio", listaPrec);
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
