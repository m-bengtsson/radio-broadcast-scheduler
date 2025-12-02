using Microsoft.EntityFrameworkCore;

public class RadioSchedulerContext : DbContext
{
   public RadioSchedulerContext(DbContextOptions<RadioSchedulerContext> options) : base(options) { }
   public DbSet<BroadcastContent> Broadcasts { get; set; }
   public DbSet<Contributor> Contributors { get; set; }
   public DbSet<Payment> Payments { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      // Configure inheritance using Table-per-Hierarchy (TPH)
      modelBuilder.Entity<BroadcastContent>()
          .HasDiscriminator<string>("BroadcastType")
          .HasValue<Music>("Music")
          .HasValue<Reportage>("Reportage")
          .HasValue<LiveSession>("LiveSession");

      base.OnModelCreating(modelBuilder);
   }
}