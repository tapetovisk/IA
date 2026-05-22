using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;

namespace Service.IA.Provedor
{
    public class ProvedorGroq : ProvedorBase, IProvedorGroq
    {
        public override string Descricao { get; set; } = "Groq";
        public override string UrlPadrao { get; set; } = "https://api.groq.com/openai/v1";
        public override string TagKey { get; set; } = "Authorization";
    }
}
