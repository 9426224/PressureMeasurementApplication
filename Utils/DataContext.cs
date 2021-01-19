using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PressureMeasurementApplication.Utils
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) :base(options)
        {
            this.Database.Migrate();
        }
    }
}
