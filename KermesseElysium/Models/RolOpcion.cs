//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KermesseElysium.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class RolOpcion
    {
        [Display(Name = "ID Rol Opción")]
        public int idRolOpcion { get; set; }

        [Display(Name = "Rol")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public int rol { get; set; }

        [Display(Name = "Opción")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public int opcion { get; set; }

        public virtual Opcion Opcion1 { get; set; }
        public virtual Rol Rol1 { get; set; }
    }
}
