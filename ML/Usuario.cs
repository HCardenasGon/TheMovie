using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        public string? Username { get; set; }

        public string? NombreCompleto { get; set; }

        public string? Email { get; set; }

        public byte[]? Password { get; set; }

        public List<object>? Usuarios { get; set; }
    }
}
