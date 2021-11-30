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

    public partial class ArqueoCajaDet
    {
        [Display(Name = "Id Arqueo Caja Det")]
        public int idArqueoCajaDet { get; set; }

        [Display(Name = "Arqueo Caja")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public int arqueoCaja { get; set; }

        [Display(Name = "Moneda")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public int moneda { get; set; }

        [Display(Name = "Denominación")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public int denominacion { get; set; }

        [Display(Name = "Cantidad")]
        [DataType(DataType.Currency, ErrorMessage = "Por favor ingres un dato de tipo decimal")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public decimal cantidad { get; set; }

        [Display(Name = "Subtotal")]
        [DataType(DataType.Currency, ErrorMessage = "Por favor ingres un dato de tipo decimal")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public decimal subtotal { get; set; }
    
        public virtual ArqueoCaja ArqueoCaja1 { get; set; }
        public virtual Denominacion Denominacion1 { get; set; }
        public virtual Moneda Moneda1 { get; set; }
    }
}
