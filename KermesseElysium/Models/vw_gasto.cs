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
    
    public partial class vw_gasto
    {
        public int idGasto { get; set; }
        public string nombre { get; set; }
        public string catGasto { get; set; }
        public System.DateTime fechGasto { get; set; }
        public string concepto { get; set; }
        public double monto { get; set; }
        public string usuarioCreacion { get; set; }
        public System.DateTime fechaCreacion { get; set; }
        public string usuarioModificacion { get; set; }
        public Nullable<System.DateTime> fechaModificacion { get; set; }
        public string usuarioEliminacion { get; set; }
        public Nullable<System.DateTime> fechaEliminacion { get; set; }
    }
}
