using Cinema.Helpers;
using Cinema.Views;
using System;
using System.Windows.Input;

namespace Cinema.ViewModels
{
    public class NavigationViewModel : BaseViewModel
    {
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
        public ICommand NavigateCommand { get; }

        public NavigationViewModel()
        {
            CurrentPageType = typeof(FilmsPage);
            NavigateCommand = new RelayCommand(Navigate);
        }

        private void Navigate(object pageTag)
        {
			switch (pageTag.ToString())
			{
				case "filmsPage":
					CurrentPageType = typeof(FilmsPage);
					break;

				case "favoritesPage":
                    CurrentPageType = typeof(FavoritesPage);
                    break;

				case "filtersPage":
                    CurrentPageType = typeof(FiltersPage);
                    break;

                case "filmInfoPage":
                    CurrentPageType = typeof(FilmInfoPage);
                    break;
				default:
					break;
			}
		}
    }
}