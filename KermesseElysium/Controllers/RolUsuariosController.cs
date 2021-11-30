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
    public class RolUsuariosController : Controller
    {
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();

        // GET: RolUsuarios
        public ActionResult Index(string buscar = "")
        {
            var usuarioRol = from ur in db.vw_rolusuario select ur;
            if (!string.IsNullOrEmpty(buscar))
            {
                usuarioRol = usuarioRol.Where(ur => ur.nombres.Contains(buscar) || ur.rolDescripcion.Contains(buscar));
            }
            return View(usuarioRol.ToList());
        }

        // GET: RolUsuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vw_rolusuario usuarioRol = db.vw_rolusuario.Find(id);
            if (usuarioRol == null)
            {
                return HttpNotFound();
            }
            return View(usuarioRol);
        }

        // GET: RolUsuarios/Create
        public ActionResult Create()
        {
            ViewBag.rol = new SelectList(db.Rol.Where(r => r.estado == 1 || r.estado == 2), "idRol", "rolDescripcion");
            ViewBag.usuario = new SelectList(db.Usuario.Where(u => u.estado == 1 || u.estado == 2), "idUsuario", "nombres");
            return View();
        }

        // POST: RolUsuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idRolUsuario,usuario,rol")] RolUsuario rolUsuario)
        {
            if (ModelState.IsValid)
            {
                var existe = (from d in db.RolUsuario where d.rol == rolUsuario.rol && d.usuario == rolUsuario.usuario select d).FirstOrDefault();

                if (existe == null)

                {
                    db.RolUsuario.Add(rolUsuario);
                db.SaveChanges();
                return RedirectToAction("Index");
                }
                else
                {

                    return RedirectToAction("Index");
                }
            }

            ViewBag.rol = new SelectList(db.Rol.Where(r => r.estado == 1 || r.estado == 2), "idRol", "rolDescripcion", rolUsuario.rol);
            ViewBag.usuario = new SelectList(db.Usuario.Where(u => u.estado == 1 || u.estado == 2), "idUsuario", "nombres", rolUsuario.usuario);
            return View(rolUsuario);
        }

        // GET: RolUsuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolUsuario rolUsuario = db.RolUsuario.Find(id);
            if (rolUsuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.rol = new SelectList(db.Rol.Where(r => r.estado == 1 || r.estado == 2), "idRol", "rolDescripcion", rolUsuario.rol);
            ViewBag.usuario = new SelectList(db.Usuario.Where(u => u.estado == 1 || u.estado == 2), "idUsuario", "nombres", rolUsuario.usuario);
            return View(rolUsuario);
        }

        // POST: RolUsuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idRolUsuario,usuario,rol")] RolUsuario rolUsuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rolUsuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.rol = new SelectList(db.Rol.Where(r => r.estado == 1 || r.estado == 2), "idRol", "rolDescripcion", rolUsuario.rol);
            ViewBag.usuario = new SelectList(db.Usuario.Where(u => u.estado == 1 || u.estado == 2), "idUsuario", "nombres", rolUsuario.usuario);
            return View(rolUsuario);
        }

        // GET: RolUsuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            vw_rolusuario rolUsuario = db.vw_rolusuario.Find(id);
            if (rolUsuario == null)
            {
                return HttpNotFound();
            }
            return View(rolUsuario);
        }

        // POST: RolUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                RolUsuario rolUsuario = db.RolUsuario.Find(id);
                db.RolUsuario.Remove(rolUsuario);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public ActionResult VerReporteRolUsuario(string tipo, string buscar = "")
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RRolUsuario.rdlc");

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            List<vw_rolusuario> lista = new List<vw_rolusuario>();
            var rolusuario = from ru in db.vw_rolusuario select ru;

            if (!string.IsNullOrEmpty(buscar))
            {
                rolusuario = rolusuario.Where(ro => ro.nombres.Contains(buscar) || ro.rolDescripcion.Contains(buscar));
            }

            lista = rolusuario.ToList();

            ReportDataSource rds = new ReportDataSource("DSRolUsuario", lista);
            rpt.DataSources.Add(rds);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return File(b, mt);
        }

        public ActionResult VerReporteRolUsuarioIndiv(int id)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RRolUsuarioIndiv.rdlc");

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            var rolusuario = from ru in db.vw_rolusuario select ru;
            rolusuario = rolusuario.Where(ru => ru.idRolUsuario == id);

            ReportDataSource rds = new ReportDataSource("DSRolUsuarioIndiv", rolusuario.ToList());
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
