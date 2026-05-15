using LLama;
using LLama.Abstractions;
using LLama.Common;

namespace Service.IA.ServicoProvedor
{
    public class ArquivoLLM
    {
        public void SetModelo(string pathModelo)
        {
            var parameters = new ModelParams(pathModelo)
            {
                ContextSize = 4096,
                GpuLayerCount = 0, // 0 = CPU, >0 = offload camadas para GPU
            };

            using var weights = LLamaWeights.LoadFromFile(parameters);
            using var context = weights.CreateContext(parameters);
            var executor = new InteractiveExecutor(context);

            var chatClient = executor.AsChatClient();
        }
    }
}
