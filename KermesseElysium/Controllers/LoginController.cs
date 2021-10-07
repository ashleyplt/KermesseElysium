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
                        ViewBag.Error = "Usuario o Contraseña Incorrecto";
                        return View();
                    }

                    Session["User"] = oUser;
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


    }
}