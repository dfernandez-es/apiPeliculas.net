using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace peliculasapi.Entidades
{
    public class Actor
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string nombre  { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public string Foto { get; set; }
    }
}