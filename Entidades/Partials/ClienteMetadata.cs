using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Partials
{
    public class ClienteMetadata
    {
        [Required(ErrorMessage = "Debe ingresar un nombre")]
        [StringLength(maximumLength: 200, ErrorMessage = "El nombre no debe superar los 200 caracteres")]
        public string Nombre { get; set; }

        [EmailAddress(ErrorMessage = "Debe ingresar un formato de email válido")]
        [StringLength(maximumLength: 300, ErrorMessage = "El email no debe superar los 300 caracteres")]
        public string Email { get; set; }

        [StringLength(maximumLength: 11, ErrorMessage = "El CUIT no debe superar los 11 caracteres")]
        public string Cuit { get; set; }

        [StringLength(maximumLength: 300, ErrorMessage = "La dirección no debe superar los 300 caracteres")]
        public string Direccion { get; set; }
    }
}
