using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;

namespace Service.IA.Provedor
{
    public class ProvedorGrok : ProvedorBase, IProvedorGrok
    {
        public override string Descricao { get; set; } = "Grok";
        public override string UrlPadrao { get; set; } = "https://api.x.ai/v1";
        public override string TagKey { get; set; } = "Authorization";
    }
}
