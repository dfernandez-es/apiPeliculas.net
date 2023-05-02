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
    [Route("api/[controller]")]
    public class ActoresController : ControllerBase
    {
        public ApplicationDbContext Context { get; }
        private readonly IMapper mapper;
        public ActoresController(ApplicationDbContext context, IMapper mapper)
        {
        this.mapper = mapper;
        this.Context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get()
        {
            var actores = await Context.Actores.ToListAsync();
            return Ok(mapper.Map<List<ActorDTO>>(actores));            
        }

        [HttpGet("{id}", Name = "GetActor")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var actor = await Context.Actores.FirstOrDefaultAsync(x=>x.Id == id);
            if (actor == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<ActorDTO>(actor));
        }

        [HttpPost]
        public async  Task<ActionResult> Post([FromBody]ActorCreacionDTO actorCreacionDTO)
        {
            var actor = mapper.Map<Actor>(actorCreacionDTO);
            Context.Actores.Add(actor);
            await Context.SaveChangesAsync();
            return CreatedAtRoute("GetActor", new { id = actor.Id }, mapper.Map<ActorDTO>(actor));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]ActorCreacionDTO actorCreacionDTO)
        {
            var actor = mapper.Map<Actor>(actorCreacionDTO);
            actor.Id = id;            
            Context.Entry(actor).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var  actor = await Context.Actores.AnyAsync(x => x.Id == id);
            if(!actor) return NotFound();
            Context.Remove(new Actor{Id = id});
            await Context.SaveChangesAsync();
            return NoContent();
        }
    }
}