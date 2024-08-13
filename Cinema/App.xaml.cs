using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Cinema.Helpers;
using Cinema.Models.Database;
using Cinema.Services;
using Cinema.Services.Repositories;
using Cinema.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Cinema
{
    sealed partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            //Предоставленный ключ "7d3e436f-f59c-46dd-989d-dac71211f263"
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["KinopoiskApiKey"] = "490e41e8-3cff-4c89-aac0-44f43dbdb20e"; //Личный ключ
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            Task.Run(async () => await InitializeDatabase()).GetAwaiter().GetResult();
            InitializeServises();

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(NavigationPage), e.Arguments);
                }
                Window.Current.Activate();
            }
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<NavigationViewModel>();
            services.AddSingleton<FilmsViewModel>();
            services.AddSingleton<FavoritesViewModel>();
            services.AddSingleton<FilmDetailsViewModel>();
            services.AddSingleton<FiltersViewModel>();
            
            services.AddDbContext<DatabaseContext>();

            services.AddSingleton<NavigationManager>();
            services.AddSingleton<CommandAggregator>();
            services.AddSingleton<KinopoiskParserService>();
            services.AddSingleton<KinopoiskApiService>();
            services.AddSingleton<DatabaseRepository>();
            services.AddSingleton<KinopoiskRepository>();
            services.AddSingleton<DatabaseFillService>();
            services.AddSingleton<FilmMigratorService>();
        }

        private void InitializeServises()
        {
            ServiceProvider.GetRequiredService<CommandAggregator>();
            ServiceProvider.GetRequiredService<NavigationViewModel>();
            ServiceProvider.GetRequiredService<FilmsViewModel>();
            ServiceProvider.GetRequiredService<FavoritesViewModel>();
            ServiceProvider.GetRequiredService<FilmDetailsViewModel>();
            ServiceProvider.GetRequiredService<FiltersViewModel>();
        }

        private async Task InitializeDatabase()
        {
            var dbContext = ServiceProvider.GetRequiredService<DatabaseContext>();
            await dbContext.Database.EnsureCreatedAsync();
            if (!dbContext.Users.Any())
            {
                var fillDatabaseService = ServiceProvider.GetRequiredService<DatabaseFillService>();
                try
                {
                    await fillDatabaseService.FillStartDataToDatabaseAsync();
                }
                catch (Exception)
                {
                    await dbContext.Database.EnsureDeletedAsync();
                    Debug.WriteLine("Для первого запуска приложения необходим интернет и доступ к API");
                    throw new Exception("Для первого запуска приложения необходим интернет и доступ к API");
                }
            }
        }
    }
}
