using Cinema.Models.Entities;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cinema.Services.Repositories
{
    public class KinopoiskRepository(KinopoiskApiService api, KinopoiskParserService parser, DatabaseRepository repository)
    {
        public async Task<FilmEntity> BuildFilm(string json)
        {
            FilmEntity film = parser.ParseFilm(json);
            film.PosterImage = await api.GetImageAsBytesAsync(film.PosterUrl);
            List<GenreEntity> genres = parser.ParseGenres(json);
            foreach (var g in genres)
            {
                var genre = await repository.GetGenreByNameAsync(g.Title) ?? g;
                film.FilmGenres.Add(new FilmGenre { Film = film, Genre = genre });
            }
            return film;
        }

        public async Task<FilmEntity> GetFilmByIdAsync(int kinopoiskId)
        {
            var jsonFilm = await api.GetFilmInfoByIdAsync(kinopoiskId);
            return await BuildFilm(jsonFilm);
        }

        public async Task<List<FilmEntity>> GetAllFilmsAsync()
        {
            List<FilmEntity> films = [];
            for (int i = 1; i <= 2; i++)
            {
                var jsonString = await api.GetFilmsOnPageAsync(i);
                using (var json = JsonDocument.Parse(jsonString))
                {
                    var root = json.RootElement;
                    var jsonFilmsArray = root.GetProperty("items").EnumerateArray();
                    foreach (JsonElement jsonFilm in jsonFilmsArray)
                    {
                        FilmEntity film = await BuildFilm(jsonFilm.GetRawText());
                        films.Add(film);
                    }
                }
            }
            return films;
        }

        public async Task<List<GenreEntity>> GetAllGenresAsync()
        {
            var json = await api.GetAllGenresAsync();
            List<GenreEntity> genres = parser.ParseGenres(json);
            return genres;
        }
    }
}