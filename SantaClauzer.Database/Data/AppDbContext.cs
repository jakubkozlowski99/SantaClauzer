using Microsoft.EntityFrameworkCore;
using SantaClauzer.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaClauzer.Database.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<PresentGroupModel> PresentGroups { get; set; }
        public DbSet<WishListModel> WishLists { get; set; }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<UserRoleModel> UserRoles { get; set; }
        public DbSet<RefreshTokenModel> RefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=STEFAN\\SQLEXPRESS;Database=SantaClauzerDb;Trusted_Connection=true;TrustServerCertificate=true");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // PresentGroup.Creator -> User : do NOT cascade delete (prevent multiple cascade paths)
            modelBuilder.Entity<PresentGroupModel>()
                .HasOne(p => p.Creator)
                .WithMany(u => u.PresentGroups)
                .HasForeignKey(p => p.CreatorId)
                .OnDelete(DeleteBehavior.Restrict); // prevents cascade path conflicts

            // RefreshToken -> User : allow cascade or restrict depending on your semantics
            modelBuilder.Entity<RefreshTokenModel>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // other relationships configured similarly...
        }
    }
}
