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

    public partial class ListaPrecio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ListaPrecio()
        {
            this.ListaPrecioDet = new HashSet<ListaPrecioDet>();
        }

        [Display(Name = "Id Lista Precio")]
        public int idListaPrecio { get; set; }

        [Display(Name = "Kermesse")]
        public Nullable<int> kermesse { get; set; }

        [Display(Name = "Nombre")]
        [DataType(DataType.Text, ErrorMessage = "Por favor ingrese un dato de tipo texto")]
        [StringLength(100, ErrorMessage = "Longitud máxima 100")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string nombre { get; set; }

        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText, ErrorMessage = "Por favor ingrese un dato de tipo texto")]
        [StringLength(100, ErrorMessage = "Longitud máxima 100")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string descripcion { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        public int estado { get; set; }
    
        public virtual Kermesse Kermesse1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ListaPrecioDet> ListaPrecioDet { get; set; }
    }
}
