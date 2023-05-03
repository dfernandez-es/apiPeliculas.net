using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using peliculasapi.Validaciones;

namespace peliculasapi.DTOs
{
    public class ActorCreacionDTO
    {
        [Required]
        [StringLength(120)]
        public string nombre  { get; set; }
        public DateTime fechaNacimiento { get; set; }
        [PesoArchivoValidacion(PesoMaximoEnMegaBytes: 4)]
        [TipoArchivoValidacion]
        public IFormFile foto { get; set; }
    }
}