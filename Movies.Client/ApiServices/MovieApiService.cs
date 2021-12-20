using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Movies.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Client.ApiServices
{
    public class MovieApiService : IMovieApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MovieApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            //Way 1
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "/movies");

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var movieList = JsonConvert.DeserializeObject<List<Movie>>(content);
            return movieList;


            //Way 2
            //// 1. Get token from Identity Server

            //// "Retrive" our api credentials. This must be registered on Identity Server
            //var apiClientCredentials = new ClientCredentialsTokenRequest
            //{
            //    Address = "https://localhost:5005/connect/token",

            //    ClientId = "movieClient",
            //    ClientSecret = "secret",

            //    //This is the scope our Protected API requirs
            //    Scope = "movieAPI"
            //};


            //// Create a new HttpClient to talk to our IdentityServer (localhost:5005)
            //var client = new HttpClient();

            //// Check if we can reach Discovery document 
            //var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5005");
            //if (disco.IsError)
            //{
            //    return null;
            //}

            //// Authenticate and get an access token from IdentityServer 
            //var tokenResponse = await client.RequestClientCredentialsTokenAsync(apiClientCredentials);
            //if (tokenResponse.IsError)
            //{
            //    return null;
            //}

            //// 2. Send request to protected API

            ////Another HttpClient for comunicating with our Protected API
            //var apiClient = new HttpClient();

            ////Set the access token in the request Authorization: Bearer <token>
            //apiClient.SetBearerToken(tokenResponse.AccessToken);

            ////Send request to our Protected API
            //var response = await apiClient.GetAsync("https://localhost:5001/api/movies");
            //response.EnsureSuccessStatusCode();


            //// 3. Deserialize Object to MovieList
            //var content = await response.Content.ReadAsStringAsync();
            //List<Movie> movieList = JsonConvert.DeserializeObject<List<Movie>>(content);
            //return movieList;   
        }

        public async Task<Movie> GetMovie(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"/movies/{id}");

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var movie = JsonConvert.DeserializeObject<Movie>(content);
            return movie;
        }

        public async Task<Movie> CreateMovie(Movie movie)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "/movies/");

            var response = await httpClient.PostAsync(request.RequestUri, movie, new JsonMediaTypeFormatter());

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var newMovie = JsonConvert.DeserializeObject<Movie>(content);
            return newMovie;
        }

        public async Task DeleteMovie(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var request = new HttpRequestMessage(
                HttpMethod.Delete,
                $"/movies/{id}");

            var response = await httpClient.DeleteAsync(request.RequestUri);

            response.EnsureSuccessStatusCode();
        }

        public async Task<Movie> UpdateMovie(Movie movie)
        {
            var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");

            var request = new HttpRequestMessage(
                HttpMethod.Put,
                $"/movies/{movie.Id}");

            var response = await httpClient.PutAsync(request.RequestUri, movie, new JsonMediaTypeFormatter());

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var newMovie = JsonConvert.DeserializeObject<Movie>(content);
            return newMovie;
        }

        public async Task<UserInfoViewModel> GetUserInfo()
        {
            var idpClient = _httpClientFactory.CreateClient("IDPClient");
            var metaDataResponse = await idpClient.GetDiscoveryDocumentAsync();
            if (metaDataResponse.IsError)
            {
                throw new HttpRequestException("Something went wrong while requesting access token");
            }

            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            var userInfoResponse = await idpClient.GetUserInfoAsync(
                new UserInfoRequest
                {
                    Address = metaDataResponse.UserInfoEndpoint,
                    Token = accessToken
                }
            );
            if (userInfoResponse.IsError) {
                throw new HttpRequestException("Something went wrong while getting user info");
            }

            var userInfoDictionary = new Dictionary<string, string>();

            foreach (var claim in userInfoResponse.Claims)
            {
                userInfoDictionary.Add(claim.Type, claim.Value);
            }

            return new UserInfoViewModel(userInfoDictionary);
        }
    }
}
