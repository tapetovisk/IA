using Microsoft.Extensions.AI;
using OpenAI;
using Service.IA.Model;
using Service.IA.Model.MicrosoftFoundry;
using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;
using System.ClientModel;
using System.ComponentModel;

namespace Service.IA.Provedor
{
    public class ProvedorMicrosoftFoundry : ProvedorBase, IProvedorMicrosoftFoundry
    {
        public override string Descricao { get; set; } = "Microsoft Foundry";

        [Description("Provedor Microsoft Foundry")]
        public IProvedorBase SetProvedor(
            [Description("URL do serviço Microsoft Foundry.")] string url,
            [Description("Chave de API do Microsoft Foundry.")] string apiKey)
        => base.SetProvedor(url, new Tuple<string, string>(TagKey, apiKey), 10);

        internal override OpenAIClient SetProvedor()
        {
            var options = new OpenAIClientOptions
            {
                Endpoint = new Uri(this.url)
            };

            openAIClient = new OpenAIClient(new ApiKeyCredential("local-key"), options);

            return openAIClient;
        }

        [Description("Retorna um IChatClient para o modelo especificado a partir do openAIClient configurado.")]
        public override IChatClient SetMedolo(
            [Description("Identificador do modelo de linguagem (ex.: \"gpt-4o\", \"llama3\").")] string model)
        {
            var options = new OpenAIClientOptions
            {
                Endpoint = new Uri(this.url)
            };

            openAIClient = new OpenAIClient(new ApiKeyCredential("local-key"), options);

            return openAIClient.GetChatClient(model).AsIChatClient();
        }

        [Description("Retorna a lista de todos os modelos disponíveis no servidor Ollama via GET /api/models.")]
        public async Task<ModelosMicrosoftFoundry> GetListaModelos()
        {
            var response = await _httpClient.GetAsync("/openai/models");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var s = content.Replace("[\"", "").Replace("\"]", "").Split("\",\"");

                return new ModelosMicrosoftFoundry() { modelo = s };
            }
            return new ModelosMicrosoftFoundry();
        }

        public async override Task<List<Modelos>> ModeloPadrao()
        {
            var detalhes = await GetListaModelos();

            var modelos = new List<Modelos>();

            foreach (var item in detalhes.modelo)
            {
                modelos.Add(new Modelos()
                {
                    Modelo = item
                });
            }
            return modelos;
        }

    }
}
