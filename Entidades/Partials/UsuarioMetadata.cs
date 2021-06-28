using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Partials
{
    public class UsuarioMetadata
    {
        [Required(ErrorMessage = "Debe ingresar un email")]
        [EmailAddress(ErrorMessage = "Debe ingresar un formato de email válido")]
        [StringLength(maximumLength: 300, ErrorMessage = "El email no debe superar los 300 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe ingresar una password")]
        [StringLength(maximumLength: 300, ErrorMessage = "La password no debe superar los 300 caracteres")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre")]
        [StringLength(maximumLength: 100, ErrorMessage = "El nombre no debe superar los 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe ingresar un apellido")]
        [StringLength(maximumLength: 100, ErrorMessage = "El apellido no debe superar los 100 caracteres")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Debe indicar si es administrador o no")]
        public bool EsAdmin { get; set; }
    }
}
