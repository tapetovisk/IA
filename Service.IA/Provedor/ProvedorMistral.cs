using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;

namespace Service.IA.Provedor
{
    public class ProvedorMistral : ProvedorBase, IProvedorGoogle
    {
        public override string UrlPadrao { get; set; } = "https://api.mistral.ai/v1";
        public override string TagKey { get; set; } = "Authorization";
    }
}
