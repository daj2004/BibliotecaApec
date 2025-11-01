using System.ComponentModel.DataAnnotations;

namespace BibliotecaUNAPEC.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Usuario requerido")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Contrase�a requerida")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}