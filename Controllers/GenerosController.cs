using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using peliculasapi.DTOs;
using peliculasapi.Entidades;

namespace peliculasapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // -> "api/generos"
    public class GenerosController:ControllerBase{
        public ApplicationDbContext Context { get; }
        private readonly IMapper mapper;

        public GenerosController(ApplicationDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.Context = context;
        }


        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get(){
            var generos = await Context.Generos.ToListAsync();
            var dtos = mapper.Map<List<Genero>, List<GeneroDTO>>(generos);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GeneroDTO>> Get(int id){
            var genero = await Context.Generos.FirstOrDefaultAsync(x => x.Id == id);
            if(genero == null) return NotFound();
            var dto = mapper.Map<Genero, GeneroDTO>(genero);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<GeneroDTO>> Post([FromBody] GeneroCreacionDTO generoCreacionDTO){
            var genero = mapper.Map<Genero>(generoCreacionDTO);
            Context.Generos.Add(genero);
            await Context.SaveChangesAsync();
            var generoDTO = mapper.Map<GeneroDTO>(genero);
            return new CreatedAtRouteResult("GetGenero", new {id = generoDTO.Id}, generoDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO){
            var genero = await Context.Generos.FirstOrDefaultAsync(x => x.Id == id);
            if(genero == null) return NotFound();
            mapper.Map(generoCreacionDTO, genero);
            await Context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id){
            var genero = await Context.Generos.FirstOrDefaultAsync(x => x.Id == id);
            if(genero == null) return NotFound();
            Context.Remove(new Genero{Id = id});
            await Context.SaveChangesAsync();
            return NoContent();
        }

    }
}