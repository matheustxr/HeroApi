using HeroApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace HeroApi.Data
{
    public class HeroContext : DbContext
    {
        public HeroContext(DbContextOptions<HeroContext> options) : base(options) { }

        public DbSet<Hero> Heroes { get; set; }
        public DbSet<Superpower> Superpowers { get; set; }
        public DbSet<HeroSuperpower> HeroSuperpowers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HeroSuperpower>()
                .HasKey(hs => new { hs.HeroId, hs.SuperpowerId });

            modelBuilder.Entity<Hero>()
                .HasIndex(h => h.HeroName)
                .IsUnique();
        }
    }
}
