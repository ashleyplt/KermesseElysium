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

    public partial class Producto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Producto()
        {
            this.IngresoComunidad = new HashSet<IngresoComunidad>();
            this.ListaPrecioDet = new HashSet<ListaPrecioDet>();
        }

        [Display(Name = "ID Producto")]
        public int idProducto { get; set; }

        [Display(Name = "Comunidad")]
        public Nullable<int> comunidad { get; set; }

        [Display(Name = "Categoría Producto")]
        public Nullable<int> catProd { get; set; }

        [Display(Name = "Nombre Producto")]
        [DataType(DataType.Text, ErrorMessage = "Por favor ingrese un dato de tipo texto")]
        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(45, ErrorMessage = "Longitud máxima 45")]
        public string nombre { get; set; }

        [Display(Name = "Descripción")]
        [DataType(DataType.Text, ErrorMessage = "Por favor ingrese un dato de tipo texto")]
        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(100, ErrorMessage = "Longitud máxima 100")]
        public string descripcion { get; set; }

        [Display(Name = "Cantidad")]
        public Nullable<int> cantidad { get; set; }

        [Display(Name = "Precio Venta Sugerido")]
        [DataType(DataType.Currency, ErrorMessage = "Por favor ingrese un dato de tipo decimal")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public double precioVSugerido { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public int estado { get; set; }

        public virtual CategoriaProducto CategoriaProducto { get; set; }
        public virtual Comunidad Comunidad1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IngresoComunidad> IngresoComunidad { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ListaPrecioDet> ListaPrecioDet { get; set; }
    }
}
