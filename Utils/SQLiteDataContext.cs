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
        public DbSet<DataModel> DataModel { get; set; }

        public SQLiteDataContext(DbContextOptions options) :base(options)
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
