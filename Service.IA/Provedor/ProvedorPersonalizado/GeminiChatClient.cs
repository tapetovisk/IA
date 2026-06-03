using Microsoft.Extensions.AI;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Service.IA.Provedor.ProvedorPersonalizado
{
    public class GeminiChatClient : IChatClient
    {
        private readonly string _apiKey;
        private readonly string _model;
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://generativelanguage.googleapis.com/v1beta/models";

        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public ChatClientMetadata Metadata { get; }

        public GeminiChatClient(string apiKey, string model = "gemini-2.0-flash")
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _model = model;
            _httpClient = new HttpClient();
            Metadata = new ChatClientMetadata("Google Gemini", new Uri(BaseUrl), model);
        }

        public GeminiChatClient(string apiKey, HttpClient httpClient, string model = "gemini-2.0-flash")
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _model = model;
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            Metadata = new ChatClientMetadata("Google Gemini", new Uri(BaseUrl), model);
        }

        public async Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
        {
            var requestBody = BuildRequestBody(messages, options);
            var url = $"{BaseUrl}/{_model}:generateContent?key={_apiKey}";

            using var response = await _httpClient.PostAsJsonAsync(url, requestBody, JsonOptions, cancellationToken);
            response.EnsureSuccessStatusCode();

            var geminiResponse = await response.Content.ReadFromJsonAsync<GeminiResponse>(JsonOptions, cancellationToken)
                ?? throw new InvalidOperationException("Resposta vazia da API Gemini.");

            var text = geminiResponse.Candidates?[0].Content?.Parts?[0].Text ?? string.Empty;

            var c = new OpenAIModel() { Data = text };
            var TextString = JsonSerializer.Serialize(c, JsonOptions);

            return new ChatResponse(new ChatMessage(ChatRole.Assistant, TextString))
            {
                ModelId = _model,
                FinishReason = ChatFinishReason.Stop
            };
        }

        public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
            IEnumerable<ChatMessage> messages,
            ChatOptions? options = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var requestBody = BuildRequestBody(messages, options);
            var url = $"{BaseUrl}/{_model}:streamGenerateContent?alt=sse&key={_apiKey}";

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(requestBody, options: JsonOptions)
            };
            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var reader = new System.IO.StreamReader(stream);

            while (!reader.EndOfStream)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var line = await reader.ReadLineAsync(cancellationToken);
                if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data:"))
                    continue;

                var json = line["data:".Length..].Trim();
                if (json == "[DONE]") break;

                var chunk = JsonSerializer.Deserialize<GeminiResponse>(json, JsonOptions);
                var text = chunk?.Candidates?[0].Content?.Parts?[0].Text;
                if (text is not null)
                    yield return new ChatResponseUpdate(ChatRole.Assistant, text) { ModelId = _model };
            }
        }

        public object? GetService(Type serviceType, object? serviceKey = null)
        {
            if (serviceType == typeof(IChatClient)) return this;
            return null;
        }

        public void Dispose() => _httpClient.Dispose();

        private static GeminiRequest BuildRequestBody(IEnumerable<ChatMessage> messages, ChatOptions? options)
        {
            var contents = messages.Select(m => new GeminiContent
            {
                Role = m.Role == ChatRole.User ? "user" : "model",
                Parts = [new GeminiPart { Text = m.Text ?? string.Empty }]
            }).ToList();

            GeminiInstruction Instruction = new GeminiInstruction();
            if (!string.IsNullOrEmpty(options.Instructions))
            {
                Instruction = new GeminiInstruction
                {
                    Parts = [new GeminiPart { Text = options.Instructions }]
                };
            }

            GeminiGenerationConfig? config = null;
            if (options is not null)
            {
                config = new GeminiGenerationConfig
                {
                    MaxOutputTokens = options.MaxOutputTokens,
                    Temperature = (double?)options.Temperature,
                    TopP = (double?)options.TopP,
                    StopSequences = options.StopSequences?.ToList()
                };
            }

            return new GeminiRequest { Contents = contents, SystemInstruction = Instruction, GenerationConfig = config };
        }

        private sealed class GeminiRequest
        {
            [JsonPropertyName("contents")]
            public List<GeminiContent> Contents { get; set; } = [];

            [JsonPropertyName("tools")]
            public List<GeminiTools> Tools { get; set; } = [];

            [JsonPropertyName("system_instruction")]
            public GeminiInstruction SystemInstruction { get; set; } = new();

            [JsonPropertyName("generationConfig")]
            public GeminiGenerationConfig? GenerationConfig { get; set; }
        }

        private sealed class GeminiInstruction
        {
            [JsonPropertyName("parts")]
            public List<GeminiPart> Parts { get; set; } = [];
        }

        private sealed class GeminiContent
        {
            [JsonPropertyName("role")]
            public string Role { get; set; } = "user";

            [JsonPropertyName("parts")]
            public List<GeminiPart> Parts { get; set; } = [];
        }

        private class GeminiPart
        {
            [JsonPropertyName("text")]
            public string Text { get; set; } = string.Empty;
        }

        private class GeminiTools
        {
            [JsonPropertyName("functionDeclarations")]
            public List<Functiondeclaration> FunctionDeclarations { get; set; } = [];
        }

        private class GeminiPartFile : GeminiPart
        {
            [JsonPropertyName("inline_data")]
            public List<GeminiFile> InlineData { get; set; } = [];

            [JsonPropertyName("file_data")]
            public List<GeminiFileUri> FileData { get; set; } = [];
        }

        private class GeminiFileUri : GeminiPart
        {
            [JsonPropertyName("data")]
            public string Data { get; set; } = string.Empty;

            [JsonPropertyName("file_uri")]
            public string FileUri { get; set; } = string.Empty;
        }

        private sealed class GeminiFile
        {
            [JsonPropertyName("mime_type")]
            public string MimeType { get; set; } = string.Empty;

            [JsonPropertyName("data")]
            public string Data { get; set; } = string.Empty;
        }

        private sealed class GeminiGenerationConfig
        {
            [JsonPropertyName("maxOutputTokens")]
            public int? MaxOutputTokens { get; set; }

            [JsonPropertyName("temperature")]
            public double? Temperature { get; set; }

            [JsonPropertyName("topP")]
            public double? TopP { get; set; }

            [JsonPropertyName("stopSequences")]
            public List<string>? StopSequences { get; set; }
        }

        private sealed class GeminiResponse
        {
            [JsonPropertyName("candidates")]
            public List<GeminiCandidate>? Candidates { get; set; }
        }

        private sealed class GeminiCandidate
        {
            [JsonPropertyName("content")]
            public GeminiContent? Content { get; set; }
        }

        private class OpenAIModel
        {
            [JsonPropertyName("data")]
            public object Data { get; set; }
        }

        public class Functiondeclaration
        {
            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;
            [JsonPropertyName("description")]
            public string Description { get; set; } = string.Empty;
            [JsonPropertyName("parameters")]
            public Parameters Parameters { get; set; } = new();
        }

        public class Parameters
        {
            [JsonPropertyName("type")]
            public string Type { get; set; } = string.Empty;
            [JsonPropertyName("properties")]
            public Properties Properties { get; set; } = new();
            [JsonPropertyName("required")]
            public List<string> Required { get; set; } = new List<string>();
        }

        public class Properties
        {
            [JsonPropertyName("location")]
            public Location Location { get; set; } = new();
        }

        public class Location
        {
            [JsonPropertyName("type")]  
            public string Type { get; set; } = string.Empty;
            [JsonPropertyName("description")]
            public string Description { get; set; } = string.Empty;
        }

    }
}
