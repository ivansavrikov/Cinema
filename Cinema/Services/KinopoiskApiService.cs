using System.Threading.Tasks;
using System;
using RestSharp;
using System.Text.Json;

namespace Cinema.Services
{
    public class KinopoiskApiService
    {
        private readonly RestClient _restClient;
        private readonly string _baseUrl;
        private readonly string _token;

        public KinopoiskApiService()
        {
            _baseUrl = "https://kinopoiskapiunofficial.tech/";            
            _restClient = new RestClient(_baseUrl);
            _token = Windows.Storage.ApplicationData.Current.LocalSettings.Values["KinopoiskApiKey"] as string;
            _restClient.AddDefaultHeader("X-API-KEY", _token);
        }

        public async Task<string> GetFilmInfoByIdAsync(int id)
        {
            var request = new RestRequest($"api/v2.2/films/{id}", Method.Get);
            var response = await _restClient.ExecuteAsync(request);
            if (!response.IsSuccessful)
                throw new Exception("Error retrieving film: " + response.ErrorMessage);
            string json = response.Content;
            return json;
        }

        // 1 >= page <= 5
        public async Task<string> GetFilmsOnPageAsync(int page)
        {
            var request = new RestRequest($"api/v2.2/films?page={page}", Method.Get);
            var response = await _restClient.ExecuteAsync(request);
            if (!response.IsSuccessful)
                throw new Exception("Error retrieving film: " + response.ErrorMessage);
            string json = response.Content;
            return json;
        }

        public async Task<string> GetAllGenresAsync()
        {
            var request = new RestRequest($"api/v2.2/films/filters", Method.Get);
            var response = await _restClient.ExecuteAsync(request);
            if (!response.IsSuccessful)
                throw new Exception("Error retrieving film: " + response.ErrorMessage);
            string json = response.Content;
            return json;
        }

        public async Task<byte[]> GetImageAsBytesAsync(string imageUrl)
        {
            var request = new RestRequest(imageUrl , Method.Get);
            var response = await _restClient.ExecuteAsync(request);
            if (!response.IsSuccessful)
                throw new Exception("Error retrieving film: " + response.ErrorMessage);
            return response.RawBytes;
        }
    }
}
