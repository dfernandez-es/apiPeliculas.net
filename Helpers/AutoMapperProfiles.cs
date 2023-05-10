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
            CreateMap<Pelicula,PeliculasDetallesDTO>()
                .ForMember(x => x.Generos, options => options.MapFrom(MapPeliculasGeneros))
                .ForMember(x => x.Actores, options => options.MapFrom(MapPeliculasActores));
        }


        private List<ActorPeliculaDetalleDTO> MapPeliculasActores(Pelicula pelicula, PeliculasDetallesDTO peliculasDetallesDTO)
        {
            var resultado = new List<ActorPeliculaDetalleDTO>();
            if(pelicula.PeliculasActores == null){return resultado;}
            foreach(var actorPelicula in pelicula.PeliculasActores)
            {
                resultado.Add(new ActorPeliculaDetalleDTO() {ActorId = actorPelicula.ActorId, NombrePersona = actorPelicula.Actor.Nombre, Personaje = actorPelicula.Personaje});
            }
            return resultado;
        }
        private List<GeneroDTO> MapPeliculasGeneros(Pelicula pelicula, PeliculasDetallesDTO peliculasDetallesDTO)
        {
            var resultado = new List<GeneroDTO>();
            if(pelicula.PeliculasGeneros == null){return resultado;}
            foreach(var generoPelicula in pelicula.PeliculasGeneros)
            {
                resultado.Add(new GeneroDTO() {Id = generoPelicula.GeneroId, Nombre = generoPelicula.Genero.Nombre});
            }
            return resultado;
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