using Cinema.Models.Entities;

namespace Cinema.ViewModels
{
    public class FilmInfoViewModel : BaseViewModel
    {
        private FilmEntity _currentFilm;
        public FilmEntity CurrentFilm
        {
            get => _currentFilm;

            set
            {
                if (value == _currentFilm)
                    return;
                _currentFilm = value;
                OnPropertyChanged();
            }
        }

        public FilmInfoViewModel(FilmsViewModel filmsViewModel)
        {
            filmsViewModel.GetFilmInfoEvent += SetCurrentFilm;
        }

        public void SetCurrentFilm(object film)
        {
            CurrentFilm = film as FilmEntity;
        }
    }
}
