using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Embeddings;
using Service.IA.Model;
using System.ClientModel;
using System.ClientModel.Primitives;
using System.ComponentModel;

namespace Service.IA.Provedor.Base
{
    public class ProvedorBase : IProvedorBase
    {
        public OpenAIClient? openAIClient { get; set; } = null;
        public EmbeddingClient? embeddingClient { get; set; } = null;
        public HttpClient _httpClient { get; set; } = new HttpClient();
        public virtual string UrlPadrao { get; set; } = "";
        public virtual string TagKey { get; set; } = "Authorization";
        public virtual int TimeoutMinutes { get; set; } = 10;
        public virtual string Descricao { get; set; } = "";

        internal string url { get; set; } = "";

        internal Tuple<string, string> apiKey { get; set; } = new Tuple<string, string>("", "");

        [Description("Configura o provedor com URL, chave de API e timeout personalizados. " +
            "Recria o _httpClient com o endpoint e os cabeçalhos corretos, " +
            "e inicializa o openAIClient via SetProvedor()")]
        public virtual IProvedorBase SetProvedor(
            [Description("URL base do provedor. Se vazio, usa UrlPadrao.")] string url,
            [Description("Tupla com o nome do cabeçalho de autenticação (Item1) e o valor da chave de API (Item2).")] Tuple<string, string> apiKey,
            [Description("Timeout do _httpClient em minutos. Padrão: 10.")] int timeoutMinutes = 10)
        {
            this.url = url;
            this.apiKey = apiKey;
            TimeoutMinutes = timeoutMinutes > 0 ? timeoutMinutes : 10;

            if (string.IsNullOrEmpty(url)) url = UrlPadrao;
            SetProvedor();

            if (string.IsNullOrEmpty(url)) return this;

            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromMinutes(TimeoutMinutes);
            _httpClient.BaseAddress = new Uri(url);

            if (!string.IsNullOrEmpty(apiKey.Item1) || !string.IsNullOrEmpty(apiKey.Item2))
            {
                if (TagKey == "Authorization" || string.IsNullOrEmpty(apiKey.Item1))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey.Item2);
                } else
                {
                    _httpClient.DefaultRequestHeaders.Add(apiKey.Item1, apiKey.Item2);
                }

                apiKey = this.apiKey;
            }

            return this;
        }

        [Description("Atalho para configurar o provedor usando apenas a chave de API," +
            " aproveitando a UrlPadrao e o TagKey já definidos no provedor concreto.")]
        public virtual IProvedorBase SetProvedor([Description("Valor da chave de API")] string apiKey)
            => string.IsNullOrEmpty(UrlPadrao) ?
            this :
            SetProvedor(UrlPadrao, new Tuple<string, string>(TagKey, apiKey), 10);

        [Description("Retorna um IChatClient para o modelo especificado a partir do openAIClient configurado.")]
        public virtual IChatClient SetMedolo(
            [Description("Identificador do modelo de linguagem (ex.: \"gpt-4o\", \"llama3\").")] string model)
        {
            if (openAIClient == null) return null;
            if (string.IsNullOrEmpty(model)) return null;

            return openAIClient.GetChatClient(model).AsIChatClient();
        }

        /// <summary>
        /// Inicializa o <see cref="openAIClient"/> com as credenciais e o endpoint configurados.
        /// Usa o construtor simples quando a URL corresponde à <see cref="UrlPadrao"/> (OpenAI oficial);
        /// caso contrário, usa um endpoint customizado (ex.: provedores compatíveis com a API OpenAI).
        /// </summary>
        /// <returns>O <see cref="OpenAIClient"/> recém-criado.</returns>
        internal virtual OpenAIClient SetProvedor()
        {
            ApiKeyCredential credenciar;

            credenciar = string.IsNullOrEmpty(apiKey.Item2) ?
                new ApiKeyCredential("local") :
                new ApiKeyCredential(apiKey.Item2);

            var options = new OpenAIClientOptions
            {
                Endpoint = new Uri(this.url)
            };

            openAIClient = this.url.IndexOf("openai") >= 0 ?
                new OpenAIClient(apiKey.Item2) :
                new OpenAIClient(credenciar, options);

            return openAIClient;
        }

        public virtual void SetEmbeddingClient(string model)
        {
            if (openAIClient == null) return;
            if (string.IsNullOrEmpty(model)) return;
            embeddingClient = openAIClient.GetEmbeddingClient(model);
        }

        public async virtual Task<List<Modelos>> ModeloPadrao() => await Task.FromResult(new List<Modelos>());
    }
}
