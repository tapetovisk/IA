using Azure.AI.Projects;
using Azure.Identity;
using OpenAI;
using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;
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
            var Foundry = new AIProjectClient(
                new Uri(base.url),
                new DefaultAzureCredential());

            openAIClient = Foundry.ProjectOpenAIClient;

            return openAIClient;
        }
    }
}
