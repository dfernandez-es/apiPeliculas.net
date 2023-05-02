using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace peliculasapi.Validaciones
{
    public class PesoArchivoValidacion: ValidationAttribute
    {
        public int PesoMaximoEnMegaBytes { get; }
        public PesoArchivoValidacion(int PesoMaximoEnMegaBytes)
        {
            this.PesoMaximoEnMegaBytes = PesoMaximoEnMegaBytes;
            
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

            if(formFile.Length > PesoMaximoEnMegaBytes * 1024 * 1024)
            {
                return new ValidationResult($"El peso del archivo no puede ser mayor a {PesoMaximoEnMegaBytes} MB");
            }
        
            return ValidationResult.Success;
        
        }
    }
}