using konzola.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace konzola.Services
{
    public class TimeService
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==";
        public TimeService()
        {
            _httpClient = new HttpClient();
        }
        public async Task<List<Time>> GetTimesAsync()
        {
            var response = await _httpClient.GetAsync(ApiUrl);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response:");
            Console.WriteLine(content);
            var entries = System.Text.Json.JsonSerializer.Deserialize<List<Time>>(content);
            return entries;
        }
    }
}
