using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;

namespace Service.IA.Provedor
{
    public class ProvedorOpenAI : ProvedorBase, IProvedorOpenAI
    {
        public override string UrlPadrao { get; set; } = "https://api.openai.com/v1";
        public override string TagKey { get; set; } = "Authorization";

        public IProvedorBase SetProvedor(string url, string apiKey)
            => SetProvedor(url, new Tuple<string, string>(TagKey, $"Bearer {apiKey}"));
    }
}
