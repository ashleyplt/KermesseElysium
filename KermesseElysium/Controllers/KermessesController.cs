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
    public class KermessesController : Controller
    {
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();

        // GET: Kermesses1
        public ActionResult Index(string buscar = "")
        {
            var kermesse = from g in db.Kermesse select g;

            kermesse = kermesse.Where(p => p.estado.Equals(2) || p.estado.Equals(1));


            if (!string.IsNullOrEmpty(buscar))
            {
                kermesse = kermesse.Where(g => g.nombre.Contains(buscar));
            }

            return View(kermesse.ToList());
        }
        // GET: Kermesses1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kermesse kermesse = db.Kermesse.Find(id);
            if (kermesse == null)
            {
                return HttpNotFound();
            }
            return View(kermesse);
        }

        // GET: Kermesses1/Create
        public ActionResult Create()
        {
            ViewBag.parroquia = new SelectList(db.Parroquia, "idParroquia", "nombre");
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName");
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName");
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName");
            return View();
        }

        // POST: Kermesses1/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idKermesse,parroquia,nombre,fInicio,fFinal,descripcion,estado,usuarioCreacion,fechaCreacion,usuarioModificacion,fechaModificacion,usuarioEliminacion,fechaEliminacion")] Kermesse kermesse)
        {
            if (ModelState.IsValid)
            {
                String temp = Convert.ToString(Session["idUser"]);
                int idu = int.Parse(temp);
                kermesse.fechaCreacion =  DateTime.Today;
                kermesse.estado = 1;
                kermesse.usuarioCreacion = idu;
                db.Kermesse.Add(kermesse);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.parroquia = new SelectList(db.Parroquia, "idParroquia", "nombre", kermesse.parroquia);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", kermesse.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", kermesse.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", kermesse.usuarioEliminacion);
            return View(kermesse);
        }

        // GET: Kermesses1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kermesse kermesse = db.Kermesse.Find(id);
            if (kermesse == null)
            {
                return HttpNotFound();
            }
            ViewBag.parroquia = new SelectList(db.Parroquia, "idParroquia", "nombre", kermesse.parroquia);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", kermesse.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", kermesse.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", kermesse.usuarioEliminacion);
            return View(kermesse);
        }

        // POST: Kermesses1/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idKermesse,parroquia,nombre,fInicio,fFinal,descripcion,estado,usuarioCreacion,fechaCreacion,usuarioModificacion,fechaModificacion,usuarioEliminacion,fechaEliminacion")] Kermesse kermesse)
        {
            if (ModelState.IsValid)
            {
                String temp = Convert.ToString(Session["idUser"]);
                int idu = int.Parse(temp);
                kermesse.fechaCreacion = DateTime.Today;
                kermesse.estado = 1;
                kermesse.usuarioCreacion = idu;
                db.Entry(kermesse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.parroquia = new SelectList(db.Parroquia, "idParroquia", "nombre", kermesse.parroquia);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", kermesse.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", kermesse.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", kermesse.usuarioEliminacion);
            return View(kermesse);
        }

        // GET: Kermesses1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kermesse kermesse = db.Kermesse.Find(id);
            if (kermesse == null)
            {
                return HttpNotFound();
            }
            return View(kermesse);
        }

        // POST: Kermesses1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kermesse kermesse = db.Kermesse.Find(id);

            kermesse.estado = 3;

            db.Entry(kermesse).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult VerReporteResumen(string buscar = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RReporteResumen.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <MarginTop>0cm</MarginTop>
                      <MarginLeft>0cm</MarginLeft>
                      <MarginRight>0cm</MarginRight>
                      <MarginBottom>0cm</MarginBottom>
                        <EmbedFonts>None</EmbedFonts>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;

            var ingresoComunidad = from ic in db.vw_resumenkermesse select ic;

            if (!string.IsNullOrEmpty(buscar))
            {
                ingresoComunidad = ingresoComunidad.Where(c => c.kermesse.Contains(buscar));
            }

            List<vw_resumenkermesse> listaIngCom = new List<vw_resumenkermesse>();
            listaIngCom = ingresoComunidad.ToList();

            ReportDataSource rds = new ReportDataSource("DsResumenKermesse", listaIngCom);
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render("PDF", deviceInfo, out mt, out enc, out f, out s, out w);

            return File(b, mt);

        }
        public ActionResult VerReporteKermesses(string tipo, string buscar = "")
        {

            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RKermesse.rdlc");

            rpt.ReportPath = ruta;

            DBKermesseElysiumEntities modelo = new DBKermesseElysiumEntities();

            var Lista = from m in db.vw_kermesse select m;

            if (!string.IsNullOrEmpty(buscar))
            {
                Lista = Lista.Where(m => m.nombre.Contains(buscar));
            }

            List<vw_kermesse> listap = new List<vw_kermesse>();
            listap = Lista.ToList();

            ReportDataSource rds = new ReportDataSource("DSKermesse", listap);
            rpt.DataSources.Add(rds);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return File(b, mt);
        }

        public ActionResult VerReporteKermessesIndiv(int id)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RKermesseIndiv.rdlc");
            string deviceInfo = @"<DeviceInfo>
                      <MarginTop>0cm</MarginTop>
                      <MarginLeft>0cm</MarginLeft>
                      <MarginRight>0cm</MarginRight>
                      <MarginBottom>0cm</MarginBottom>
                        <EmbedFonts>None</EmbedFonts>
                    </DeviceInfo>";

            rpt.ReportPath = ruta;


            var Lista = from m in db.vw_kermesse select m;


            Lista = Lista.Where(m => m.idKermesse == id);

            ReportDataSource rds = new ReportDataSource("DSKermesseIndiv", Lista.ToList());
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
