using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MI_API_REST.Entities
{
    public class ModSerServicios
    {
        public int SerCodigo { get; set; }
        public string SerNombre { get; set; }
        public string SerDescripcion { get; set; }
        public decimal SerPrecio { get; set; }

        public IFormFile Imagen { get; set; }

        public string Ruta { get; set; }

        public DateTime SerFechaCreacion { get; set; }
        public DateTime SerUltModificacion { get; set; }

        //[NotMapped]
        //public List<ModSerDetalleSer> ModDetalle { get; set; }
    }

    public class ModSerDetalleSer
    {
        public int DesCodigo { get; set; }
        public string DesDescripcion { get; set; }
        public string DesRuta { get; set; }
        //public IFormFile DetFoto { get; set; }
        public int DesCodSer { get; set; }
    }
}
