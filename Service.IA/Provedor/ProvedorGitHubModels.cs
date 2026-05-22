using OllamaSharp;
using Service.IA.Model;
using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;
using System.ComponentModel;

namespace Service.IA.Provedor
{
    public class ProvedorGitHubModels : ProvedorBase, IProvedorGitHubModels
    {
        public override string Descricao { get; set; } = "GitHub Models";
        public override string TagKey { get; set; } = "api-key";
        public override string UrlPadrao { get; set; } = "https://models.inference.ai.azure.com";

        [Description("Configura o provedor de modelos do GitHub com a URL e a chave de API fornecidas.")]
        public IProvedorBase SetProvedor(
            [Description("URL do serviço de modelos do GitHub.")] string url,
            [Description("Chave de API do serviço de modelos do GitHub.")] string apiKey)
        => base.SetProvedor(url, new Tuple<string, string>(TagKey, apiKey), 10);

    }
}
