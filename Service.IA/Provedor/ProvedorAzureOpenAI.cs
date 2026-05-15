using Azure;
using Azure.AI.OpenAI;
using OpenAI;
using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;

namespace Service.IA.Provedor
{
    public class ProvedorAzureOpenAI : ProvedorBase, IProvedorAzureOpenAI
    {
        public override string TagKey { get; set; } = "api-key";

        public IProvedorBase SetProvedor(string url, string apiKey)
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
