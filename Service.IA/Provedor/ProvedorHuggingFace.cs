using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;

namespace Service.IA.Provedor
{
    public class ProvedorHuggingFace : ProvedorBase, IProvedorHuggingFace
    {
        public override string UrlPadrao { get; set; } = "https://api-inference.huggingface.co";
        public override string TagKey { get; set; } = "Authorization";
    }
}
