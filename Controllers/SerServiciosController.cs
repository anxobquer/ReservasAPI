using Microsoft.AspNetCore.Mvc;
using MI_API_REST.Repository;
using MI_API_REST.Entities;
using Microsoft.AspNetCore.Authorization;
using System;

namespace MI_API_REST.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/Servicios")]
    public class SerServiciosController : ControllerBase
    {
        readonly RepSerServicios repServicio;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private static string _urlBaseApi = "";
        /// <summary>
        /// Inicializando nuestra variable principal.
        /// </summary>
        public SerServiciosController(IWebHostEnvironment webHostEnvironment)
        {
            repServicio = new();
            _webHostEnvironment = webHostEnvironment;

            IConfigurationRoot builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json").Build();

            string? env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            _urlBaseApi = env == "Development" ? builder.GetSection("Url:UrlApiLocal").Value : builder.GetSection("Url:UrlApiWeb").Value;
        }
        /// <summary>
        /// Método para mostrar lista de clientes.
        /// </summary>
        /// <returns></returns>

        [HttpGet("MostrarServicios")]
        public ActionResult MostrarServicios()
        {
            List<ModSerServicios>? modGas = new();
            modGas = repServicio.MostrarServicios();

            if (modGas != null)
            {
                modGas.ForEach(item =>
                {
                    item.Ruta = GetImage(item.Ruta);
                });
                return Ok(modGas);
            }
            else
            {
                return BadRequest("No se encontraron registros.");
            }
        }

        /// <summary>
        /// Método para mostrar detalles de un servicio.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        [HttpGet("BuscarServicio/{codigo}")]
        public ActionResult BuscarServicio(string codigo)
        {
            ModSerServicios? modSer = new();
            modSer = repServicio.BuscarServicio(codigo);

            if (modSer != null)
            {
                modSer.Ruta = GetImage(modSer.Ruta);
                return Ok(modSer);
            }
            else
            {
                return BadRequest("Registro no encontrado.");
            }
        }

        /// <summary>
        /// Registra un nuevo servicio
        /// </summary>
        /// <param name="modServicio"></param>
        /// <returns></returns>
        [HttpPost("RegistrarServicio")]
        public async Task<ActionResult> RegistrarServicio([FromForm] ModSerServicios modServicio)
        {
            int resultado = await repServicio.RegistrarServicio(_webHostEnvironment.WebRootPath, modServicio);
            if (resultado == 0)
            {
                return BadRequest("Error inesperado al intentar guardar el servicio.");
            }
            else
            {
                return Ok("Servicio registrado con éxito.");
            }
        }

        /// <summary>
        /// Actualiza los detalles del servicio
        /// </summary>
        /// <param name="modServicio"></param>
        /// <returns></returns>
        [HttpPut("ActualizarServicio")]
        public async Task<ActionResult> ActualizarServicio([FromForm] ModSerServicios modServicio)
        {
            if (ModelState.IsValid)
            {
                int resultado = await repServicio.ActualizarServicio(_webHostEnvironment.WebRootPath, modServicio);
                if (resultado == 0)
                {
                    return BadRequest("Error inesperado al intentar actualizar el servicio.");
                }
                else
                {
                    return Ok("Servicio actualizado con éxito.");
                }
            }
            else
            {
                return BadRequest("Los datos enviados no son correctos.");
            }
        }
        /// <summary>
        /// Elimina servicio con todos sus detalles
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        [HttpDelete("EliminarServicio/{codigo}")]
        public async Task<ActionResult> ElilminarServicio(string codigo)
        {
            int resultado = repServicio.EliminarServicio(_webHostEnvironment.WebRootPath, codigo);
            if (resultado == 0)
            {
                return BadRequest("Error inesperado al intentar eliminar el servicio.");
            }
            else
            {
                return Ok("Servicio eliminado con éxito.");
            }
        }

        /// <summary>
        /// Obtiene la ruta física de la imagen
        /// </summary>
        /// <param name="urlImgBd"></param>
        /// <returns></returns>
        private string ObtenerUrlImagen(string urlImgBd)
        {
            return Path.Combine(_webHostEnvironment.WebRootPath, urlImgBd.TrimStart('/').Replace("/", "\\"));
        }

        // ServiciosImagenes/ser_18_122368f4-c208-4746-a76b-275f64dd6758.jpg
        private string GetImage(string urlBd)
        {
            string ruta = ObtenerUrlImagen(urlBd);

            string imgUrl;

            if (!System.IO.File.Exists(ruta))
            {
                imgUrl = _urlBaseApi + "/none.png";
            }
            else
            {
                imgUrl = _urlBaseApi + "/" + urlBd;
            }
            return imgUrl;

            //return (!System.IO.File.Exists(ruta)) ? _urlBaseApi + "/none.png" : _urlBaseApi + "/" + urlBd;
        }
    }
}
