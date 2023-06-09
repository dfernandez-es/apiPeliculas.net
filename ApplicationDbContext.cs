using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using peliculasapi.Entidades;

namespace peliculasapi
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder){
            builder.Entity<PeliculasActores>().HasKey(x => new{x.ActorId, x.PeliculaId});

            builder.Entity<PeliculasGeneros>().HasKey(x => new{x.GeneroId, x.PeliculaId});
    
            base.OnModelCreating(builder);
        }
        

        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }  
        public DbSet<PeliculasActores> PeliculasActores { get; set; }
        public DbSet<PeliculasGeneros> PeliculasGeneros { get; set; }
    }
}