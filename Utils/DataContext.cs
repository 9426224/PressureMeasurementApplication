using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PressureMeasurementApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PressureMeasurementApplication.Utils
{
    public class DataContext : DbContext
    {
        public DbSet<DataModel> DataModel { get; set; }

        public DataContext(DbContextOptions options) :base(options)
        {
            this.Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DataModel>()
                .HasKey(x => x.Data);
        }
    }
}
