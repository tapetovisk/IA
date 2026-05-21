using Azure;
using Azure.AI.OpenAI;
using OpenAI;
using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;
using System.ComponentModel;

namespace Service.IA.Provedor
{
    public class ProvedorAzureOpenAI : ProvedorBase, IProvedorAzureOpenAI
    {
        public override string Descricao { get; set; } = "Azure OpenAI";
        public override string TagKey { get; set; } = "api-key";

        [Description("Provedor Azure OpenAI")]
        public IProvedorBase SetProvedor(
            [Description("URL do serviço Azure OpenAI.")] string url,
            [Description("Chave de API do Azure OpenAI.")] string apiKey)
        => base.SetProvedor(url, new Tuple<string, string>(TagKey, apiKey), 10);

        internal override OpenAIClient SetProvedor()
        {
            openAIClient = new AzureOpenAIClient(
                new Uri(base.url),
                new AzureKeyCredential(base.apiKey.Item2));

            return openAIClient;
        }
    }
}
