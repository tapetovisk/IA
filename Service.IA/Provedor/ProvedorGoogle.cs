using Microsoft.Extensions.AI;
using Service.IA.Enum;
using Service.IA.Model;
using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;
using Service.IA.Provedor.ProvedorPersonalizado;
using System.ComponentModel;

namespace Service.IA.Provedor
{
    public class ProvedorGoogle : ProvedorBase, IProvedorGoogle
    {
        public override string Descricao { get; set; } = "Google";
        public override string UrlPadrao { get; set; } = "https://generativelanguage.googleapis.com/v1beta/openai/";
        public override string TagKey { get; set; } = "X-goog-api-key";

        [Description("Retorna um IChatClient para o modelo especificado a partir do openAIClient configurado.")]
        public override IChatClient SetMedolo(
           [Description("Identificador do modelo de linguagem (ex.: \"gpt-4o\", \"llama3\").")] string model)
        {
            if (openAIClient == null) return null;
            if (string.IsNullOrEmpty(model)) return null;
        
            return new GeminiChatClient(apiKey.Item2, model);
        }

        public async override Task<List<Modelos>> ModeloPadrao() => new List<Modelos>()
        {
            new Modelos()
            {
                Descricao = "Gemini 3.1 Pro",
                Modelo = "gemini-3.1-pro",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto, EnumTipoModelo.Visao, EnumTipoModelo.Audio },
                Quantizacao = "float16"
            },
            new Modelos()
            {
                Descricao = "Gemini 3.5 Flash",
                Modelo = "gemini-3.5-flash",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto, EnumTipoModelo.Visao, EnumTipoModelo.Audio  },
                Quantizacao = "float16"
            },
            new Modelos()
            {
                Descricao = "Gemini 3.1 Flash Lite",
                Modelo = "gemini-3.1-flash-lite",
                TipoModelo = new EnumTipoModelo[] {EnumTipoModelo.Texto, EnumTipoModelo.Visao, EnumTipoModelo.Audio },
                Quantizacao = "float16"
            },
            new Modelos(){
                Descricao = "Gemini 3 Deep Think",
                Modelo = "gemini-3-deep-think",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Pensamento },
                Quantizacao = "float16"
            },
            new Modelos()
            {
                Descricao = "Nano Banana 2",
                Modelo = "nano-banana-2",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Imagem },
                Quantizacao = "float16"
            },
            new Modelos()
            {
                Descricao = "Veo 3.1 Generate 001",
                Modelo = "veo-3.1-generate-001",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Video },
                Quantizacao = "float16"
            },
            new Modelos()
            {
                Descricao = "Gemini Embedding 001",
                Modelo = "gemini-embedding-001",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Embedding },
                Quantizacao = "float16"
            },
            new Modelos()
            {
                Descricao = "Text Embedding 005",
                Modelo = "text-embedding-005",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Embedding },
                Quantizacao = "float16"
            }
        };
    }
}
