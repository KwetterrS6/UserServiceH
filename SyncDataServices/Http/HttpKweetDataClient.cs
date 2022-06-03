using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using UserService.Dtos;

namespace UserService.SyncDataServices.Http
{
    public class HttpKweetDataClient : IKweetDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpKweetDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }


        public async Task SendUserToKweet(UserReadDto user)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(user),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_configuration["KweetService"]}", httpContent);

            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to KweetService was OK!");
            }
            else
            {
                Console.WriteLine("--> Sync POST to KweetService was NOT OK!");
            }
        }
    }
}