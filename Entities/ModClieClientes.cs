using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
namespace MI_API_REST.Entities
{
    public class ModClieClientes
    {
        public int CliCodigo { get; set; }
        public string? CliNombres { get; set; }
        public string? CliApellidos { get; set; }
        public string? CliTelefono { get; set; }
        public string? CliEmail { get; set; }
    }
}
