using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json; // Para GetFromJsonAsync
using KoguiApp.Models;

namespace KoguiApp.Services
{
    class ColorApiService : IColorApiService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string ApiBaseUrl = "https://www.thecolorapi.com/id?hex=";

        public async Task<string> GetColorNameAsync(string hex)
        {
            // Remove o '#' para a URL da API
            var cleanHex = hex.Replace("#", "");
            var response = await _httpClient.GetFromJsonAsync<ColorApiResponse>($"{ApiBaseUrl}{cleanHex}");
            return response?.name?.value;
        }
    }
}
