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
    public class UsuariosController : Controller
    {
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();

        // GET: Usuarios
        public ActionResult Index(string buscar = "")
        {
            var usuario = from u in db.Usuario select u;
            usuario = usuario.Where(u => u.estado.Equals(2) || u.estado.Equals(1));

            if (!string.IsNullOrEmpty(buscar))
            {
                usuario = usuario.Where(us => us.nombres.Contains(buscar) || us.apellidos.Contains(buscar) || us.email.Contains(buscar) || us.userName.Contains(buscar));
            }

            return View(usuario.ToList());
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var u = new Usuario();
                u.idUsuario = 0;
                u.userName = usuario.userName;
                u.pwd = usuario.pwd;
                u.nombres = usuario.nombres;
                u.apellidos = usuario.apellidos;
                u.email = usuario.email;
                u.estado = 1;
                db.Usuario.Add(u);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var u = new Usuario();
                u.idUsuario = usuario.idUsuario;
                u.userName = usuario.userName;
                u.pwd = usuario.pwd;
                u.nombres = usuario.nombres;
                u.apellidos = usuario.apellidos;
                u.email = usuario.email;
                u.estado = 2;
                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuario.Find(id);
            usuario.estado = 3;
            db.Entry(usuario).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult VerReporteUsuario(string tipo, string buscar = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RUsuario.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <MarginTop>0cm</MarginTop>
                      <MarginLeft>0cm</MarginLeft>
                      <MarginRight>0cm</MarginRight>
                      <MarginBottom>0cm</MarginBottom>
                        <EmbedFonts>None</EmbedFonts>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            List<Usuario> listaUser = new List<Usuario>();
            var usuario = from u in db.Usuario select u;
            usuario = usuario.Where(u => u.estado.Equals(2) || u.estado.Equals(1));

            if (!string.IsNullOrEmpty(buscar))
            {
                usuario = usuario.Where(us => us.nombres.Contains(buscar) || us.apellidos.Contains(buscar) || us.email.Contains(buscar) || us.userName.Contains(buscar));
            }

            listaUser = usuario.ToList();

            ReportDataSource rds = new ReportDataSource("DSUsuario", listaUser);
            rpt.DataSources.Add(rds);

            var b = rpt.Render(tipo, deviceInfo, out mt, out enc, out f, out s, out w);

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
