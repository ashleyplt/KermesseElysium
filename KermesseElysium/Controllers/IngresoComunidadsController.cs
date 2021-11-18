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
    public class IngresoComunidadsController : Controller
    {
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();

        // GET: IngresoComunidads
        public ActionResult Index(string buscar = "")
        {
            var ingresoComunidad = from ic in db.vw_ingresocomunidad select ic;
            ingresoComunidad = ingresoComunidad.Where(ic => ic.fechaEliminacion.Equals(null) && ic.usuarioEliminacion.Equals(null));

            if(!string.IsNullOrEmpty(buscar))
            {
                ingresoComunidad = ingresoComunidad.Where(ic => ic.comunidad.Contains(buscar) || ic.kermesse.Contains(buscar) || ic.producto.Contains(buscar));
            }
            return View(ingresoComunidad.ToList());
        }

        // GET: IngresoComunidads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vw_ingresocomunidad ingresoComunidad = db.vw_ingresocomunidad.Find(id);
            if (ingresoComunidad == null)
            {
                return HttpNotFound();
            }
            return View(ingresoComunidad);
        }

        // GET: IngresoComunidads/Create
        public ActionResult Create()
        {
            ViewBag.comunidad = new SelectList(db.Comunidad.Where(c => c.estado == 1 || c.estado == 2), "idComunidad", "nombre");
            ViewBag.kermesse = new SelectList(db.Kermesse.Where(k => k.fechaEliminacion.Equals(null) && k.usuarioEliminacion.Equals(null)), "idKermesse", "nombre");
            ViewBag.producto = new SelectList(db.Producto.Where(p => p.estado == 1 || p.estado == 2), "idProducto", "nombre");
            ViewBag.usuarioCreacion = new SelectList(db.Usuario.Where(u => u.estado == 1 || u.estado == 2), "idUsuario", "userName");
            ViewBag.usuarioModificacion = new SelectList(db.Usuario.Where(u => u.estado == 1 || u.estado == 2), "idUsuario", "userName");
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario.Where(u => u.estado == 1 || u.estado == 2), "idUsuario", "userName");
            return View();
        }

        // POST: IngresoComunidads/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IngresoComunidad ingresoComunidad)
        {
            if (ModelState.IsValid)
            {
                var ic = new IngresoComunidad();
                ic.producto = ingresoComunidad.producto;
                ic.kermesse = ingresoComunidad.kermesse;
                ic.comunidad = ingresoComunidad.comunidad;
                ic.cantProducto = ingresoComunidad.cantProducto;
                ic.totalBonos = ingresoComunidad.totalBonos;
                ic.usuarioCreacion = (int)Session["idUser"];
                ic.fechaCreacion = DateTime.Now;
                db.IngresoComunidad.Add(ic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.comunidad = new SelectList(db.Comunidad.Where(c => c.estado == 1 || c.estado == 2), "idComunidad", "nombre", ingresoComunidad.comunidad);
            ViewBag.kermesse = new SelectList(db.Kermesse.Where(k => k.fechaEliminacion.Equals(null) && k.usuarioEliminacion.Equals(null)), "idKermesse", "nombre", ingresoComunidad.kermesse);
            ViewBag.producto = new SelectList(db.Producto.Where(p => p.estado == 1 || p.estado == 2), "idProducto", "nombre", ingresoComunidad.producto);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario.Where(u => u.estado == 1 || u.estado == 2), "idUsuario", "userName", ingresoComunidad.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario.Where(u => u.estado == 1 || u.estado == 2), "idUsuario", "userName", ingresoComunidad.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario.Where(u => u.estado == 1 || u.estado == 2), "idUsuario", "userName", ingresoComunidad.usuarioEliminacion);
            return View(ingresoComunidad);
        }

        // GET: IngresoComunidads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IngresoComunidad ingresoComunidad = db.IngresoComunidad.Find(id);
            if (ingresoComunidad == null)
            {
                return HttpNotFound();
            }
            ViewBag.comunidad = new SelectList(db.Comunidad, "idComunidad", "nombre", ingresoComunidad.comunidad);
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", ingresoComunidad.kermesse);
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre", ingresoComunidad.producto);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioEliminacion);
            return View(ingresoComunidad);
        }

        // POST: IngresoComunidads/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idIngresoComunidad,kermesse,comunidad,producto,cantProducto,totalBonos,usuarioCreacion,fechaCreacion,usuarioModificacion,fechaModificacion,usuarioEliminacion,fechaEliminacion")] IngresoComunidad ingresoComunidad)
        {
            if (ModelState.IsValid)
            {
                ingresoComunidad.usuarioModificacion = (int)Session["idUser"];
                ingresoComunidad.fechaModificacion = DateTime.Now;

                db.Entry(ingresoComunidad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.comunidad = new SelectList(db.Comunidad, "idComunidad", "nombre", ingresoComunidad.comunidad);
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", ingresoComunidad.kermesse);
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre", ingresoComunidad.producto);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", ingresoComunidad.usuarioEliminacion);
            return View(ingresoComunidad);
        }

        // GET: IngresoComunidads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vw_ingresocomunidad ingresoComunidad = db.vw_ingresocomunidad.Find(id);
            if (ingresoComunidad == null)
            {
                return HttpNotFound();
            }
            return View(ingresoComunidad);
        }

        // POST: IngresoComunidads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IngresoComunidad ingresoComunidad = db.IngresoComunidad.Find(id);
            ingresoComunidad.usuarioEliminacion = (int)Session["idUser"];
            ingresoComunidad.fechaEliminacion = DateTime.Now;
            db.Entry(ingresoComunidad).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult VerReporteIngresoComunidad(string tipo, string buscar = "")
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RIngresoComunidad.rdlc");

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            List<vw_ingresocomunidad> lista = new List<vw_ingresocomunidad>();
            var ingcomunidad = from ro in db.vw_ingresocomunidad select ro;

            if (!string.IsNullOrEmpty(buscar))
            {
                ingcomunidad = ingcomunidad.Where(ic => ic.comunidad.Contains(buscar) || ic.kermesse.Contains(buscar) || ic.producto.Contains(buscar));
            }

            lista = ingcomunidad.ToList();

            ReportDataSource rds = new ReportDataSource("DSIngresoComunidad", lista);
            rpt.DataSources.Add(rds);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return File(b, mt);
        }

        public ActionResult VerReporteIngresoComunidadIndiv(int id)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RIngresoComunidadIndiv.rdlc");

            rpt.ReportPath = ruta;

            var ingcomunidad = from ro in db.vw_ingresocomunidad select ro;
            ingcomunidad = ingcomunidad.Where(ic => ic.idIngresoComunidad == id);

            ReportDataSource rds = new ReportDataSource("DSIngresoComunidad", ingcomunidad.ToList());
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
