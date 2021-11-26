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

    public partial class vw_kermesse
    {
        public int idKermesse { get; set; }

        [Display(Name = "Parroquia")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public string parroquia { get; set; }

        [Display(Name = "Nombre Kermesse")]
        [DataType(DataType.Text, ErrorMessage = "Por favor ingrese un dato de tipo texto")]
        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(45, ErrorMessage = "Longitud máxima 45")]
        public string nombre { get; set; }

        [Display(Name = "Fecha Inicio")]
        [DataType(DataType.Date, ErrorMessage = "Por favor ingrese un fecha válida")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        public System.DateTime fInicio { get; set; }

        [Display(Name = "Fecha Final")]
        [DataType(DataType.Date, ErrorMessage = "Por favor ingrese un fecha válida")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        public System.DateTime fFinal { get; set; }

        [Display(Name = "Descripción")]
        [DataType(DataType.Text, ErrorMessage = "Por favor ingrese un dato de tipo texto")]
        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(100, ErrorMessage = "Longitud máxima 100")]
        public string descripcion { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public int estado { get; set; }

        [Display(Name = "Usuario Creación")]
        [Required(ErrorMessage = "Ingrese un dato válido")]
        public string usuarioCreacion { get; set; }

        [Display(Name = "Fecha Creación")]
        [DataType(DataType.DateTime, ErrorMessage = "Ingrese una fecha válida")]
        [Required(ErrorMessage = "Ingrese un dato válido")]
        public System.DateTime fechaCreacion { get; set; }

        [Display(Name = "Usuario Modificación")]
        public string usuarioModificacion { get; set; }

        [Display(Name = "Fecha Modificación")]
        [DataType(DataType.DateTime, ErrorMessage = "Ingrese una fecha válida")]
        public Nullable<System.DateTime> fechaModificacion { get; set; }

        [Display(Name = "Usuario Eliminación")]
        public string usuarioEliminacion { get; set; }

        [Display(Name = "Fecha Eliminación")]
        [DataType(DataType.DateTime, ErrorMessage = "Ingrese una fecha válida")]
        public Nullable<System.DateTime> fechaEliminacion { get; set; }
    }
}
