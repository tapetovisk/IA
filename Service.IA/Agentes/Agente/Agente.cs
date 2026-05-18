using Microsoft.Agents.AI;
using Service.IA.Agentes.Agente.Base;

namespace Service.IA.Agentes.Agente
{
    public class Agente : AgenteBase, IAgenteBase
    {
        public async Task<AgentResponse<T>> RunAsync<T>(string Prompt)
        {
            var prompt = $"{Prompt}";
            var Response = await Agent.RunAsync<T>(prompt);
            return Response;
        }

    }
}
