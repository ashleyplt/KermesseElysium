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
    
    public partial class vw_ingresocomunidad
    {
        public int idIngresoComunidad { get; set; }
        public string kermesse { get; set; }
        public string comunidad { get; set; }
        public string producto { get; set; }
        public int cantProducto { get; set; }
        public int totalBonos { get; set; }
        public string usuarioCreacion { get; set; }
        public System.DateTime fechaCreacion { get; set; }
        public string usuarioModificacion { get; set; }
        public Nullable<System.DateTime> fechaModificacion { get; set; }
        public string usuarioEliminacion { get; set; }
        public Nullable<System.DateTime> fechaEliminacion { get; set; }
    }
}