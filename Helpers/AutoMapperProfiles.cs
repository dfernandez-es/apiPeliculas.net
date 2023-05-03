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
            CreateMap<ActorCreacionDTO,Actor>().ReverseMap().ForMember(x => x.foto, options => options.Ignore());
        }
    }
}