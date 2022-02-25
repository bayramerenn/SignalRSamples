using Microsoft.EntityFrameworkCore;

namespace CovidChart.API.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
  
        public DbSet<Covid> Covids{ get; set; }
        public DbSet<CovidChartPivot> CovidChartPivot { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CovidChartPivot>(mb => mb.HasNoKey());
            base.OnModelCreating(modelBuilder);
        }
    }
}
