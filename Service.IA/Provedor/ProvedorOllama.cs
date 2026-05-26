using Microsoft.Extensions.AI;
using OllamaSharp;
using OpenAI;
using Service.IA.Enum;
using Service.IA.Model;
using Service.IA.Model.Ollama;
using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;
using Service.IA.Util;
using System.ClientModel;
using System.ComponentModel;

namespace Service.IA.Provedor
{
    /// <summary>
    /// Provedor concreto para integração com o <b>Ollama</b>, servidor local de modelos de linguagem (LLMs).
    /// Herda a infraestrutura HTTP/OpenAI de <see cref="ProvedorBase"/> e sobrescreve os membros necessários
    /// para apontar ao endpoint padrão do Ollama (<c>http://localhost:11434</c>) e usar a API nativa via <c>OllamaSharp</c>.
    /// </summary>
    public class ProvedorOllama : ProvedorBase, IProvedorOllama
    {
        public override string Descricao { get; set; } = "Ollama";
        public override string UrlPadrao { get; set; } = "http://localhost:11434";
        public override string TagKey { get; set; } = "Authorization";

        [Description("Configura o provedor Ollama com URL e chave de API opcionais. " +
            "Se url for nulo, usa UrlPadrao. A chave é formatada automaticamente como <c>Bearer {apiKey}</c>.")]
        public IProvedorBase SetProvedor(
            [Description("URL base do servidor Ollama. Usa UrlPadrao se nulo.")] string url,
            [Description("Chave de API (pode ser vazia para servidores locais sem autenticação).")] string apiKey)
            => base.SetProvedor(url ?? UrlPadrao, new Tuple<string, string>(TagKey, $"Bearer {apiKey}"), 10);

        /// <summary>
        /// Inicializa o <see cref="ProvedorBase.openAIClient"/> apontando para o endpoint <c>/v1</c> do Ollama,
        /// que expõe uma API compatível com o formato OpenAI. A credencial é vazia pois o Ollama local não exige chave.
        /// </summary>
        /// <returns>O <see cref="OpenAIClient"/> configurado para o Ollama.</returns>
        internal override OpenAIClient SetProvedor()
        {
            openAIClient = SetProvedorOllama(url.TrimEnd('/') + "/v1");
            return openAIClient;
        }

        private OpenAIClient SetProvedorOllama(string url)
        {
            ApiKeyCredential credenciar;

            credenciar = string.IsNullOrEmpty(apiKey.Item2) ?
                new ApiKeyCredential("local") :
                new ApiKeyCredential(apiKey.Item2);

            var openAIClientInter = new OpenAIClient(
                credenciar,
                new OpenAIClientOptions
                {
                    Endpoint = new Uri(base.url.TrimEnd('/') + "/v1")
                });

            return openAIClientInter;
        }

        public override void SetEmbeddingClient(string model)
        {
            var openAIClientEmb = SetProvedorOllama(url.TrimEnd('/') + "/");

            if (openAIClientEmb == null) return;
            if (string.IsNullOrEmpty(model)) return;
            embeddingClient = openAIClientEmb.GetEmbeddingClient(model);
        }

        [Description("Retorna um IChatClient para o modelo especificado a partir do openAIClient configurado.")]
        public override IChatClient SetMedolo(
            [Description("Identificador do modelo de linguagem (ex.: \"gpt-4o\", \"llama3\").")] string model)
        {
            if (openAIClient == null) return null;
            if (string.IsNullOrEmpty(model)) return null;

            return new OllamaApiClient(_httpClient, model);
        }

        [Description("Retorna a lista de todos os modelos disponíveis no servidor Ollama via GET /api/models.")]
        public async Task<List<ModelosOllama>> GetListaModelos()
        {
            var response = await _httpClient.GetAsync("api/tags");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var modelos = System.Text.Json.JsonSerializer.Deserialize<LstModelosOllama>(content);
                return modelos?.models ?? new List<ModelosOllama>();
            }
            return new List<ModelosOllama>();
        }

        [Description("Retorna os detalhes de um modelo específico no Ollama via POST /api/show com o nome do modelo.")]
        public async Task<ModeloDetalhe> GetDetalhesModelo(
            [Description("Nome do modelo para o qual os detalhes serão retornados.")] string model)
        {
            var response = await _httpClient.PostAsync("api/show", Conversor.ConvertJson(new { model }));

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var modelos = System.Text.Json.JsonSerializer.Deserialize<ModeloDetalhe>(content);
                return modelos;
            }
            return new ModeloDetalhe();
        }

        [Description("Puxa o modelo especificado do Ollama para o servidor local via POST /api/pull com o nome do modelo. " +
            "Retorna a resposta do servidor, que pode indicar sucesso ou detalhes de erro.")]
        public async Task<string> PegarModelo(
            [Description("Nome do modelo que será puxado do servidor Ollama local.")] string model)
        {
            try
            {
                var response = await _httpClient.PostAsync("api/pull", Conversor.ConvertJson(new { model }));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return content;
                }
                return $"Erro: {response.StatusCode} - {response.ReasonPhrase}";
            } catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [Description("Exclui o modelo especificado do servidor Ollama local via POST /api/delete com o nome do modelo. " +
            "Retorna a resposta do servidor, que pode indicar sucesso ou detalhes de erro.")]
        public async Task<string> EscluirModelo(
            [Description("Nome do modelo que será excluído do servidor Ollama local.")] string model)
        {
            try
            {
                var response = await _httpClient.PostAsync("api/delete", Conversor.ConvertJson(new { model }));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return content;
                }
                return $"Erro: {response.StatusCode} - {response.ReasonPhrase}";
            } catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async override Task<List<Modelos>> ModeloPadrao()
        {
            var lista = await GetListaModelos();

            var modelos = new List<Modelos>();

            foreach (var item in lista)
            {
                var detalhes = await GetDetalhesModelo(item.model);

                List<EnumTipoModelo> tipos = new List<EnumTipoModelo>();
                foreach (var capability in detalhes.capabilities)
                {
                    switch (capability)
                    {
                        case "text": tipos.Add(EnumTipoModelo.Texto); break;
                        case "completion": tipos.Add(EnumTipoModelo.Conclusao); break;
                        case "vision": tipos.Add(EnumTipoModelo.Visao); break;
                        case "audio": tipos.Add(EnumTipoModelo.Audio); break;
                        case "tools": tipos.Add(EnumTipoModelo.Ferramenta); break;
                        case "thinking": tipos.Add(EnumTipoModelo.Pensamento); break;
                        default: break;
                    }
                }

                modelos.Add(new Modelos()
                {
                    Modelo = item.model,
                    Quantizacao = item.details.quantization_level,
                    TipoModelo = tipos.ToArray()
                });
            }

            return modelos;
        }
    }
}
