using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using OpenAI;
using Service.IA.Enum;
using Service.IA.Model;
using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;

namespace Service.IA.Provedor
{
    public class ProvedorAzureOpenAI : ProvedorBase, IProvedorAzureOpenAI
    {
        public override string Descricao { get; set; } = "Azure OpenAI";
        public override string TagKey { get; set; } = "api-key";

        [Description("Provedor Azure OpenAI")]
        public IProvedorBase SetProvedor(
            [Description("URL do serviço Azure OpenAI.")] string url,
            [Description("Chave de API do Azure OpenAI.")] string apiKey)
        => base.SetProvedor(url, new Tuple<string, string>(TagKey, apiKey), 10);

        internal override OpenAIClient SetProvedor()
        {
            openAIClient = new AzureOpenAIClient(
                new Uri(base.url),
                new AzureKeyCredential(base.apiKey.Item2));

            return openAIClient;
        }

        public async override Task<List<Modelos>> ModeloPadrao() => new List<Modelos>()
        {
            new Modelos()
            {
                Descricao = "Modelo de exemplo para Azure OpenAI",
                Modelo = "text-embedding-3-large",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Embedding },
                Quantizacao = "float16"
            },
            new Modelos()
            {
                Descricao = "Modelo de exemplo para Azure OpenAI",
                Modelo = "text-embedding-3-small",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Embedding },
                Quantizacao = "float16"
            }, 
            new Modelos()
            {
                Descricao = "Modelo de exemplo para Azure OpenAI",
                Modelo = "text-embedding-ada-002",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Embedding },
                Quantizacao = "float16"
            },
            new Modelos()
            {
                Descricao = "Modelo de exemplo para Azure OpenAI",
                Modelo = "o3-mini",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                Quantizacao = "float16"
            }, new Modelos()
            {
                Descricao = "Modelo de exemplo para Azure OpenAI",
                Modelo = "o1",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                Quantizacao = "float16"

            }, new Modelos()
            {
                Descricao = "Modelo de exemplo para Azure OpenAI",
                Modelo = "o4-min",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                Quantizacao = "float16"
            }, new Modelos()
            {
                Descricao = "Modelo de exemplo para Azure OpenAI",
                Modelo = "gpt-5.5",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                Quantizacao = "float16"
            }, new Modelos()
            {
                Descricao = "Modelo de exemplo para Azure OpenAI",
                Modelo = "gpt-5.1",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                Quantizacao = "float16"
            }, new Modelos()
            {
                Descricao = "Modelo de exemplo para Azure OpenAI",
                Modelo = "gpt-4o",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                Quantizacao = "float16"
            }, new Modelos()
            {
                Descricao = "Modelo de exemplo para Azure OpenAI",
                Modelo = "gpt-4o-mini",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                Quantizacao = "float16"
            }, new Modelos()
            {
                Descricao = "Modelo de exemplo para Azure OpenAI",
                Modelo = "gpt-5.1-mini",
                TipoModelo = new EnumTipoModelo[] { EnumTipoModelo.Texto },
                Quantizacao = "float16"
            }
        };
    }
}
