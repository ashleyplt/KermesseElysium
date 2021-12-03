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

    public partial class ArqueoCaja
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ArqueoCaja()
        {
            this.ArqueoCajaDet = new HashSet<ArqueoCajaDet>();
        }

        [Display(Name = "ID Arqueo Caja")]
        public int idArqueoCaja { get; set; }

        [Display(Name = "Kermesse")]
        [Required(ErrorMessage = "Ingrese un dato válido")]
        public int kermesse { get; set; }

        [Display(Name = "Fecha Arqueo")]
        [DataType(DataType.Date, ErrorMessage = "Por favor ingrese una fecha válida")]
        public Nullable<System.DateTime> fechaArqueo { get; set; }

        [Display(Name = "Gran Total")]
        [DataType(DataType.Currency, ErrorMessage = "Por favor ingrese un dato de tipo decimal")]
        public Nullable<decimal> granTotal { get; set; }

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
        [DataType(DataType.DateTime, ErrorMessage = "Ingrese un fecha válida")]
        public Nullable<System.DateTime> fechaModificacion { get; set; }

        [Display(Name = "Usuario Eliminación")]
        public Nullable<int> usuarioEliminacion { get; set; }

        [Display(Name = "Fecha Eliminación")]
        [DataType(DataType.DateTime, ErrorMessage = "Ingrese una fecha válida")]
        public Nullable<System.DateTime> fechaEliminacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ArqueoCajaDet> ArqueoCajaDet { get; set; }
        public virtual Kermesse Kermesse1 { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Usuario Usuario1 { get; set; }
        public virtual Usuario Usuario2 { get; set; }

    }
}
