using ClienteApi.Models;
using MI_API_REST.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MI_API_REST.Controllers
{
    [ApiController]
    [Route("api/Estadisticas")]
    public class DashboardController : ControllerBase
    {
        RepDashboard repDashboard;

        public DashboardController()
        {
            repDashboard = new RepDashboard();
        }

        [HttpGet("MostrarEstadisticas")]
        public ActionResult MostrarEstadisticas()
        {
            ModDashboard? modelo = new();
            modelo = repDashboard.MostrarEstadisticas();
            if (modelo != null)
            {
                return Ok(modelo);
            }
            else
            {
                return BadRequest("No hay estadísticas para mostrar.");
            }
        }
    }
}