using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Service.IA.Provedor.ProvedorPersonalizado
{
    public class GeminiChatClientKernel : IChatClient
    {
        private readonly Kernel _kernel;
        private readonly IChatCompletionService _chatService;
        private readonly string _model;

        public GeminiChatClientKernel(string apiKey, string model = "gemini-2.0-flash")
        {
            _model = model;
            _kernel = Kernel.CreateBuilder()
                .AddGoogleAIGeminiChatCompletion(modelId: model, apiKey: apiKey)
                .Build();

            _chatService = _kernel.GetRequiredService<IChatCompletionService>();
        }

        public async Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
        {
            var skHistory = ToSkHistory(messages.ToList());
            PromptExecutionSettings? settings = BuildSettings(options);

            var response = await _chatService.GetChatMessageContentAsync(
            skHistory,
            executionSettings: settings,
            kernel: _kernel,
            cancellationToken: cancellationToken);

            return new ChatResponse(new ChatMessage(ChatRole.Assistant, response.Content ?? string.Empty))
            {
                ModelId = _model,
                FinishReason = ChatFinishReason.Stop
            };
        }

        public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
        {
            var skHistory = ToSkHistory(messages.ToList());
            PromptExecutionSettings? settings = BuildSettings(options);

            await foreach (var chunk in _chatService.GetStreamingChatMessageContentsAsync(
            skHistory,
            executionSettings: settings,
            kernel: _kernel,
            cancellationToken: cancellationToken))
            {
                if (!string.IsNullOrEmpty(chunk.Content))
                {
                    yield return new ChatResponseUpdate(ChatRole.Assistant, chunk.Content) { ModelId = _model };
                }
            }
        }

        public object? GetService(Type serviceType, object? serviceKey = null)
        {
            if (serviceType == typeof(Kernel)) return _kernel;
            if (serviceType == typeof(IChatCompletionService)) return _chatService;
            return this;
        }

        public void Dispose()
        {
            return;
        }

        private static ChatHistory ToSkHistory(IList<ChatMessage> messages)
        {
            var history = new ChatHistory();

            foreach (var msg in messages)
            {
                if (msg.Role == ChatRole.System)
                {
                    history.AddSystemMessage(msg.Text ?? string.Empty);
                    continue;
                }

                var items = new ChatMessageContentItemCollection();

                foreach (var content in msg.Contents)
                {
                    switch (content)
                    {
                        // Texto puro
                        case Microsoft.Extensions.AI.TextContent text:
                            items.Add(new Microsoft.SemanticKernel.TextContent(text.Text));
                            break;

                            // Imagem via URL pública
                            case UriContent  img:
                                items.Add(new ImageContent(img.Uri));
                                break;
                            
                            case DataContent img:
                                items.Add(new ImageContent(
                                    img.Data,
                                    img.MediaType ?? "image/jpeg"));
                                break;
                    }
                }

                // Fallback: mensagem só texto sem Contents populado explicitamente
                if (items.Count == 0 && !string.IsNullOrEmpty(msg.Text))
                    items.Add(new Microsoft.SemanticKernel.TextContent(msg.Text));

                if (msg.Role == ChatRole.Assistant)
                    history.AddAssistantMessage(msg.Text);
                else
                    history.AddUserMessage(items);
            }

            return history;
        }

        private PromptExecutionSettings? BuildSettings(ChatOptions? options) =>
        new()
        {
            FunctionChoiceBehavior = null,
            ModelId = _model,
        };
    }
}
