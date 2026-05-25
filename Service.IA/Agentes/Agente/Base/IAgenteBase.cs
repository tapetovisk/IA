using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace Service.IA.Agentes.Agente.Base
{
    public interface IAgenteBase
    {
        void SetFuncao(Delegate funcao);
        void SetFuncao(ChatClientAgent Agente);
        void SetFuncao(List<AITool> funcoes);
        void SetContextProvider(AIContextProvider contextProvider);
        void SetHistorico(ChatHistoryProvider historico);
        void SetHistorico();
        Task<ChatClientAgent> SetAgentAsync(IChatClient client, string Nome, string Descricao, string Modelo, string Instrucoes);
        Task<ChatClientAgent> SetAgentAsync(IChatClient client, string Nome, string Descricao, ChatOptions chat);
        Task<ChatClientAgent> SetAgentAsync(IChatClient client, ChatClientAgentOptions system);
    }
}
