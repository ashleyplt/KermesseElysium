using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KermesseElysium.Models;

namespace KermesseElysium.Controllers
{
    public class KermessesController : Controller
    {
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();

        // GET: Kermesses
        public ActionResult Index()
        {
            var kermesse = db.Kermesse.Include(k => k.Parroquia1).Include(k => k.Usuario).Include(k => k.Usuario1).Include(k => k.Usuario2);
            return View(kermesse.ToList());
        }

        // GET: Kermesses/Details/5
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

        // GET: Kermesses/Create
        public ActionResult Create()
        {
            ViewBag.parroquia = new SelectList(db.Parroquia, "idParroquia", "nombre");
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName");
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName");
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName");
            return View();
        }

        // POST: Kermesses/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idKermesse,parroquia,nombre,fInicio,fFinal,descripcion,estado,usuarioCreacion,fechaCreacion,usuarioModificacion,fechaModificacion,usuarioEliminacion,fechaEliminacion")] Kermesse kermesse)
        {
            if (ModelState.IsValid)
            {
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

        // GET: Kermesses/Edit/5
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

        // POST: Kermesses/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idKermesse,parroquia,nombre,fInicio,fFinal,descripcion,estado,usuarioCreacion,fechaCreacion,usuarioModificacion,fechaModificacion,usuarioEliminacion,fechaEliminacion")] Kermesse kermesse)
        {
            if (ModelState.IsValid)
            {
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

        // GET: Kermesses/Delete/5
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

        // POST: Kermesses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kermesse kermesse = db.Kermesse.Find(id);
            db.Kermesse.Remove(kermesse);
            db.SaveChanges();
            return RedirectToAction("Index");
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
