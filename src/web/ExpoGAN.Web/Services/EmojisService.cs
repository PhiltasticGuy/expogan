using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExpoGAN.Web.Services
{
    public class EmojisService
    {
        private readonly HttpClient _client;

        public EmojisService(HttpClient client)
        {
            client.BaseAddress = new Uri("http://expogan.api");

            client.DefaultRequestHeaders.Add(
                "Accept",
                "text/plain");
            client.DefaultRequestHeaders.Add(
                "User-Agent", 
                "ExpoGAN.Web");

            _client = client;
        }

        public async Task<string> GenerateEmojisAsync(string id)
        {
            var parameters = new Dictionary<string, string>
            {
                ["id"] = id
            };

            using var response =
                await _client.PostAsync("generate", new FormUrlEncodedContent(parameters));

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> InterpolateEmojisAsync(string id, string firstEmojiId, string secondEmojiId)
        {
            var parameters = new Dictionary<string, string>
            {
                ["id"] = id,
                ["firstId"] = firstEmojiId,
                ["secondId"] = secondEmojiId
            };

            using var response =
                await _client.PostAsync("interpolate", new FormUrlEncodedContent(parameters));

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

    }
}
