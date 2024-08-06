using System;
using System.Linq;
using System.Threading.Tasks;
using Cinema.Models.Database;
using Cinema.Services;
using Cinema.Services.Repositories;
using Cinema.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
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
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            Task.Run(async () => await InitializeDatabase()).GetAwaiter().GetResult();
            InitializeServises();

            Frame rootFrame = Window.Current.Content as Frame;

            // Не повторяйте инициализацию приложения, если в окне уже имеется содержимое,
            // только обеспечьте активность окна
            if (rootFrame == null)
            {
                // Создание фрейма, который станет контекстом навигации, и переход к первой странице
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Загрузить состояние из ранее приостановленного приложения
                }

                // Размещение фрейма в текущем окне
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // Если стек навигации не восстанавливается для перехода к первой странице,
                    // настройка новой страницы путем передачи необходимой информации в качестве параметра
                    // навигации
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Обеспечение активности текущего окна
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
            //TODO: Сохранить состояние приложения и остановить все фоновые операции
            deferral.Complete();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            //TODO: Подумать над временем жизни сервисов!!!!
            services.AddSingleton<CommandAggregator>();
            services.AddSingleton<NavigationViewModel>();
            services.AddSingleton<FilmsViewModel>();
            services.AddSingleton<FavoritesFilmsViewModel>();
            services.AddSingleton<FilmInfoViewModel>();

            services.AddDbContext<DatabaseContext>();

            services.AddSingleton<KinopoiskParserService>();
            services.AddSingleton<KinopoiskApiService>();
            services.AddSingleton<DatabaseRepository>();
            services.AddSingleton<DatabaseFillService>();
        }

        private void InitializeServises()
        {
            ServiceProvider.GetRequiredService<CommandAggregator>();
            ServiceProvider.GetRequiredService<NavigationViewModel>();
            ServiceProvider.GetRequiredService<FilmsViewModel>();
            ServiceProvider.GetRequiredService<FavoritesFilmsViewModel>();
            ServiceProvider.GetRequiredService<FilmInfoViewModel>();
        }

        private async Task InitializeDatabase()
        {
            var dbContext = ServiceProvider.GetRequiredService<DatabaseContext>();
            await dbContext.Database.EnsureCreatedAsync();
            if (!dbContext.Users.Any())
            {
                var fillService = ServiceProvider.GetRequiredService<DatabaseFillService>();
                await fillService.FillStartDataToDatabaseAsync();
            }
        }
    }
}
