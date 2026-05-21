using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;
using System.ComponentModel;

namespace Service.IA.Provedor
{
    public class ProvedorOpenAI : ProvedorBase, IProvedorOpenAI
    {
        public override string Descricao { get; set; } = "OpenAI";
        public override string UrlPadrao { get; set; } = "https://api.openai.com/v1";
        public override string TagKey { get; set; } = "Authorization";

        [Description("Configura o provedor OpenAI com a URL e a chave de API fornecidas.")]
        public IProvedorBase SetProvedor(
            [Description("URL do serviço OpenAI.")] string url,
            [Description("Chave de API do serviço OpenAI.")] string apiKey)
            => SetProvedor(url, new Tuple<string, string>(TagKey, $"Bearer {apiKey}"));
    }
}
