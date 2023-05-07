using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace peliculasapi.DTOs
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int CantidadRegistrosPorPagina = 10;
        private readonly int CantidadMaximaRegistrosPorPagina = 25;
        public int cantidadRegistrosPorPagina{
            get=>CantidadRegistrosPorPagina;
            set{CantidadRegistrosPorPagina = (value>CantidadMaximaRegistrosPorPagina) ? CantidadMaximaRegistrosPorPagina : value;}
        }
    }
}