using Service.IA.Provedor.Base;

namespace Service.IA.Provedor.Interface
{
    public interface IProvedorAzureOpenAI : IProvedorBase
    {
        IProvedorBase SetProvedor(string url, string apiKey);
    }
}
