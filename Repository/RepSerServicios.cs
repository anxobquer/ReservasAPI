using MI_API_REST.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MI_API_REST.Repository
{
    /// <summary>
    /// Clase para el manto de Servicios.
    /// </summary>
    public class RepSerServicios
    {
        readonly FuncionesGenerales funciones;
        private Dictionary<string, string> parametros;              

        /// <summary>
        /// 
        /// </summary>
        public RepSerServicios()
        {
            funciones = new FuncionesGenerales();
            parametros = new Dictionary<string, string>();
        }

        /// <summary>
        /// Muestra lista de servicios registrados.
        /// </summary>
        /// <returns></returns>
        public List<ModSerServicios>? MostrarServicios()
        {
            DataTable dt = funciones.CargarDatos("sel_all_ser_servicios");
            if (dt.Rows.Count == 0)
            {
                return null;
            }

            List<ModSerServicios> list = new();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ModSerServicios servicio = new()
                {
                    SerCodigo = Convert.ToInt32(dt.Rows[i]["ser_codigo"].ToString()),
                    SerNombre = dt.Rows[i]["ser_nombre"].ToString(),
                    SerDescripcion = dt.Rows[i]["ser_descripcion"].ToString(),
                    SerPrecio = Convert.ToDecimal(dt.Rows[i]["ser_precio"].ToString()),
                    Ruta = dt.Rows[i]["des_ruta"].ToString(),
                    SerFechaCreacion = Convert.ToDateTime(dt.Rows[i]["ser_fecha_creacion"].ToString()),
                    SerUltModificacion = Convert.ToDateTime(dt.Rows[i]["ser_ult_modificacion"].ToString()),
                };
                list.Add(servicio);
            }
            return list;
        }

        /// <summary>
        /// Método para buscar un cliente en específico.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public ModSerServicios BuscarServicio(string codigo)
        {
            parametros.Add("@codigo", codigo);
            DataTable dt = funciones.CargarDatos("sel_ser_servicio", parametros);
            if (dt.Rows.Count == 0)
            {
                return null;
            }

            ModSerServicios servicio = new()
            {
                SerCodigo = Convert.ToInt32(dt.Rows[0]["ser_codigo"].ToString()),
                SerNombre = dt.Rows[0]["ser_nombre"].ToString(),
                SerDescripcion = dt.Rows[0]["ser_descripcion"].ToString(),
                Ruta = dt.Rows[0]["des_ruta"].ToString(),
                SerPrecio = Convert.ToDecimal(dt.Rows[0]["ser_precio"].ToString())
            };

            return servicio;
        }
        /// <summary>
        /// Registra un nuevo servicio con su respectiva imagen principal.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="modServicios"></param>
        /// <returns></returns>
        public async Task<int> RegistrarServicio(string host, [FromForm] ModSerServicios modServicios)
        {
            if (string.IsNullOrEmpty(modServicios.SerNombre))
            {
                return 0;
            }

            parametros.Add("@nombre", modServicios.SerNombre);
            parametros.Add("@descripcion", modServicios.SerDescripcion);
            parametros.Add("@precio", Convert.ToString(modServicios.SerPrecio));
            int codser = funciones.EjecutarInsDelUpd("ins_ser_servicios", parametros);
            if (codser > 0)
            {
                GuardarArchivo(host, "ser", codser, modServicios.Imagen);
            }
            return codser;
        }
        /// <summary>
        /// Actualiza información general del servicio y la imagen principal del mismo.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="modServicios"></param>
        /// <returns></returns>
        public async Task<int> ActualizarServicio(string host, [FromForm] ModSerServicios modServicios)
        {
            if (string.IsNullOrEmpty(modServicios.SerNombre))
            {
                return 0;
            }

            parametros.Add("@nombre", modServicios.SerNombre);
            parametros.Add("@descripcion", modServicios.SerDescripcion);
            parametros.Add("@precio", Convert.ToString(modServicios.SerPrecio));
            parametros.Add("@codigo", Convert.ToString(modServicios.SerCodigo));
            int respuesta = funciones.EjecutarInsDelUpd("up_ser_servicios", parametros);
            if (respuesta > 0)
            {
                parametros = new()
                {
                    { "@codigo", Convert.ToString(modServicios.SerCodigo)}
                };
                DataTable dt = funciones.CargarDatos("get_ser_img", parametros);
                if (dt.Rows.Count > 0)
                {
                    string? rutaArchivoActual = dt.Rows[0]["des_ruta"].ToString();

                    if (rutaArchivoActual != modServicios.Ruta)
                    {
                        if (modServicios.Imagen != null && modServicios.Imagen.FileName != "none.png")
                        {
                            int result = ActualizarArchivo(host, "ser", modServicios.SerCodigo, modServicios.Imagen);
                            if (result > 0)
                            {
                                 FileHelper FileHelper = new();
                                FileHelper.EliminarArchivo(host, rutaArchivoActual);
                            }
                        }
                    }
                }
                // Se registra la imagen por primera vez
                else
                {
                    if (modServicios.Imagen != null && modServicios.Imagen.FileName != "none.png")
                    {
                        GuardarArchivo(host, "ser", modServicios.SerCodigo, modServicios.Imagen);
                    }
                }
            }
            return respuesta;
        }

        /// <summary>
        /// Guardamos el archivo en el servidor.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="prefijoEntidad"></param>
        /// <param name="codigoEntidad"></param>
        /// <param name="archivo"></param>
        /// <returns></returns>
        public int GuardarArchivo(string host, string prefijoEntidad, int codigoEntidad, IFormFile archivo)
        //public int GuardarArchivo(string prefijoEntidad, int codigoEntidad, [FromForm] ModSerServicios modServicios)
        {
            FileHelper FileHelper = new();
            string rutaArchivo = FileHelper.SubirArchivo( host, "ServiciosImagenes", prefijoEntidad, codigoEntidad, archivo);

            parametros = new()
            {
                { "@descripcion", "" },
                { "@ruta", rutaArchivo },
                { "@codser", Convert.ToString(codigoEntidad) },
                { "@principal", "SI" }
            };
            return funciones.EjecutarInsDelUpd("ins_des_detalle_ser", parametros);
        }

        /// <summary>
        /// Actualiza el archivo en el servidor.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="prefijoEntidad"></param>
        /// <param name="codigoEntidad"></param>
        /// <param name="archivo"></param>
        /// <returns></returns>
        private int ActualizarArchivo(string host, string prefijoEntidad, int codigoEntidad, IFormFile archivo)
        //private int ActualizarArchivo(string prefijoEntidad, int codigoEntidad, [FromForm] ModSerServicios modServicios)
        {
            FileHelper FileHelper = new();
            string rutaArchivo = FileHelper.SubirArchivo(host, "ServiciosImagenes", prefijoEntidad, codigoEntidad, archivo);
            parametros = new()
            {
                { "@ruta", rutaArchivo },
                { "@codser", Convert.ToString(codigoEntidad) }
            };
            return funciones.EjecutarInsDelUpd("up_ruta_des_detalle_ser", parametros);
        }

        /// <summary>
        /// Elimina imagen del servidor
        /// Elimina servicio de nuestra BD
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public int EliminarServicio(string host, string codigo)
        {
            parametros = new()
            {
                { "@codigo", codigo}
            };

            DataTable dt = funciones.CargarDatos("get_ser_img", parametros);

            if (dt.Rows.Count > 0)
            {
                string? rutaArchivoActual = dt.Rows[0]["des_ruta"].ToString();
                FileHelper fileHelper = new();
                fileHelper.EliminarArchivo(host, rutaArchivoActual);
            }

            return funciones.EjecutarInsDelUpd("del_ser_servicios", parametros);
        }
    }
}
