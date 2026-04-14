using BCrypt.Net;
using Hotel_Management_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management_API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected AppDbContext()
        {
        }
        
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Room>()
                .Property(r => r.PricePerNight)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<RoomType>()
                .Property(rt => rt.BasePrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Room>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Room>()
                .HasOne(r => r.RoomType)
                .WithMany(rt => rt.Rooms)
                .HasForeignKey(r => r.RoomTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.ToTable("AppUsers");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(x => x.Username)
                    .IsUnique();

                entity.Property(x => x.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(x => x.FullName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Role)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);

                entity.Property(x => x.TokenVersion)
                    .HasDefaultValue(0);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshTokens");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Token)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasIndex(x => x.Token)
                    .IsUnique();

                entity.Property(x => x.IsRevoked)
                    .HasDefaultValue(false);

                entity.HasOne(x => x.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AppUser>().HasData(
                    new AppUser {Id = 999, Username = "Admin1", FullName = "Quan tri vien 1", Role = "Admin", IsActive = true, PasswordHash = "$2a$12$ANVS8jHjMjiekgW9gwHjAeR2dCKRP20cx4QUcgY8tmhCx2RCSdUxe" }
            );
        }
    }
}
