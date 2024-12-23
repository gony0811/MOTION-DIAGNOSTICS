using EPLE.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace EPLE.Data
{
    public class DataContext : DbContext
    {
        public DbSet<DeviceConfigEntity> DeviceConfig { get; set; }

        public DbSet<DataConfigEntity> DataConfig { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeviceConfigEntity>(eb =>
            {
                eb.ToTable("Device");
                eb.Property(e => e.DeviceName);
                eb.HasKey(e => e.DeviceName);

                // 릴레이션은 굳이 맺지 않음...
                // eb.HasMany(e => e.DataEntity).WithMany();
            });

            modelBuilder.Entity<DataConfigEntity>(eb =>
            {
                eb.ToTable("Data");
                eb.Property(e => e.Name).ValueGeneratedNever();
                eb.HasKey(e => e.Name);

                eb.HasIndex(e => new { e.Name }).IsUnique();

                eb.Property(e => e.Type).HasConversion<string>();
                eb.Property(e => e.Direction).HasConversion<string>();
            });
        }
    }
}
