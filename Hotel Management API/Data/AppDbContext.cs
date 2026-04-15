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
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureHotel(modelBuilder);
            ConfigureRoomType(modelBuilder);
            ConfigureRoom(modelBuilder);
            ConfigureAppUser(modelBuilder);
            ConfigureRefreshToken(modelBuilder);
            ConfigureBooking(modelBuilder);
            SeedData(modelBuilder);
        }

        private static void ConfigureHotel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.ToTable("Hotels");

                entity.HasKey(h => h.Id);

                entity.Property(h => h.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(h => h.Address)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(h => h.Phone)
                    .HasMaxLength(20);

                entity.Property(h => h.IsActive)
                    .HasDefaultValue(true);
            });
        }

        private static void ConfigureRoomType(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomType>(entity =>
            {
                entity.ToTable("RoomTypes");

                entity.HasKey(rt => rt.Id);

                entity.Property(rt => rt.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(rt => rt.BasePrice)
                    .HasColumnType("decimal(18,2)");
            });
        }

        private static void ConfigureRoom(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Rooms");

                entity.HasKey(r => r.Id);

                entity.Property(r => r.RoomNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(r => r.PricePerNight)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(r => r.Hotel)
                    .WithMany(h => h.Rooms)
                    .HasForeignKey(r => r.HotelId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.RoomType)
                    .WithMany(rt => rt.Rooms)
                    .HasForeignKey(r => r.RoomTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(r => new { r.HotelId, r.RoomNumber })
                    .IsUnique();
            });
        }

        private static void ConfigureAppUser(ModelBuilder modelBuilder)
        {
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
        }

        private static void ConfigureRefreshToken(ModelBuilder modelBuilder)
        {
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
        }

        private static void ConfigureBooking(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Bookings");

                entity.HasKey(b => b.Id);

                entity.Property(b => b.CustomerName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(b => b.CustomerPhone)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(b => b.CustomerEmail)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(b => b.Status)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(b => b.Note)
                    .HasMaxLength(500);

                entity.Property(b => b.PricePerNight)
                    .HasColumnType("decimal(18,2)");

                entity.Property(b => b.TotalAmount)
                    .HasColumnType("decimal(18,2)");

                entity.Property(b => b.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(b => b.Hotel)
                    .WithMany()
                    .HasForeignKey(b => b.HotelId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.Room)
                    .WithMany()
                    .HasForeignKey(b => b.RoomId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.AppUser)
                    .WithMany()
                    .HasForeignKey(b => b.AppUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(b => b.RoomId);
                entity.HasIndex(b => b.HotelId);
                entity.HasIndex(b => b.AppUserId);
                entity.HasIndex(b => b.Status);
            });
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = 999,
                    Username = "Admin1",
                    FullName = "Quan tri vien 1",
                    Role = "Admin",
                    IsActive = true,
                    TokenVersion = 0,
                    PasswordHash = "$2a$12$ANVS8jHjMjiekgW9gwHjAeR2dCKRP20cx4QUcgY8tmhCx2RCSdUxe"
                }
            );
        }
    }
}