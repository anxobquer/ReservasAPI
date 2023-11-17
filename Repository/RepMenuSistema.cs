using MI_API_REST.Entities;
using System.Data;

namespace MI_API_REST.Repository
{
    public class RepMenuSistema
    {
        readonly FuncionesGenerales funciones;
        private Dictionary<string, string> parametros;

        public RepMenuSistema()
        {
            funciones = new FuncionesGenerales();
            parametros = new Dictionary<string, string>();
        }

        /// <summary>
        /// Método para mostrar lista de opciones de menú.
        /// </summary>
        /// <returns></returns>
        public List<ModMenuSistema>? MostrarMenu(int codigo)
        {
            parametros = new()
            {
                { "@codigo", Convert.ToString(codigo)}
            };
            DataTable dt = funciones.CargarDatos("sel_mes_menu_sistema", parametros);
            if (dt.Rows.Count == 0)
            {
                return null;
            }

            List<ModMenuSistema> list = new();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ModMenuSistema clientes = new()
                {
                    Codigo = Convert.ToInt32(dt.Rows[i]["mes_codigo"].ToString()),
                    Nombre = dt.Rows[i]["mes_nombre"].ToString(),
                    CodMes = Convert.ToInt32(dt.Rows[i]["mes_codmes"].ToString()),
                    Accion = dt.Rows[i]["mes_accion"].ToString(),
                    Controlador = dt.Rows[i]["mes_controlador"].ToString(),
                    Icono = dt.Rows[i]["mes_icono"].ToString(),
                    Orden = Convert.ToInt32(dt.Rows[i]["mes_orden"].ToString()),
                    TotalSubMenu = Convert.ToInt32(dt.Rows[i]["total_sub_menu"].ToString())
                };
                list.Add(clientes);
            }
            return list;
        }
    }
}
