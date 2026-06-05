using Service.IA.Enum;
using Service.IA.Model;
using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;

namespace Service.IA.Provedor
{
    public class ProvedorOpenRouter : ProvedorBase, IProvedorOpenRouter
    {
        public override string Descricao { get; set; } = "Open Router";
        public override string UrlPadrao { get; set; } = "https://openrouter.ai/api/v1";
        public override string TagKey { get; set; } = "Authorization";

        public async override Task<List<Modelos>> ModeloPadrao() => new List<Modelos>()
        {
            new Modelos()
                {
                    Descricao = "openai/gpt-oss-120b:free",
                    Modelo = "openai/gpt-oss-120b:free",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                    Quantizacao = "8-bit"
                },
            new Modelos()
                {
                    Descricao = "google/gemma-4-31b-it:free",
                    Modelo = "google/gemma-4-31b-it:free",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                    Quantizacao = "8-bit"
                },
            new Modelos()
                {
                    Descricao = "nvidia/llama-nemotron-embed-vl-1b-v2:free",
                    Modelo = "nvidia/llama-nemotron-embed-vl-1b-v2:free",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Embedding },
                    Quantizacao = "8-bit"
                },
            new Modelos()
                {
                    Descricao = "sourceful/riverflow-v2.5-fast:free",
                    Modelo = "sourceful/riverflow-v2.5-fast:free",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Imagem },
                    Quantizacao = "8-bit"
                },
            new Modelos()
                {
                    Descricao = "sourceful/riverflow-v2.5-pro:free",
                    Modelo = "sourceful/riverflow-v2.5-pro:free",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Imagem },
                    Quantizacao = "8-bit"
                },
            new Modelos()
                {
                    Descricao = "moonshotai/kimi-k2.6:free",
                    Modelo = "moonshotai/kimi-k2.6:free",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                    Quantizacao = "8-bit"
                },
            new Modelos()
                {
                    Descricao = "qwen/qwen3-next-80b-a3b-instruct:free",
                    Modelo = "qwen/qwen3-next-80b-a3b-instruct:free",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                    Quantizacao = "8-bit"
                },
            new Modelos()
                {
                    Descricao = "meta-llama/llama-3.3-70b-instruct:free",
                    Modelo = "meta-llama/llama-3.3-70b-instruct:free",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                    Quantizacao = "8-bit"
                },
            new Modelos()
                {
                    Descricao = "qwen/qwen3-coder:free",
                    Modelo = "qwen/qwen3-coder:free",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                    Quantizacao = "8-bit"
                },

        };
    }
}
