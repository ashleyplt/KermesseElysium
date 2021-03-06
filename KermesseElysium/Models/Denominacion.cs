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

    public partial class Denominacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Denominacion()
        {
            this.ArqueoCajaDet = new HashSet<ArqueoCajaDet>();
        }

        [Display(Name = "ID Denominación")]
        public int idDenominacion { get; set; }

        [Display(Name = "Moneda")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public int moneda { get; set; }

        [Display(Name = "Valor")]
        [DataType(DataType.Currency, ErrorMessage = "Por favor ingrese un dato de tipo decimal")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public decimal valor { get; set; }

        [Display(Name = "Valor en Letras")]
        [DataType(DataType.Text, ErrorMessage = "Por favor ingrese un dato de tipo texto")]
        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(100, ErrorMessage = "Longitud máxima 100")]
        public string valorLetras { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public int estado { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ArqueoCajaDet> ArqueoCajaDet { get; set; }
        public virtual Moneda Moneda1 { get; set; }
    }
}
