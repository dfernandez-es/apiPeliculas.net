using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace peliculasapi.Servicios
{
    public class AlmacenadorArchivosLocal : IAlmacenadorArchivos
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;
        public AlmacenadorArchivosLocal(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.env = env;

        }
        
            
        
        public Task BorrarArchivo(string ruta, string contenedor)
        {
            if(ruta != null)
            {
                var nombreArchivo = Path.GetFileName(ruta);
                var directorioArchivo = Path.Combine(env.WebRootPath, contenedor, nombreArchivo);
                if(File.Exists(directorioArchivo))
                {
                    File.Delete(directorioArchivo);
                }
            }
            return  Task.FromResult(0);
        }
        

        public async Task<string> EditarArchivo(byte[] contenido, string extension, string contenedor, string ruta, string contentType)
        {
            await BorrarArchivo(ruta, contenedor);
            return await GuardarArchivo(contenido, extension, contenedor, contentType);
        }

        public async Task<string> GuardarArchivo(byte[] contenido, string extension, string contenedor, string contentType)
        {
            var nombreArchivo = $"{Guid.NewGuid()}{extension}";
            var ruta = Path.Combine(env.WebRootPath, contenedor);

            if(!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }

            string  rutaFinal = Path.Combine(ruta, nombreArchivo);
            await File.WriteAllBytesAsync(rutaFinal, contenido);

            var urlActual = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            var urlParaBD = Path.Combine(urlActual, contenedor, nombreArchivo).Replace("\\", "/");
            return urlParaBD;
        }
    }
}
        
        
    
