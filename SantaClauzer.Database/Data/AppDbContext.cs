using Microsoft.EntityFrameworkCore;
using SantaClauzer.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantaClauzer.Database.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<PresentGroupModel> PresentGroups { get; set; }
        public DbSet<WishListModel> WishLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=STEFAN\\SQLEXPRESS;Database=SantaClauzerDb;Trusted_Connection=true;TrustServerCertificate=true");
        }
    }
}
