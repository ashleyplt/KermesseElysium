using KermesseElysium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KermesseElysium.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        private DBKermesseElysiumEntities db = new DBKermesseElysiumEntities();

        public ActionResult Borrar()
        {
            Session["User"] = null;
            return RedirectToAction("Login");
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string User, string Pass)
        {
            try
            {
                using (Models.DBKermesseElysiumEntities db = new Models.DBKermesseElysiumEntities())
                {
                    var oUser = (from d in db.Usuario
                                 where d.email == User.Trim() && d.pwd == Pass.Trim()
                                 select d).FirstOrDefault();
                    if (oUser == null)
                    {
                        ViewBag.Error = "Usuario o contraseña incorrecta.";
                        return View();
                    }

                    Session["User"] = oUser;
                    System.Web.HttpContext.Current.Session["idUser"] = oUser.idUsuario;
                    String temp = oUser.nombres + " " + oUser.apellidos;
                    System.Web.HttpContext.Current.Session["Username"] = temp;
                    // ViewBag.Data = oUser.nombres + " " + oUser.apellidos;
                }
                return RedirectToAction("index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }



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
                return RedirectToAction("~/Login/Login");
            }

            return RedirectToAction("~/Login/Login");
        }

    }
}