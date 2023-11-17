using MI_API_REST.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MI_API_REST.Repository
{
    /// <summary>
    /// Clase para el manto de clientes.
    /// </summary>
    public class RepCliClientes
    {
        readonly FuncionesGenerales funciones;
        readonly Dictionary<string, string> parametros;

        /// <summary>
        /// Inicializando variables en nuestro constructor.
        /// </summary>
        public RepCliClientes()
        {
            funciones = new FuncionesGenerales();
            parametros = new Dictionary<string, string>();
        }

        /// <summary>
        /// Método para mostrar lista de clientes.
        /// </summary>
        /// <returns></returns>
        public List<ModClieClientes>? MostrarClientes()
        {
            DataTable dt = funciones.CargarDatos("sel_all_cli_clientes");
            if (dt.Rows.Count == 0)
            {
                return null;
            }

            List<ModClieClientes> list = new();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ModClieClientes clientes = new()
                {
                    CliCodigo = Convert.ToInt32(dt.Rows[i]["cli_codigo"].ToString()),
                    CliNombres = dt.Rows[i]["cli_nombres"].ToString(),
                    CliApellidos = dt.Rows[i]["cli_apellidos"].ToString(),
                    CliTelefono = dt.Rows[i]["cli_telefono"].ToString(),
                    CliEmail = dt.Rows[i]["cli_email"].ToString()
                };
                list.Add(clientes);
            }
            return list;
        }

        /// <summary>
        /// Método para buscar un cliente en específico.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public ModClieClientes? BuscarCliente(string codigo)
        {
            parametros.Add("@codigo", codigo);
            DataTable dt = funciones.CargarDatos("sel_cli_cliente", parametros);
            if (dt.Rows.Count == 0)
            {
                return null;
            }

            ModClieClientes cliente = new()
            {
                CliCodigo = Convert.ToInt32(dt.Rows[0]["cli_codigo"].ToString()),
                CliNombres = dt.Rows[0]["cli_nombres"].ToString(),
                CliApellidos = dt.Rows[0]["cli_apellidos"].ToString(),
                CliTelefono = dt.Rows[0]["cli_telefono"].ToString(),
                CliEmail = dt.Rows[0]["cli_email"].ToString()
            };
            
            return cliente;
        }

        /// <summary>
        /// Método para registrar un nuevo cliente.
        /// </summary>
        /// <param name="modClie"></param>
        /// <returns></returns>
        public int RegistrarCliente(ModClieClientes modClie)
        {
            //if (modClie.CliNombres.Trim().ToString().Count() == 0)
            //{
            //    return -1; // Devolver este valor en caso de venir null el nombre
            //}
            if (String.IsNullOrEmpty(modClie.CliNombres))
            {
                return -1; // Devolver este valor en caso de venir null el nombre
            }

            parametros.Add("@nombres", modClie.CliNombres.Trim());
            parametros.Add("@apellidos", modClie.CliApellidos.Trim());
            parametros.Add("@telefono", modClie.CliTelefono.Trim());
            parametros.Add("@email", modClie.CliEmail.Trim());

            return funciones.EjecutarInsDelUpd("ins_cli_cliente", parametros);
        }

        /// <summary>
        /// Método para actualizar datos del cliente.
        /// </summary>
        /// <param name="modClie"></param>
        /// <returns></returns>
        public int ActualizarCliente(ModClieClientes modClie)
        {
            if (String.IsNullOrEmpty(modClie.CliNombres))
            {
                return -1; // Devolver este valor en caso de venir null el nombre
            }

            parametros.Add("@nombres", modClie.CliNombres.Trim());
            parametros.Add("@apellidos", modClie.CliApellidos.Trim());
            parametros.Add("@telefono", modClie.CliTelefono.Trim());
            parametros.Add("@email", modClie.CliEmail.Trim());
            parametros.Add("@codigo", Convert.ToString(modClie.CliCodigo));

            return funciones.EjecutarInsDelUpd("up_cli_cliente", parametros);
        }

        /// <summary>
        /// Método para eliminar la información del cliente.        
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns>Éxito = 1 o No eliminado = 0 </returns>
        public int EliminarCliente(string codigo)
        {
            parametros.Add("@codigo", codigo);
            return funciones.EjecutarInsDelUpd("del_cli_cliente", parametros); 
        }
    }
}
