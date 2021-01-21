using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PressureMeasurementApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PressureMeasurementApplication.Utils
{
    public class SQLiteDataContext : DbContext
    {
        public DbSet<MissionModel> MissionModel { get; set; }
        public DbSet<DataModel> DataModel { get; set; }

        public SQLiteDataContext(DbContextOptions<SQLiteDataContext> options) :base(options)
        {
            this.Database.Migrate();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MissionModel>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasMany(x => x.DataModels)
                    .WithOne().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DataModel>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

        }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SQLiteDataContext>
    {
        public SQLiteDataContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SQLiteDataContext>()
                .UseSqlite("datasource = default.sqlite");

            return new SQLiteDataContext(builder.Options);
        }
    }

}
