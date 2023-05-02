using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace peliculasapi.Validaciones
{
    public class TipoArchivoValidacion:ValidationAttribute
    {
        private readonly string[] tiposValidos;
        public TipoArchivoValidacion()
        {
            tiposValidos = new string[]{ "image/jpeg", "image/png", "image/gif" };
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;
            if(formFile == null)
            {
                return ValidationResult.Success;
            }

            if(!tiposValidos.Contains(formFile.ContentType))
            {
                return new ValidationResult($"El tipo de archivo no es válido: {string.Join(",", tiposValidos)}");
            }
            return ValidationResult.Success;
        }
    }
}