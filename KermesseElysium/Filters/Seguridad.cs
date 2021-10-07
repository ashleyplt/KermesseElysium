using KermesseElysium.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KermesseElysium.Filters
{
    public class Seguridad : ActionFilterAttribute
    {

        private Models.Usuario oUsuario;
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                base.OnActionExecuted(filterContext);

                oUsuario = (Models.Usuario)HttpContext.Current.Session["User"];
                if (oUsuario == null)
                {
                    if (filterContext.Controller is LoginController == false)
                    {
                        filterContext.HttpContext.Response.Redirect("~/Login/Login");
                    }
                }
            }
            catch (Exception)
            {
                filterContext.HttpContext.Response.Redirect("~/Login/Login");
            }


        }

    }
}