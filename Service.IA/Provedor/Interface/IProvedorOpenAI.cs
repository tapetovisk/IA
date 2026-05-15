using Service.IA.Provedor.Base;

namespace Service.IA.Provedor.Interface
{
    public interface IProvedorOpenAI : IProvedorBase
    {
        IProvedorBase SetProvedor(string url, string apiKey);
    }
}
