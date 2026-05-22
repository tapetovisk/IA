using Service.IA.Enum;
using Service.IA.Model;
using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;
using static System.Net.Mime.MediaTypeNames;

namespace Service.IA.Provedor
{
    public class ProvedorGrok : ProvedorBase, IProvedorGrok
    {
        public override string Descricao { get; set; } = "Grok";
        public override string UrlPadrao { get; set; } = "https://api.x.ai/v1";
        public override string TagKey { get; set; } = "Authorization";

        public override List<Modelos> ModeloPadrao() => new List<Modelos>()
        {
            new Modelos()
            {
                Descricao = "Grok 4.3",
                Modelo = "grok-4.3",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                Quantizacao = "8-bit"
            },
            new Modelos()
            {
                Descricao = "Grok 4.20 Reasoning",
                Modelo = "grok-4.20-reasoning",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto, EnumTipoModelo.Pensamento },
                Quantizacao = "8-bit"
            },
            new Modelos()
            {
                Descricao = "Grok 4.20 Non-Reasoning",
                Modelo = "grok-4.20-non-reasoning",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto, EnumTipoModelo.Pensamento },
                Quantizacao = "8-bit"
            },
            new Modelos()
            {
                Descricao = "Grok Build 0.1",
                Modelo = "grok-build-0.1",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto, EnumTipoModelo.Codigo },
                Quantizacao = "8-bit"
            },
            new Modelos()
            {
                Descricao = "Grok Imagine Image Quality",
                Modelo = "grok-imagine-image-quality",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Imagem },
                Quantizacao = "8-bit"
            },
            new Modelos()
            {
                Descricao = "Grok Imagine Video",
                Modelo = "grok-imagine-video",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Video },
                Quantizacao = "8-bit"
            }
        };
    }
}
