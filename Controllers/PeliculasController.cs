using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using peliculasapi.DTOs;
using peliculasapi.Entidades;
using peliculasapi.Helpers;
using peliculasapi.Servicios;

namespace peliculasapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeliculasController:ControllerBase
    {
        private ApplicationDbContext Context { get; }
        private IMapper Mapper { get; }
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "peliculas";
        public PeliculasController(ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.almacenadorArchivos = almacenadorArchivos;
            this.Mapper = mapper;
            this.Context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<PeliculaDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO){
            var queryable = Context.Peliculas.AsQueryable();
            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.cantidadRegistrosPorPagina);
            var peliculas = await queryable.Paginar(paginacionDTO).ToListAsync();
            return Ok(Mapper.Map<List<PeliculaDTO>>(peliculas));
        }

        [HttpGet("{id}", Name = "GetPelicula")]
        public async Task<ActionResult<PeliculaDTO>> Get(int id){
            var pelicula = await Context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);
            if(pelicula == null) return NotFound();
            return Ok(Mapper.Map<PeliculaDTO>(pelicula));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] PeliculaCreacionDTO peliculaCreacionDTO){
            var pelicula = Mapper.Map<Pelicula>(peliculaCreacionDTO);


            if(peliculaCreacionDTO.Poster != null){
                using(var ms = new MemoryStream()){
                    await peliculaCreacionDTO.Poster.CopyToAsync(ms);
                    var contenido = ms.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
                    pelicula.Poster = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor,peliculaCreacionDTO.Poster.ContentType);
                }
            }

            Context.Peliculas.Add(pelicula);
            await Context.SaveChangesAsync();
            return CreatedAtRoute("GetPelicula", new {id = pelicula.Id}, Mapper.Map<PeliculaDTO>(pelicula));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] PeliculaCreacionDTO peliculaCreacionDTO){   
            var peliculaDB = await Context.Peliculas.FirstOrDefaultAsync(x=>x.Id == id);
            if(peliculaDB == null) return NotFound();
            peliculaDB = Mapper.Map(peliculaCreacionDTO,peliculaDB);
            if(peliculaCreacionDTO.Poster != null){
                using(var ms = new MemoryStream()){
                    await peliculaCreacionDTO.Poster.CopyToAsync(ms);
                    var contenido = ms.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
                    peliculaDB.Poster = await almacenadorArchivos.EditarArchivo(contenido, extension, "actores",peliculaDB.Poster,peliculaCreacionDTO.Poster.ContentType);
                }
            }   
            await Context.SaveChangesAsync();
            return NoContent();
        }     

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id){
            var pelicula = await Context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);
            if(pelicula == null) return NotFound();
            Context.Peliculas.Remove(pelicula);
            await Context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<PeliculaPatchDTO> patchDocument)
        {
            if(patchDocument == null) return BadRequest();
            var pelicula= await Context.Peliculas.FirstOrDefaultAsync(x=>x.Id == id);
            if(pelicula== null) return NotFound();
            var peliculaPatchDTO = Mapper.Map<PeliculaPatchDTO>(pelicula);
            patchDocument.ApplyTo(peliculaPatchDTO,ModelState);
            if(!ModelState.IsValid) return BadRequest(ModelState);
            Mapper.Map(peliculaPatchDTO,pelicula);
            await Context.SaveChangesAsync();
            return NoContent();
        }


    }
}