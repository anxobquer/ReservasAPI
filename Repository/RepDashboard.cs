using ClienteApi.Models;
using MI_API_REST.Entities;
using System.Data;

namespace MI_API_REST.Repository
{
    public class RepDashboard
    {
        readonly FuncionesGenerales funciones;
        private Dictionary<string, string> parametros;

        public RepDashboard()
        {
            funciones = new FuncionesGenerales();
            parametros = new Dictionary<string, string>();
        }

        /// <summary>
        /// Muestra lista de servicios registrados.
        /// </summary>
        /// <returns></returns>
        public ModDashboard? MostrarEstadisticas()
        {
            DataTable dt = funciones.CargarDatos("dashboard");
            if (dt.Rows.Count == 0)
            {
                return null;
            }

            ModDashboard modelo = new()
            {
                NumClientes = Convert.ToInt32(dt.Rows[0]["numclientes"].ToString()),
                NumServicios = Convert.ToInt32(dt.Rows[0]["numservicios"].ToString()),
            };

            return modelo;
        }
    }
}