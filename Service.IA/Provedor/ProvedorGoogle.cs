using OpenAI;
using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;

namespace Service.IA.Provedor
{
    public class ProvedorGoogle : ProvedorBase, IProvedorGoogle
    {
        public override string UrlPadrao { get; set; } = "https://generativelanguage.googleapis.com/v1beta/openai/";
        public override string TagKey { get; set; } = "Authorization";
    }
}
