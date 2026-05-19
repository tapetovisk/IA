using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;
using System.ComponentModel;

namespace Service.IA.Provedor
{
    public class ProvedorAnthopic : ProvedorBase, IProvedorAnthopic
    {
        [Description("Provedor Azure OpenAI")]
        public IProvedorBase SetProvedor(
           [Description("URL do serviço Azure OpenAI.")] string url,
           [Description("Chave de API do Azure OpenAI.")] string apiKey)
       => base.SetProvedor(url, new Tuple<string, string>(TagKey, apiKey), 10);
    }
}
