using Service.IA.Provedor.Base;

namespace Service.IA.Provedor.Interface
{
    public interface IProvedorAnthropic : IProvedorBase
    {
        IProvedorBase SetProvedor(string url, string apiKey);
    }
}
