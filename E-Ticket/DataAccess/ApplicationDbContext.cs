using E_Ticket.DataAccess.Enum;
using E_Ticket.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Ticket.DataAccess
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Actor> Actors  { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ActorMovies> ActorMovies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define many-to-many relationship
            modelBuilder.Entity<ActorMovies>()
                .HasKey(am => new { am.ActorId, am.MovieId }); // Composite primary key

            modelBuilder.Entity<ActorMovies>()
                .HasOne(am => am.Actor)
                .WithMany(a => a.ActorMovies)
                .HasForeignKey(am => am.ActorId);

            modelBuilder.Entity<ActorMovies>()
                .HasOne(am => am.Movie)
                .WithMany(m => m.ActorMovies)
                .HasForeignKey(am => am.MovieId);
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {

        }

        //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //=> optionsBuilder.UseSqlServer("Data Source=MOUSSAN\\MSSQLSERVERS;Initial Catalog=E-Ticket;Integrated Security=True;TrustServerCertificate=True");


    }
}
