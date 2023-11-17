using System.Data;
namespace MI_API_REST.Repository
{
    /// <summary>
    /// Clase para manteniniento de Usuarios que consumen esta API.
    /// </summary>
    public class RepUsuariosApi
    {
        FuncionesGenerales funciones = new();
        Dictionary<string, string> parametros = new();

        /// <summary>
        /// Método para verificar si existe el usuario que quiere loguearse.
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public bool VerificarUsuario(string usuario, string pass)
        {
            parametros.Add("@usuario", usuario);
            parametros.Add("@pass", pass);

            DataTable dt = new DataTable();
            dt = funciones.CargarDatos("sel_usa_usuarios_api", parametros);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
