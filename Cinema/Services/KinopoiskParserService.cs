using Cinema.Models.Entities;
using System.Collections.Generic;
using System.Text.Json;

namespace Cinema.Services
{
    public class KinopoiskParserService
    {
        public FilmEntity ParseSingleFilm(string jsonString)
        {
            FilmEntity film = new FilmEntity();
            using (JsonDocument doc = JsonDocument.Parse(jsonString))
            {
                JsonElement root = doc.RootElement;

                film.KinopoiskId = root.GetProperty("kinopoiskId").GetInt32();
                
                string title = root.GetProperty("nameRu").GetString();
                if (string.IsNullOrEmpty(title))
                    title = root.GetProperty("nameOriginal").GetString();

                film.Title = title;

                if (root.TryGetProperty("description", out JsonElement description))
                    film.Description = description.GetString();

                film.PosterUrl = root.GetProperty("posterUrl").GetString();

                film.Year = root.GetProperty("year").GetInt16();

                var genres = new List<GenreEntity>();
                JsonElement jsonGenresArray = root.GetProperty("genres");
                foreach (JsonElement jsonGenre in jsonGenresArray.EnumerateArray())
                {
                    GenreEntity genre = new GenreEntity();
                    genre.Title = jsonGenre.GetProperty("genre").GetString();
                    genres.Add(genre);
                }

                film.Genres = genres;
            }

            return film;
        }

        public List<FilmEntity> ParseFilmCollection(string jsonString)
        {
            List<FilmEntity> films = new List<FilmEntity>();

            using (JsonDocument doc = JsonDocument.Parse(jsonString))
            {
                foreach (JsonElement jsonFilm in doc.RootElement.GetProperty("items").EnumerateArray())
                {
                    FilmEntity film = ParseSingleFilm(jsonFilm.ToString());
                    films.Add(film);
                }
            }

            return films;
        }

        public List<GenreEntity> ParseGenres(string jsonString)
        {
            List<GenreEntity> genres = new List<GenreEntity>();

            using (JsonDocument doc = JsonDocument.Parse(jsonString))
            {
                JsonElement jsonGenresArray = doc.RootElement.GetProperty("genres");
                foreach (JsonElement jsonGenre in jsonGenresArray.EnumerateArray())
                {
                    GenreEntity genre = new GenreEntity();
                    genre.Title = jsonGenre.GetProperty("genre").GetString();
                    genre.KinopoiskId = jsonGenre.GetProperty("id").GetString();
                    genres.Add(genre);
                }
            }

            return genres;
        }
    }
}
