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

    public partial class IngresoComunidad
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IngresoComunidad()
        {
            this.IngresoComunidadDet = new HashSet<IngresoComunidadDet>();
        }

        [Display(Name = "ID")]
        public int idIngresoComunidad { get; set; }

        [Display(Name = "Kermesse")]
        public Nullable<int> kermesse { get; set; }

        [Display(Name = "Comunidad")]
        public Nullable<int> comunidad { get; set; }

        [Display(Name = "Producto")]
        public Nullable<int> producto { get; set; }

        [Display(Name = "Cantidad de producto")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public int cantProducto { get; set; }

        [Display(Name = "Total de bonos")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public int totalBonos { get; set; }

        [Display(Name = "Usuario Creación")]
        [Required(ErrorMessage = "Ingrese un dato válido")]
        public int usuarioCreacion { get; set; }

        [Display(Name = "Fecha Creación")]
        [DataType(DataType.DateTime, ErrorMessage = "Ingrese una fecha válida")]
        [Required(ErrorMessage = "Ingrese un dato válido")]
        public System.DateTime fechaCreacion { get; set; }

        [Display(Name = "Usuario Modificación")]
        public Nullable<int> usuarioModificacion { get; set; }

        [Display(Name = "Fecha Modificación")]
        [DataType(DataType.DateTime, ErrorMessage = "Ingrese una fecha válida")]
        public Nullable<System.DateTime> fechaModificacion { get; set; }

        [Display(Name = "Usuario Eliminación")]
        public Nullable<int> usuarioEliminacion { get; set; }

        [Display(Name = "Fecha Eliminación")]
        [DataType(DataType.DateTime, ErrorMessage = "Ingrese una fecha válida")]
        public Nullable<System.DateTime> fechaEliminacion { get; set; }

        public virtual Comunidad Comunidad1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IngresoComunidadDet> IngresoComunidadDet { get; set; }
        public virtual Kermesse Kermesse1 { get; set; }
        public virtual Producto Producto1 { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Usuario Usuario1 { get; set; }
        public virtual Usuario Usuario2 { get; set; }
    }
}
