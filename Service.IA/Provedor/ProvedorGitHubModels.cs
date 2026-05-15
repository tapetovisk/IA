using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;

namespace Service.IA.Provedor
{
    public class ProvedorGitHubModels : ProvedorBase, IProvedorGitHubModels
    {
        public override string TagKey { get; set; } = "api-key";
        public override string UrlPadrao { get; set; } = "https://models.inference.ai.azure.com";

        public IProvedorBase SetProvedor(string url, string apiKey)
        => base.SetProvedor(url, new Tuple<string, string>(TagKey, apiKey), 10);
    }
}
