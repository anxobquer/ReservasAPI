using Microsoft.AspNetCore.Mvc;
using MI_API_REST.Repository;
using MI_API_REST.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Numerics;

namespace MI_API_REST.Controllers
{
    /// <summary>
    /// Controlador clientes
    /// </summary>
    [ApiController]
    [Route("api/clientes")]
    public class CliClientesController : ControllerBase
    {

        private RepCliClientes repCliClientes;

        /// <summary>
        /// Inicializando nuestra variable principal.
        /// </summary>
        public CliClientesController()
        {
            repCliClientes = new RepCliClientes();
        }

        /// <summary>
        /// M�todo para mostrar lista de clientes.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("MostrarClientes")]
        public ActionResult MostrarClientes()
        {
            List<ModClieClientes>? modClie = new();
            modClie = repCliClientes.MostrarClientes();

            if (modClie != null)
            {
                return Ok(modClie);
            }
            else
            {
                return BadRequest("No se encontraron clientes registrados.");
            }
        }

        /// <summary>
        /// M�todo para buscar cliente en espec�fico.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("BuscarCliente/{codigo}")]
        public ActionResult BuscarCliente(string codigo)
        {
            ModClieClientes? modClie = new();
            modClie = repCliClientes.BuscarCliente(codigo);

            if (modClie != null)
            {
                return Ok(modClie);
            }
            else
            {
                return BadRequest("El cliente con el c�digo " + codigo + " no est� registrado.");
            }
        }

        /// <summary>
        /// M�todo para registrar nuevo cliente.
        /// </summary>
        /// <param name="modClie"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpPost("RegistrarCliente")]
        public ActionResult RegistrarCliente(ModClieClientes modClie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Los datos enviados no tienen un formato correcto.");
            }

            try
            {
                //if (modClie.CliNombres.Trim().ToString().Count() == 0)
                if (String.IsNullOrEmpty(modClie.CliNombres))
                {
                    return BadRequest("El Nombre del cliente es obligatorio.");
                }

                int resultado = repCliClientes.RegistrarCliente(modClie);
                if (resultado == 0)
                {
                    return BadRequest("Error inesperado al intentar guardar el cliente.");
                }
                else
                {
                    return Ok("Cliente registrado con �xito.");
                }

            }
            catch (Exception e)
            {

                throw;
            }
        }

        /// <summary>
        /// M�todo para actualizar datos del cliente.
        /// </summary>
        /// <param name="modClie"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("ActualizarCliente")]
        public ActionResult ActualizarCliente(ModClieClientes modClie)
        {
            if (String.IsNullOrEmpty(modClie.CliNombres))
            {
                return BadRequest("El Nombre del cliente es obligatorio.");
            }

            int resultado = repCliClientes.ActualizarCliente(modClie);
            if (resultado == 0)
            {
                return BadRequest("Error inesperado al intentar guardar los datos del cliente.");
            }
            else
            {
                return Ok("Cliente actualizado con �xito.");
            }
        }

        /// <summary>
        /// M�todo para Eliminar datos del cliente.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("EliminarCliente/{codigo}")]
        public ActionResult EliminarCliente(string codigo)
        {
            if (codigo.Trim().Count() == 0)
            {
                return BadRequest("El c�digo del cliente es obligatorio.");
            }

            int resultado = repCliClientes.EliminarCliente(codigo);
            if (resultado == 0)
            {
                return BadRequest("Error inesperado al intentar eliminar los datos del cliente.");
            }
            else
            {
                return Ok("Cliente eliminado con �xito.");
            }
        }
    }
}