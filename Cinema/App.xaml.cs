﻿using System;
using Cinema.Services;
using Cinema.ViewModels;
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
            InitializeServises(ServiceProvider);

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
            services.AddSingleton<CommandAggregator>();
            services.AddSingleton<NavigationViewModel>();
            services.AddSingleton<FilmsViewModel>();
            services.AddSingleton<FavoritesFilmsViewModel>();
            services.AddSingleton<FilmInfoViewModel>();

            services.AddSingleton<KinopoiskParserService>();
            services.AddSingleton<KinopoiskApiService>();
        }

        private void InitializeServises(IServiceProvider serviceProvider)
        {
            serviceProvider.GetRequiredService<CommandAggregator>();
            serviceProvider.GetRequiredService<NavigationViewModel>();
            serviceProvider.GetRequiredService<FilmsViewModel>();
            serviceProvider.GetRequiredService<FavoritesFilmsViewModel>();
            serviceProvider.GetRequiredService<FilmInfoViewModel>();
        }
    }
}
