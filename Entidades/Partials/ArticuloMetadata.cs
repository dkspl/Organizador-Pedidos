using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Partials
{
    public class ArticuloMetadata
    {
        [Required(ErrorMessage = "Debe ingresar un código")]
        [StringLength(maximumLength: 50, ErrorMessage = "El código no debe superar los 50 caracteres")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Debe ingresar una descripción")]
        [StringLength(maximumLength: 300, ErrorMessage = "La descripción no debe superar los 300 caracteres")]
        public string Descripcion { get; set; }
    }
}
