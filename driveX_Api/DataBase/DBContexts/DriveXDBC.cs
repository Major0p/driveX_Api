using driveX_Api.Models.File;
using driveX_Api.Models.User;
using Microsoft.EntityFrameworkCore;

namespace driveX_Api.DataBase.DBContexts
{
    public class DriveXDBC : DbContext
    {
        public DriveXDBC(DbContextOptions<DriveXDBC> options) : base(options){}

        //Db sets
        public DbSet<UserInfo> Users { get; set; }
        public DbSet<Details> FileDetails { get; set; }
        public DbSet<Storage> FileStorage { get; set; }
        public DbSet<SharedDetails> SharedDetails { get; set; }


        //Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(u=>u.Id);
                entity.Property(u => u.Id).HasColumnType("Varchar(100)").IsRequired();
                entity.Property(u=>u.FirstName).HasColumnType("Varchar(100)").IsRequired();
                entity.Property(u => u.LastName).HasColumnType("varchar(100)");
                entity.Property(u => u.Password).HasColumnType("varchar(100)");
                entity.Property(u => u.Email).HasColumnType("varchar(255)");
                entity.Property(u => u.Phone).HasColumnType("varchar(15)");

                //one-to-many: user -> Details
                entity.HasMany(u => u.FileDetails)
                      .WithOne(d => d.User)
                      .HasForeignKey(d => d.UserId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Details>(entity =>
            {
                entity.ToTable("FileDetails");

                entity.HasKey(d=>d.Id);
                entity.Property(d => d.Id).HasColumnType("Varchar(100)").IsRequired();
                entity.Property(d => d.Name).HasColumnType("varchar(255)");
                entity.Property(d => d.Extension).HasColumnType("varchar(10)");
                entity.Property(d => d.Path).HasColumnType("varchar(max)").IsRequired();
                entity.Property(d => d.Label).HasColumnType("varchar(255)");
                entity.Property(d => d.Size).HasColumnType("bigint");
                entity.Property(d => d.ParentId).HasColumnType("Varchar(100)").IsRequired();
                entity.Property(d => d.UserId).HasColumnType("Varchar(100)").IsRequired();
                entity.Property(d => d.Trashed).HasColumnType("bit");
                entity.Property(d => d.IsFile).HasColumnType("bit");
                entity.Property(d => d.Starred).HasColumnType("bit");

                //one-to-one : details -> storage
                entity.HasOne(d => d.Storage)
                      .WithOne(s => s.Details)
                      .HasForeignKey<Storage>(s => s.Id)
                      .OnDelete(DeleteBehavior.Cascade);

                //many to many : details <-> user (shared with)
                entity.HasMany(d => d.SharedDetails)
                      .WithOne(sd => sd.Details)
                      .HasForeignKey(sd => sd.DetailsId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Storage>(entity =>
            {
                entity.ToTable("FileStorages");

                entity.HasKey(s=>s.Id);
                entity.Property(s => s.Id).HasColumnType("Varchar(100)").IsRequired();
                entity.Property(s => s.Data).HasColumnType("varbinary(max)").IsRequired();
            });

            modelBuilder.Entity<SharedDetails>(entity =>
            {
                entity.ToTable("SharedDetails");

                entity.HasKey(sd => new { sd.UserId, sd.DetailsId });
                entity.Property(sd => sd.UserId).HasColumnType("Varchar(100)").IsRequired();
                entity.Property(sd => sd.SharedDate).HasColumnType("datetime");

                entity.HasOne(sd => sd.User)
                      .WithMany(u => u.SharedDetails)
                      .HasForeignKey(sd => sd.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(sd => sd.Details)
                      .WithMany(d => d.SharedDetails)
                      .HasForeignKey(sd=>sd.DetailsId)
                      .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}

