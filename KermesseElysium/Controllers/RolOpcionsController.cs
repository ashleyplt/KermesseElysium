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
    public class RolOpcionsController : Controller
    {
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();

        // GET: RolOpcions
        public ActionResult Index(string buscar = "")
        {
            var rolOpcion = from ro in db.vw_rolopcion select ro;

            if (!string.IsNullOrEmpty(buscar))
            {
                rolOpcion = rolOpcion.Where(ro => ro.opcionDescripcion.Contains(buscar) || ro.rolDescripcion.Contains(buscar));
            }

            return View(rolOpcion.ToList());
        }

        // GET: RolOpcions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vw_rolopcion rolOpcion = db.vw_rolopcion.Find(id);
            if (rolOpcion == null)
            {
                return HttpNotFound();
            }
            return View(rolOpcion);
        }

        // GET: RolOpcions/Create
        public ActionResult Create()
        {
            ViewBag.opcion = new SelectList(db.Opcion.Where(o => o.estado == 1 || o.estado == 2), "idOpcion", "opcionDescripcion");
            ViewBag.rol = new SelectList(db.Rol.Where(r => r.estado == 1 || r.estado == 2), "idRol", "rolDescripcion");
            return View();
        }

        // POST: RolOpcions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idRolOpcion,rol,opcion")] RolOpcion rolOpcion)
        {
            if (ModelState.IsValid)
            {

                var existe = (from d in db.RolOpcion  where d.rol == rolOpcion.rol  && d.opcion == rolOpcion.opcion  select d).FirstOrDefault();
                                
                if (existe == null)
                                
                {         
                    db.RolOpcion.Add(rolOpcion);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {

                    return RedirectToAction("Index");
                }
            }

            ViewBag.opcion = new SelectList(db.Opcion.Where(o => o.estado == 1 || o.estado == 2), "idOpcion", "opcionDescripcion", rolOpcion.opcion);
            ViewBag.rol = new SelectList(db.Rol.Where(r => r.estado == 1 || r.estado == 2), "idRol", "rolDescripcion", rolOpcion.rol);
            return View(rolOpcion);
        }

        // GET: RolOpcions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolOpcion rolOpcion = db.RolOpcion.Find(id);
            if (rolOpcion == null)
            {
                return HttpNotFound();
            }
            ViewBag.opcion = new SelectList(db.Opcion.Where(o => o.estado == 1 || o.estado == 2), "idOpcion", "opcionDescripcion", rolOpcion.opcion);
            ViewBag.rol = new SelectList(db.Rol.Where(r => r.estado == 1 || r.estado == 2), "idRol", "rolDescripcion", rolOpcion.rol);
            return View(rolOpcion);
        }

        // POST: RolOpcions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idRolOpcion,rol,opcion")] RolOpcion rolOpcion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rolOpcion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.opcion = new SelectList(db.Opcion.Where(o => o.estado == 1 || o.estado == 2), "idOpcion", "opcionDescripcion", rolOpcion.opcion);
            ViewBag.rol = new SelectList(db.Rol.Where(r => r.estado == 1 || r.estado == 2), "idRol", "rolDescripcion", rolOpcion.rol);
            return View(rolOpcion);
        }

        // GET: RolOpcions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vw_rolopcion rolOpcion = db.vw_rolopcion.Find(id);
            if (rolOpcion == null)
            {
                return HttpNotFound();
            }
            return View(rolOpcion);
        }

        // POST: RolOpcions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                RolOpcion rolOpcion = db.RolOpcion.Find(id);
                db.RolOpcion.Remove(rolOpcion);
                db.SaveChanges();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public ActionResult VerReporteRolOpcion(string tipo, string buscar = "")
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RRolOpcion.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <MarginTop>0cm</MarginTop>
                      <MarginLeft>0cm</MarginLeft>
                      <MarginRight>0cm</MarginRight>
                      <MarginBottom>0cm</MarginBottom>
                        <EmbedFonts>None</EmbedFonts>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            List<vw_rolopcion> lista = new List<vw_rolopcion>();
            var rolopcion = from ro in db.vw_rolopcion select ro;

            if (!string.IsNullOrEmpty(buscar))
            {
                rolopcion = rolopcion.Where(ro => ro.opcionDescripcion.Contains(buscar) || ro.rolDescripcion.Contains(buscar));
            }

            lista = rolopcion.ToList();

            ReportDataSource rds = new ReportDataSource("DSRolOpcion", lista);
            rpt.DataSources.Add(rds);

            var b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);
        }

        public ActionResult VerReporteRolOpcionIndiv(int id)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RRolOpcionIndiv.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <MarginTop>0cm</MarginTop>
                      <MarginLeft>0cm</MarginLeft>
                      <MarginRight>0cm</MarginRight>
                      <MarginBottom>0cm</MarginBottom>
                        <EmbedFonts>None</EmbedFonts>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            var rolopcion = from ro in db.vw_rolopcion select ro;
            rolopcion = rolopcion.Where(o => o.idRolOpcion == id);

            ReportDataSource rds = new ReportDataSource("DSRolOpcion", rolopcion.ToList());
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
