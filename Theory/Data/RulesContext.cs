
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using RepositoryDb.Models;

namespace Repository.Data
{
    public class RulesContext : DbContext
    {
        public DbSet<Rule> Rules { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Example> Examples { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var rootFolder = AppDomain.CurrentDomain.BaseDirectory;
            var dbName = "Theory.db";
            var path = rootFolder + dbName;

            optionsBuilder.UseSqlite($"Data Source={path}");
        }
    }
}
