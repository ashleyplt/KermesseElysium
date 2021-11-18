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

    public partial class IngresoComunidadDet
    {
        [Display(Name = "Id Ingreso Comunidad Det")]
        public int idIngresoComunidadDet { get; set; }

        [Display(Name = "Ingreso Comunidad")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public int ingresoComunidad { get; set; }

        [Display(Name = "Bono")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public int bono { get; set; }

        [Display(Name = "Denominación")]
        [DataType(DataType.Text, ErrorMessage = "Por favor ingrese un dato de tipo texto")]
        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(100, ErrorMessage = "Longitud máxima 100")]
        public string denominacion { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public int cantidad { get; set; }

        [Display(Name = "Subtotal Bono")]
        [DataType(DataType.Currency, ErrorMessage = "Por favor ingrese un dato de tipo decimal")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public double subTotalBono { get; set; }

        public virtual ControlBono ControlBono { get; set; }
        public virtual IngresoComunidad IngresoComunidad1 { get; set; }
    }
}
