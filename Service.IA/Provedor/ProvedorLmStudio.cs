using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;

namespace Service.IA.Provedor
{
    public class ProvedorLmStudio : ProvedorBase, IProvedorLmStudio
    {
        public override string UrlPadrao { get; set; } = "http://localhost:1234/v1";
        public override string TagKey { get; set; } = "Authorization";
    }
}
