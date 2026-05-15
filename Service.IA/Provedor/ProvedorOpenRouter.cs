using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;

namespace Service.IA.Provedor
{
    public class ProvedorOpenRouter : ProvedorBase, IProvedorOpenRouter
    {
        public override string UrlPadrao { get; set; } = "https://openrouter.ai/api/v1";
        public override string TagKey { get; set; } = "Authorization";
    }
}
