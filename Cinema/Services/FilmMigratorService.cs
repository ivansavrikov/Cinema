using Cinema.Models.Entities;
using Cinema.Services.Repositories;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Cinema.Services
{
    public class FilmMigratorService(KinopoiskApiService api, KinopoiskParserService parser, DatabaseRepository repository)
    {
        public async Task MigrateFilmAsync(int kinopoiskId)
        {
            string jsonFilm = await api.GetFilmInfoByIdAsync(kinopoiskId);
            FilmEntity film = await PrepareToMigrateAsync(jsonFilm);
            film.IsFullySynchronized = true;
            film.LastSync = DateTime.Now;
            await repository.AddFilmAsync(film);
        }

        public async Task<FilmEntity> PrepareToMigrateAsync(string jsonFilm)
        {
            FilmEntity film = parser.ParseFilm(jsonFilm);
            List<GenreEntity> genres = parser.ParseGenres(jsonFilm);
            foreach (var g in genres)
            {
                var genre = await repository.GetGenreByNameAsync(g.Title);
                if (genre == null)
                    throw new Exception($"local db does not contain genre '{genre.Title}'");
                film.FilmGenres.Add(new FilmGenre { Film = film, Genre = genre });
            }

            film.PosterImage = await api.GetImageAsBytesAsync(film.PosterUrl);
            return film;
        }
    }
}
