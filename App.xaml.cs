using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PressureMeasurementApplication.View;
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

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<Main>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            serviceProvider.GetService<Main>().Show();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            //Configuration = builder

            //ConfigurationServices(Service)
        }
    }
}
