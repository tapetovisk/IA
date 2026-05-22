using Service.IA.Enum;
using Service.IA.Model;
using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;

namespace Service.IA.Provedor
{
    public class ProvedorDeepSeek : ProvedorBase, IProvedorDeepSeek
    {
        public override string Descricao { get; set; } = "DeepSeek";
        public override string UrlPadrao { get; set; } = "https://api.deepseek.com/v1";
        public override string TagKey { get; set; } = "Authorization";

        public async override Task<List<Modelos>> ModeloPadrao() => new List<Modelos>()
        {
            new Modelos()
            {
                Descricao = "DeepSeek - 3.5 Turbo",
                Modelo = "deepseek-3.5-turbo",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                Quantizacao = "8 bits"
            },
            new Modelos()
            {
                Descricao = "DeepSeek - 4 Turbo",
                Modelo = "deepseek-4-turbo",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                Quantizacao = "8 bits"
            },
            new Modelos()
            {
                Descricao = "DeepSeek - 4 flash",
                Modelo = "deepseek-v4-flash",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                Quantizacao = "8 bits"
            }
        };

    }
}
