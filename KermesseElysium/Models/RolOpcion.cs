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
    
    public partial class RolOpcion
    {
        public int idRolOpcion { get; set; }
        public int rol { get; set; }
        public int opcion { get; set; }
    
        public virtual Opcion Opcion1 { get; set; }
        public virtual Rol Rol1 { get; set; }
    }
}
