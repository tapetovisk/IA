using OpenAI;
using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;

namespace Service.IA.Provedor
{
    public class ProvedorOllama : ProvedorBase, IProvedorOllama
    {
        public override string UrlPadrao { get; set; } = "http://localhost:11434";
        public override string TagKey { get; set; } = "Authorization";

        public IProvedorBase SetProvedor(string url, string apiKey)
            => base.SetProvedor(url ?? UrlPadrao, new Tuple<string, string>(TagKey, $"Bearer {apiKey}"), 10);

        internal override OpenAIClient SetProvedor()
        {
            openAIClient = new OpenAIClient(
                new System.ClientModel.ApiKeyCredential(""),
                new OpenAIClientOptions
                {
                    Endpoint = new Uri(base.url.TrimEnd('/') + "/v1")
                });

            return openAIClient;
        }
    }
}
