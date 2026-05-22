using Service.IA.Enum;
using Service.IA.Model;
using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;
using System.ComponentModel;

namespace Service.IA.Provedor
{
    public class ProvedorAnthropic : ProvedorBase, IProvedorAnthropic
    {
        public override string Descricao { get; set; } = "Anthropic";

        [Description("Provedor Azure OpenAI")]
        public IProvedorBase SetProvedor(
           [Description("URL do serviço Azure OpenAI.")] string url,
           [Description("Chave de API do Azure OpenAI.")] string apiKey)
       => base.SetProvedor(url, new Tuple<string, string>(TagKey, apiKey), 10);

        public override List<Modelos> ModeloPadrao() => new List<Modelos>()
        {
            new Modelos()
            {
                Descricao = "claude 4.7 opus",
                Modelo = "claude-4-7-opus-latest",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto, EnumTipoModelo.Visao },
                Quantizacao = "8-bit"
            },
            new Modelos()
            {
                Descricao = "Claude 4.6 sonnet",
                Modelo = "claude-4-6-sonnet-latest",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto, EnumTipoModelo.Visao },
                Quantizacao = "8-bit"
            },
            new Modelos()
            {
                Descricao = "Claude 4.5 haiku",
                Modelo = "claude-4-5-haiku-latest",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto, EnumTipoModelo.Visao },
                Quantizacao = "8-bit"
            },
            new Modelos()
            {
                Descricao = "Claude 3.5 sonnet",
                Modelo = "claude-3-5-sonnet-20241022",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto, EnumTipoModelo.Visao },
                Quantizacao = "8-bit"
            },
            new Modelos()
            {
                Descricao = "Claude 3.5 haiku",
                Modelo = "claude-3-5-haiku-20241022",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto, EnumTipoModelo.Visao },
                Quantizacao = "8-bit"
            },
            new Modelos()
            {
                Descricao = "Claude 3 opus",
                Modelo = "claude-3-opus-20240229",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto, EnumTipoModelo.Visao },
                Quantizacao = "8-bit"
            }
        };
    }
}
