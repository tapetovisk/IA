using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace Service.IA.Agentes.Agente.Base
{
    public class AgenteBase
    {
        private List<AITool> Funcoes { get; set; } = new List<AITool>();
        private List<AIContextProvider> ContextProviders { get; set; } = new List<AIContextProvider>();
        private ChatHistoryProvider Historico { get; set; } = null;


        public void SetFuncao(Delegate funcao) => Funcoes.Add(AIFunctionFactory.Create(funcao));
        public void SetFuncao(ChatClientAgent Agente) => Funcoes.Add(Agente.AsAIFunction());

        public void SetContextProvider(AIContextProvider contextProvider) => ContextProviders.Add(contextProvider);
        
        public void SetHistorico(ChatHistoryProvider historico) => Historico = historico;
        public void SetHistorico() => Historico = new InMemoryChatHistoryProvider();


        public async Task<ChatClientAgent> SetAgentAsync(IChatClient client, string Nome, string Descricao, string Modelo, string Instrucoes)
        => await SetAgentAsync(client, new ChatClientAgentOptions()
        {
            Name = Nome,
            Description = Descricao,
            ChatOptions = new ChatOptions()
            {
                ModelId = Modelo,
                Instructions = Instrucoes
            }
        });
        public async Task<ChatClientAgent> SetAgentAsync(IChatClient client, string Nome, string Descricao, ChatOptions chat)
        => await SetAgentAsync(client, new ChatClientAgentOptions
        {
            Name = Nome,
            Description = Descricao,
            ChatOptions = chat
        });
        public async Task<ChatClientAgent> SetAgentAsync(IChatClient client, ChatClientAgentOptions system)
        {
            if (Funcoes.Any()) system?.ChatOptions?.Tools = Funcoes;

            system.ChatHistoryProvider = Historico;
            system.AIContextProviders = ContextProviders;

            var agent = client.AsAIAgent(system);
            return agent;
        }
    }
}
