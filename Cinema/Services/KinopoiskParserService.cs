using Cinema.Models.Entities;
using System.Collections.Generic;
using System.Text.Json;

namespace Cinema.Services
{
    public class KinopoiskParserService
    {
        public FilmEntity ParseFilm(string jsonString)
        {
            FilmEntity film = new FilmEntity();
            using (var jsonFilm = JsonDocument.Parse(jsonString))
            {
                JsonElement root = jsonFilm.RootElement;
                film.KinopoiskId = root.GetProperty("kinopoiskId").GetInt32(); 
                string title = root.GetProperty("nameRu").GetString();
                if (string.IsNullOrEmpty(title))
                    title = root.GetProperty("nameOriginal").GetString();
                film.Title = title;
                if (root.TryGetProperty("description", out JsonElement description))
                    film.Description = description.GetString();
                film.PosterUrl = root.GetProperty("posterUrl").GetString();
                film.Year = root.GetProperty("year").GetInt16();
            }

            return film;
        }

        public List<GenreEntity> ParseGenres(string jsonString)
        {
            List<GenreEntity> genres = [];

            using (var json = JsonDocument.Parse(jsonString))
            {
                JsonElement jsonGenresArray = json.RootElement.GetProperty("genres");
                foreach (JsonElement jsonGenre in jsonGenresArray.EnumerateArray())
                {
                    GenreEntity genre = new();
                    genre.Title = jsonGenre.GetProperty("genre").GetString();
                    if (jsonGenre.TryGetProperty("id", out JsonElement genreKinopoiskId))
                        genre.KinopoiskId = genreKinopoiskId.GetInt32();

                    genres.Add(genre);
                }
            }

            return genres;
        }
    }
}
