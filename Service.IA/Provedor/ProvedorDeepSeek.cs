using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;

namespace Service.IA.Provedor
{
    public class ProvedorDeepSeek : ProvedorBase, IProvedorDeepSeek
    {
        public override string Descricao { get; set; } = "DeepSeek";
        public override string UrlPadrao { get; set; } = "https://api.deepseek.com/v1";
        public override string TagKey { get; set; } = "Authorization";
    }
}
