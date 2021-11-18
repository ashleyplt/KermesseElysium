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
    public class GastoesController : Controller
    {
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();

        // GET: Gastoes
        public ActionResult Index(string buscar = "")
        {
            var gasto = from g in db.Gasto select g;

            if (!string.IsNullOrEmpty(buscar))
            {
                gasto = gasto.Where(g => g.concepto.Contains(buscar));
            }

            return View(gasto.ToList());
        }

        // GET: Gastoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gasto gasto = db.Gasto.Find(id);
            if (gasto == null)
            {
                return HttpNotFound();
            }
            return View(gasto);
        }

        // GET: Gastoes/Create
        public ActionResult Create()
        {
            ViewBag.catGasto = new SelectList(db.CategoriaGasto, "idCatGasto", "nombreCategoria");
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre");
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName");
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName");
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName");
            return View();
        }

        // POST: Gastoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Gasto g)
        {
            if (ModelState.IsValid)
            {
                String temp = Convert.ToString(Session["idUser"]);
                int idu = int.Parse(temp);
                var gasto = new Gasto();
                gasto.idGasto = 0;
                gasto.usuarioCreacion = idu;
                gasto.fechaCreacion = DateTime.Now;
                gasto.fechGasto = DateTime.Now;
                gasto.kermesse = g.kermesse;
                gasto.concepto = g.concepto;
                gasto.monto = g.monto;
                gasto.catGasto = g.catGasto;
                db.Gasto.Add(gasto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }



            ViewBag.catGasto = new SelectList(db.CategoriaGasto, "idCatGasto", "nombreCategoria", g.catGasto);
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", g.kermesse);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", g.usuarioCreacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", g.usuarioEliminacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", g.usuarioModificacion);
            return View(g);
        }

        // GET: Gastoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gasto gasto = db.Gasto.Find(id);
            if (gasto == null)
            {
                return HttpNotFound();
            }
            ViewBag.catGasto = new SelectList(db.CategoriaGasto, "idCatGasto", "nombreCategoria", gasto.catGasto);
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", gasto.kermesse);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", gasto.usuarioCreacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", gasto.usuarioEliminacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", gasto.usuarioModificacion);
            return View(gasto);
        }

        // POST: Gastoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Gasto g)
        {
            if (ModelState.IsValid)
            {
                String temp = Convert.ToString(Session["idUser"]);
                int idu = int.Parse(temp);
                var gasto = db.Gasto.Find(g.idGasto);
                gasto.idGasto = g.idGasto;
                gasto.usuarioCreacion = idu;
                gasto.fechaCreacion = DateTime.Now;
                gasto.fechGasto = DateTime.Now;
                gasto.kermesse = g.kermesse;
                gasto.concepto = g.concepto;
                gasto.monto = g.monto;
                gasto.catGasto = g.catGasto;

                db.Entry(gasto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.catGasto = new SelectList(db.CategoriaGasto, "idCatGasto", "nombreCategoria", g.catGasto);
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", g.kermesse);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", g.usuarioCreacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", g.usuarioEliminacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", g.usuarioModificacion);
            return View(g);
        }

        // GET: Gastoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gasto gasto = db.Gasto.Find(id);
            if (gasto == null)
            {
                return HttpNotFound();
            }
            return View(gasto);
        }

        // POST: Gastoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Gasto gasto = db.Gasto.Find(id);
            db.Gasto.Remove(gasto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ReporteGasto(string tipo, string buscar = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RGasto2.rdlc");

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            var Gasto = from m in db.Gasto select m;

            if (!string.IsNullOrEmpty(buscar))
            {
                Gasto = Gasto.Where(m => m.concepto.Contains(buscar));
            }

            List<Gasto> listaGas = new List<Gasto>();
            listaGas = Gasto.ToList();

            ReportDataSource rds = new ReportDataSource("DSGasto2", listaGas);
            rpt.DataSources.Add(rds);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return File(b, mt);
        }
        public ActionResult ReporteGastoIndiv(int id)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RGastoIndividual2.rdlc");

            rpt.ReportPath = ruta;

            var Gasto = from m in db.Gasto select m;
            Gasto = Gasto.Where(m => m.idGasto == id);

            ReportDataSource rds = new ReportDataSource("DSGasto2", Gasto.ToList());
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
