using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TPWeb3.Models
{
    public class UsuarioVM
    {
        [Required(ErrorMessage = "Email no ingresado")]
        [EmailAddress(ErrorMessage = "Ingrese un formato válido de email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password no ingresada")]
        public string Password { get; set; }
    }
}
