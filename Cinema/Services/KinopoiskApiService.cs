using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using RestSharp;
using Cinema.Models.Entities;

namespace Cinema.Services
{
    public class KinopoiskApiService
    {
        private readonly KinopoiskParserService _parser;
        private readonly RestClient _restClient;
        private readonly string _baseUrl;
        private readonly string _token;

        public KinopoiskApiService(KinopoiskParserService parser)
        {
            _parser = parser;
            _baseUrl = "https://kinopoiskapiunofficial.tech/";
            _token = "7d3e436f-f59c-46dd-989d-dac71211f263";
            _restClient = new RestClient(_baseUrl);
            _restClient.AddDefaultHeader("X-API-KEY", _token);
        }

        public async Task<FilmEntity> GetFilmInfoByIdAsync(int id)
        {
            var request = new RestRequest($"api/v2.2/films/{id}", Method.Get);
            var response = await _restClient.ExecuteAsync(request);
            if (!response.IsSuccessful)
                throw new Exception("Error retrieving film: " + response.ErrorMessage);
            FilmEntity film = await _parser.ParseSingleFilm(response.Content);
            return film;
        }

        // 1 >= page <= 5
        public async Task<IEnumerable<FilmEntity>> GetFilmsOnPageAsync(int page)
        {
            var request = new RestRequest($"api/v2.2/films?page={page}", Method.Get);
            var response = await _restClient.ExecuteAsync(request);
            if (!response.IsSuccessful)
                throw new Exception("Error retrieving film: " + response.ErrorMessage);
            var films = await _parser.ParseFilmCollection(response.Content);
            return films;
        }

        public async Task<IEnumerable<GenreEntity>> GetAllGenresAsync()
        {
            var request = new RestRequest($"api/v2.2/films/filters", Method.Get);
            var response = await _restClient.ExecuteAsync(request);
            if (!response.IsSuccessful)
                throw new Exception("Error retrieving film: " + response.ErrorMessage);
            var genres = _parser.ParseGenres(response.Content);
            return genres;
        }
    }
}
