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

    public partial class vw_rolopcion
    {
        [Display(Name = "Rol Descripción")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public string rolDescripcion { get; set; }

        [Display(Name = "Opción Descripción")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public string opcionDescripcion { get; set; }

        
        public int idRolOpcion { get; set; }
    }
}
