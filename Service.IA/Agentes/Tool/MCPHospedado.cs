using Microsoft.Extensions.AI;
using System.Text;
using System.Text.Json;

namespace Service.IA.Agentes.Tool
{
    public class MCPHospedado(string name, string endpointUrl, string token) : AITool
    {
        private readonly HttpClient _httpClient;

        public async Task<string> InvokeAsync(object input)
        {
            var payload = new
            {
                jsonrpc = "2.0",
                method = "execute",
                @params = input,
                id = 1
            };

            var json = JsonSerializer.Serialize(payload);
            var request = new HttpRequestMessage(HttpMethod.Post, endpointUrl)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
