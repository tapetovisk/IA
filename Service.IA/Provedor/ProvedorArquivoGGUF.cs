using LLama;
using LLama.Abstractions;
using LLama.Common;
using Microsoft.Extensions.AI;
using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;
using System.ComponentModel;

namespace Service.IA.Provedor
{
    public class ProvedorArquivoGGUF : ProvedorBase, IProvedorArquivoGGUF
    {
        public override string Descricao { get; set; } = "Arquivo GGUF";

        [Description("Cria um cliente de chat usando um modelo GGUF a partir de um arquivo local.")]
        public override IChatClient SetMedolo(
            [Description("Caminho para o arquivo do modelo GGUF.")] string model)
        {
            var parameters = new ModelParams(model)
            {
                ContextSize = 4096,
                GpuLayerCount = 0, // 0 = CPU, >0 = offload camadas para GPU
            };

            using var weights = LLamaWeights.LoadFromFile(parameters);
            using var context = weights.CreateContext(parameters);
            var executor = new InteractiveExecutor(context);

            var chatClient = executor.AsChatClient();
            return chatClient;
        }
    }
}
