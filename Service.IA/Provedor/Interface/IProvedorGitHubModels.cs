using Service.IA.Provedor.Base;

namespace Service.IA.Provedor.Interface
{
    public interface IProvedorGitHubModels
    {
        IProvedorBase SetProvedor(string apiKey);
        IProvedorBase SetProvedor(string url, string apiKey);
    }
}
