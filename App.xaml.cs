using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PressureMeasurementApplication.Utils;
using PressureMeasurementApplication.View;
using PressureMeasurementApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PressureMeasurementApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ServiceProvider serviceProvider { get; private set; }
        public IConfigurationRoot Configuration { get; private set; }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<Main>();

            services.AddDbContext<SQLiteDataContext>(options => options
                .UseSqlite(Configuration.GetConnectionString("SqlConnection")), ServiceLifetime.Transient);
            
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MissionViewModel>();
            services.AddSingleton<SerialPortViewModel>();
            
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);            
            serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetService<Main>().Show();

        }
    }
}
