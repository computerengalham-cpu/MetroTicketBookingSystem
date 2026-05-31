using Microsoft.EntityFrameworkCore;
using MetroTicketSystem.Models;

namespace MetroTicketSystem.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Train> Trains { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=MetroTicketDb;Trusted_Connection=True;");
        }

        // أضف هذا الجزء لتحديد دقة حقل السعر والتخلص من التحذير
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .Property(t => t.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}