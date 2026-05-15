using Service.IA.Provedor.Base;

namespace Service.IA.Provedor.Interface
{
    public interface IProvedorOllama
    {
        IProvedorBase SetProvedor(string url);
        IProvedorBase SetProvedor(string url, string apiKey);

    }
}
