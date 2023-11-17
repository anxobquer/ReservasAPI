using System.IO;
using System.Reflection;

using MI_API_REST.Entities;
using Microsoft.AspNetCore.Hosting;

namespace MI_API_REST.Repository
{
    /// <summary>
    /// Clase para la manipulación de archivos.
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// Guarda el archivo en el servidor.
        /// </summary>
        /// <param name="host">D:\net_core\net_core\MI_API_REST\Contenido\Imagenes\</param>
        /// <param name="carpeta">Contenido</param>
        /// <param name="prefijoEntidad">ser</param>
        /// <param name="codigoEntidad">25</param>
        /// <param name="archivo"></param>
        /// <returns></returns>
        /// 
        /// ser_23_4d7d43c1-38ae-434f-b285-c65ad0591502.jpg
        public string SubirArchivo(string host, string carpeta, string prefijoEntidad, int codigoEntidad, IFormFile archivo)
        {
            string nombreArvhivo = string.Concat(prefijoEntidad, "_", codigoEntidad, "_", Guid.NewGuid().ToString() + ".jpg");

            string path = Path.Combine(host, carpeta);

            string rutaArvhivo = carpeta + "/" + nombreArvhivo;
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (FileStream stream = new(path + "/"+ nombreArvhivo, FileMode.Create))
                {
                    archivo.CopyTo(stream);
                    stream.Close();
                }
                return rutaArvhivo;
            }
            catch (Exception ex)
            {
                string msj = ex.Message;
            }
            return "";
        }


        /// <summary>
        /// Elimina archivo del servidor.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="rutaArchivo"></param>
        public void EliminarArchivo(string host, string rutaArchivo)
        {
            string path = Path.Combine(host, rutaArchivo);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
