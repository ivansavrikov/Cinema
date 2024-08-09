using Cinema.Helpers;
using Cinema.Services;
using Cinema.Views;
using System;
using System.Windows.Input;

namespace Cinema.ViewModels
{
    public class NavigationViewModel : BaseViewModel
    {
        private CommandAggregator _commandAggregator;
        private Type _currentPageType;
        public Type CurrentPageType
		{
			get => _currentPageType;
			private set
			{
                if (value == _currentPageType)
                    return;
                _currentPageType = value;
                OnPropertyChanged();
            }
		}
        public ICommand NavigateCommand => _commandAggregator.GetCommand(nameof(NavigateCommand));
        public NavigationViewModel(CommandAggregator commandAggregator)
        {
            _commandAggregator = commandAggregator;
            _commandAggregator.RegisterCommand(nameof(NavigateCommand), new RelayCommand(Navigate));

            CurrentPageType = typeof(FilmsPage);
        }

        private void Navigate(object pageTag)
        {
			switch (pageTag.ToString())
			{
				case "filmsPage":
					CurrentPageType = typeof(FilmsPage);
					break;

				case "favoritesPage":
                    CurrentPageType = typeof(UserFilmsPage);
                    break;

				case "filtersPage":
                    CurrentPageType = typeof(FiltersPage);
                    break;

                case "filmInfoPage":
                    CurrentPageType = typeof(FilmDetailsPage);
                    break;
				default:
					break;
			}
		}
    }
}