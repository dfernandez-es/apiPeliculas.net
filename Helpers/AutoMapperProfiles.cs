using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using peliculasapi.DTOs;
using peliculasapi.Entidades;

namespace peliculasapi.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Genero,GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO,Genero>();
            CreateMap<Actor,ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO,Actor>().ReverseMap().ForMember(x => x.Foto, options => options.Ignore());
            CreateMap<ActorPatchDTO,Actor>().ReverseMap();
            CreateMap<Pelicula,PeliculaDTO>().ReverseMap();
            CreateMap<PeliculaCreacionDTO,Pelicula>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.PeliculasGeneros, options => options.MapFrom(MapPeliculasGeneros))
                .ForMember(x => x.PeliculasActores, options => options.MapFrom(MapPeliculasActores)).ReverseMap();
            CreateMap<PeliculaPatchDTO,Pelicula>().ReverseMap();
        }

        private List<PeliculasGeneros> MapPeliculasGeneros(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            var resultado = new List<PeliculasGeneros>();
            if(peliculaCreacionDTO.GenerosIDs == null){return resultado;}
            foreach(var id in peliculaCreacionDTO.GenerosIDs)
            {
                resultado.Add(new PeliculasGeneros() {GeneroId = id});
            }
            return resultado;
        }

        private List<PeliculasActores> MapPeliculasActores(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            var resultado = new List<PeliculasActores>();
            if(peliculaCreacionDTO.Actores == null){return resultado;}
            foreach(var actor in peliculaCreacionDTO.Actores)
            {
                resultado.Add(new PeliculasActores() {ActorId = actor.ActorID, Personaje = actor.Personaje});
            }
            return resultado;
        }
    }
}