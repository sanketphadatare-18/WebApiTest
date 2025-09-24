using Data_Layer.Model;
using Microsoft.EntityFrameworkCore;
using Web_Api_Core_.Model;
using Web_Api_Core_.Model.Joins_Tables;

namespace Data_Layer
{
    public  class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options)
        {
        }
        
        public DbSet<Pokemon> Pokemons { get; set; }

        public DbSet<Owner> Owners { get; set; }

        public DbSet<PokemonOwner> PokemonOwners { get; set; }

        public DbSet<PokemonCategory> PokemonCategories { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Reviewer> Reviewers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PokemonCategory>()
                .HasKey(c => new { c.PokemonId, c.CategoryId });
            modelBuilder.Entity<PokemonCategory>()
                .HasOne(p => p.Pokemon)
                .WithMany(p => p.PokemonCategories)
                .HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonCategory>()
              .HasOne(p => p.Category)
              .WithMany(p => p.PokemonCategories)
              .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<PokemonOwner>()
           .HasKey(c => new { c.PokemonId, c.OwnerId });
            modelBuilder.Entity<PokemonOwner>()
                .HasOne(p => p.Pokemon)
                .WithMany(p => p.PokemonOwner)
                .HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonOwner>()
              .HasOne(p => p.Owner)
              .WithMany(p => p.PokemonOwner)
              .HasForeignKey(p => p.OwnerId);

        }


    }
}
