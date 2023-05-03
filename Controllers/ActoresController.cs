using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using peliculasapi.DTOs;
using peliculasapi.Entidades;
using peliculasapi.Servicios;

namespace peliculasapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActoresController : ControllerBase
    {
        public ApplicationDbContext Context { get; }
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        public ActoresController(ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.almacenadorArchivos = almacenadorArchivos;
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
        public async  Task<ActionResult> Post([FromForm]ActorCreacionDTO actorCreacionDTO)
        {
            var actor = mapper.Map<Actor>(actorCreacionDTO);

            if(actorCreacionDTO.foto != null){
                using(var ms = new MemoryStream()){
                    await actorCreacionDTO.foto.CopyToAsync(ms);
                    var contenido = ms.ToArray();
                    var extension = Path.GetExtension(actorCreacionDTO.foto.FileName);
                    actor.Foto = await almacenadorArchivos.GuardarArchivo(contenido, extension, "actores",actorCreacionDTO.foto.ContentType);
                }
            }
            
            Context.Actores.Add(actor);
            await Context.SaveChangesAsync();
            return CreatedAtRoute("GetActor", new { id = actor.Id }, mapper.Map<ActorDTO>(actor));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm]ActorCreacionDTO actorCreacionDTO)
        {
            var actorDB = await Context.Actores.FirstOrDefaultAsync(x=>x.Id == id);
            if(actorDB == null) return NotFound();
            actorDB = mapper.Map(actorCreacionDTO,actorDB);
            if(actorCreacionDTO.foto != null){
                using(var ms = new MemoryStream()){
                    await actorCreacionDTO.foto.CopyToAsync(ms);
                    var contenido = ms.ToArray();
                    var extension = Path.GetExtension(actorCreacionDTO.foto.FileName);
                    actorDB.Foto = await almacenadorArchivos.EditarArchivo(contenido, extension, "actores",actorDB.Foto,actorCreacionDTO.foto.ContentType);
                }
            }   
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