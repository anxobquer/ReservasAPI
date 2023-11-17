using MI_API_REST.Entities;
using MI_API_REST.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace MI_API_REST.Controllers
{
    [Route("api/MenuSistema")]
    [ApiController]
    public class MenuSistemaController : ControllerBase
    {
        private readonly RepMenuSistema menuSistema;

        public MenuSistemaController()
        {
            menuSistema = new RepMenuSistema();
        }

        [HttpGet("MenuSistema/{codigo}")]
        public ActionResult MostrarMenu(int codigo)
        {
            List<ModMenuSistema> list = menuSistema.MostrarMenu(codigo);
            if (list != null)
            {
                return Ok(list);
            }
            else
            {
                return BadRequest("No se encontraron registros.");
            }
        }
    }
}
