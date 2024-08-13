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
        private NavigationManager _navigationManager;
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
        public NavigationViewModel(CommandAggregator commandAggregator, NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            _commandAggregator = commandAggregator;
            _commandAggregator.RegisterCommand(nameof(NavigateCommand), new RelayCommand(Navigate));

            CurrentPageType = typeof(FilmsPage);
        }

        private void Navigate(object PagesTagsEnumItem)
        {
            PagesTagsEnum tag;
            if (byte.TryParse(PagesTagsEnumItem.ToString(), out byte tagByte))
                tag = (PagesTagsEnum)tagByte;
            else
                tag = (PagesTagsEnum)PagesTagsEnumItem;

            CurrentPageType = tag switch
            {
                PagesTagsEnum.LocalsFilmsPage => typeof(FilmsPage),
                PagesTagsEnum.FavoritsFilmsPage => typeof(UserFilmsPage),
                PagesTagsEnum.FiltersPage => typeof(FiltersPage),
                PagesTagsEnum.FilmDetailsPage => typeof(FilmDetailsPage),
                _ => throw new ArgumentException("uncorrect tag of page to navigate"),
            };

            _navigationManager.PreviousPage = _navigationManager.CurrentPage;
            _navigationManager.CurrentPage = tag;
        }
    }
}