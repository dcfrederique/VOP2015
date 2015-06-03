using Carcassonne_Web.Models;
using Carcassonne_Web.Models.GameObj;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Carcassonne_Web.DAL
{
    public class CarcassonneContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Turn> Turns { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<PlayerGameData> PlayerData { get; set; }
        public DbSet<Log> Logs { get; set; }

        public CarcassonneContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static CarcassonneContext Create()
        {
            return new CarcassonneContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

            modelBuilder.Entity<ApplicationUser>().Property(x => x.UserName).HasMaxLength(50);
            modelBuilder.Entity<ApplicationUser>().Ignore(x => x.PhoneNumber);
            modelBuilder.Entity<ApplicationUser>().Ignore(x => x.PhoneNumberConfirmed);
            modelBuilder.Entity<ApplicationUser>().Ignore(x => x.TwoFactorEnabled);

            modelBuilder.Entity<Room>().HasMany(x => x.Players).WithMany(x => x.Rooms);

            modelBuilder.Ignore<Score>();

        }

    }
}