using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System.ComponentModel;

namespace Service.IA.Agentes.Agente.Base
{
    public class AgenteBase : IAgenteBase
    {
        internal ChatClientAgent Agent { get; set; }
        private List<AITool> Funcoes { get; set; } = new List<AITool>();
        private List<AIContextProvider> ContextProviders { get; set; } = new List<AIContextProvider>();
        private ChatHistoryProvider Historico { get; set; } = null;

        [Description("Adiciona uma função ao agente")]
        public void SetFuncao(
            [Description("A função a ser adicionada ao agente")] Delegate funcao) 
            => Funcoes.Add(AIFunctionFactory.Create(funcao));
        [Description("Adiciona uma sub-agente ao agente")]
        public void SetFuncao(
            [Description("O sub-agente a ser adicionado ao agente")] ChatClientAgent Agente) 
            => Funcoes.Add(Agente.AsAIFunction());

        [Description("Adiciona um provedor de contexto ao agente")]
        public void SetContextProvider(
            [Description("O provedor de contexto a ser adicionado ao agente")] 
        AIContextProvider contextProvider) => ContextProviders.Add(contextProvider);
        
        [Description("Define o provedor de histórico de chat do agente")]
        public void SetHistorico(
            [Description("O provedor de histórico de chat a ser definido para o agente")] ChatHistoryProvider historico) 
            => Historico = historico;
        [Description("Define o provedor de histórico de chat do agente como memória")]
        public void SetHistorico() => Historico = new InMemoryChatHistoryProvider();

        [Description("Cria um agente de chat com as configurações definidas")]
        public async Task<ChatClientAgent> SetAgentAsync(
            [Description("Client a ser utilizado pelo agente")] IChatClient client,
            [Description("Nome do Agente")] string Nome, 
            [Description("Descrição do Agente")] string Descricao,
            [Description("Modelo do Agente")] string Modelo,
            [Description("Instruções do agente")] string Instrucoes)
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

        [Description("Cria um agente de chat com as configurações definidas")]
        public async Task<ChatClientAgent> SetAgentAsync(
            [Description("Client a ser utilizado pelo agente")] IChatClient client, 
            [Description("Nome do Agente")] string Nome, 
            [Description("Descrição do Agente")] string Descricao, 
            [Description("Class de configuração do agente contendo modelo e instruções")] ChatOptions chat)
        => await SetAgentAsync(client, new ChatClientAgentOptions
        {
            Name = Nome,
            Description = Descricao,
            ChatOptions = chat
        });
        [Description("Cria um agente de chat com as configurações definidas")]
        public async Task<ChatClientAgent> SetAgentAsync(
            [Description("Client a ser utilizado pelo agente")] IChatClient client,
            [Description("Class de configuração do agente contendo nome, decrição, modelo e instruções")] ChatClientAgentOptions system)
        {
            if (Funcoes.Any()) system?.ChatOptions?.Tools = Funcoes;

            if(Historico != null) system?.ChatHistoryProvider = Historico;
            if(ContextProviders.Any()) system?.AIContextProviders = ContextProviders;

            var agent = client.AsAIAgent(system);
            return agent;
        }
    }
}
