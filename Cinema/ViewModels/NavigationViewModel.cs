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
        public ICommand ItemTappedCommand { get; }

        public NavigationViewModel()
        {
            CurrentPageType = typeof(FilmsPage);
            ItemTappedCommand = new RelayCommand(OnItemTapped);
        }

        private void OnItemTapped(object pageTag)
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
				default:
					break;
			}
		}
    }
}