using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace peliculasapi.Helpers
{
    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var nombrePropiedad = bindingContext.ModelName;
            var proveedorDeValores = bindingContext.ValueProvider.GetValue(nombrePropiedad);
            if (proveedorDeValores == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }
            try{
                var valorDeserializado = JsonConvert.DeserializeObject<T>(proveedorDeValores.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(valorDeserializado);            
            }
            catch{
                bindingContext.ModelState.TryAddModelError(nombrePropiedad, "No es una lista de enteros");
            }
            return Task.CompletedTask;
        }
    }
}