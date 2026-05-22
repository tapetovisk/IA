using Anthropic.Models.Beta.Messages;
using Anthropic.Models.Messages;
using Microsoft.Extensions.AI;
using OllamaSharp;
using Service.IA.Enum;
using Service.IA.Model;
using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;

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

        public async override Task<List<Modelos>> ModeloPadrao() => new List<Modelos>()
        {
            new Modelos()
                {
                    Descricao = "tts-1-hd",
                    Modelo = "tts-1-hd",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Audio },
                    Quantizacao = "8-bit"
                },
            new Modelos()
                {
                    Descricao = "tts-1",
                    Modelo = "tts-1",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Audio },
                    Quantizacao = "8-bit"
                },
            new Modelos()
                {
                    Descricao = "dall-e-3",
                    Modelo = "dall-e-3",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Imagem },
                    Quantizacao = "8-bit"
                },
            new Modelos()
                {
                    Descricao = "text-embedding-3-small",
                    Modelo = "text-embedding-3-small",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Embedding },
                    Quantizacao = "8-bit"
                },
            new Modelos()
                {
                    Descricao = "text-embedding-3-large",
                    Modelo = "text-embedding-3-large",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Embedding },
                    Quantizacao = "8-bit"
                },
            new Modelos()
                {
                    Descricao = "Modelo de linguagem gpt-5-nano da OpenAI, conhecido por sua capacidade de compreensão e geração de texto.",
                    Modelo = "gpt-5-nano",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                    Quantizacao = "8-bit"
                },
            new Modelos()
                {
                    Descricao = "Modelo de linguagem o4-mini da OpenAI, conhecido por sua capacidade de compreensão e geração de texto.",
                    Modelo = "o4-mini",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                    Quantizacao = "8-bit"
                },
            new Modelos()
                {
                    Descricao = "Modelo de linguagem o3-mini da OpenAI, conhecido por sua capacidade de compreensão e geração de texto.",
                    Modelo = "o3-mini",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                    Quantizacao = "8-bit"
                },
            new Modelos()
                {
                    Descricao = "Modelo de linguagem o3 da OpenAI, conhecido por sua capacidade de compreensão e geração de texto.",
                    Modelo = "o3",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                    Quantizacao = "8-bit"
                },
            new Modelos()
                {
                    Descricao = "Modelo de linguagem o1 da OpenAI, conhecido por sua capacidade de compreensão e geração de texto.",
                    Modelo = "o1",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                    Quantizacao = "8-bit"
                },
                new Modelos()
                {
                    Descricao = "Modelo de linguagem gpt-5 da OpenAI, conhecido por sua capacidade de compreensão e geração de texto.",
                    Modelo = "gpt-5",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                    Quantizacao = "8-bit"
                },
                new Modelos()
                {
                    Descricao = "Modelo de linguagem gpt-5 chat da OpenAI, conhecido por sua capacidade avançada de compreensão e geração de texto.",
                    Modelo = "gpt-5-chat",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                    Quantizacao = "8-bit"
                },
                new Modelos()
                {
                    Descricao = "Modelo de linguagem gpt-4o Turbo da OpenAI, uma versão otimizada do GPT-4 para desempenho e custo.",
                    Modelo = "gpt-4o",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                    Quantizacao = "8-bit"
                },
                new Modelos()
                {
                    Descricao = "Modelo de linguagem gpt-5-mini da OpenAI, conhecido por sua capacidade de compreensão e geração de texto.",
                    Modelo = "gpt-5-mini",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                    Quantizacao = "8-bit"
                },
                new Modelos()
                {
                    Descricao = "Modelo de linguagem gpt-4o-mini da OpenAI, uma versão otimizada do GPT-4 para desempenho e custo.",
                    Modelo = "gpt-4o-mini",
                    TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                    Quantizacao = "8-bit"
                }
        };
    }
}
