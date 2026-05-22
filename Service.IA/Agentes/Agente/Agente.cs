using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Service.IA.Agentes.Agente.Base;
using Service.IA.Agentes.Skill;

namespace Service.IA.Agentes.Agente
{
    public class Agente : AgenteBase, IAgenteBase
    {
        internal ChatClientAgent? AgentGuardRailIn { get; set; } = null;
        internal ChatClientAgent? AgentGuardRailOut { get; set; } = null;
        internal Func<string?, bool>? GuardRailIn { get; set; } = null;
        internal Func<string?, bool>? GuardRailOut { get; set; } = null;
        private AgentSession? Session { get; set; } = null;
        private static List<TextSearchProvider.TextSearchResult> Results = new();

        public async Task<AgentSession?> GetSession()
        {
            if (Agent == null) return null;
            if (Session != null) return Session;
            Session = await Agent.CreateSessionAsync();
            return Session;
        }
        public void LimpaSession() {
            Session?.SetInMemoryChatHistory(new List<ChatMessage>());
            Session = null;
        } 
        public void SetSession(AgentSession session) => Session = session;
        public void SetSession(List<ChatMessage> msg) => Session?.SetInMemoryChatHistory(msg);

        public void SetSkill(SkillModel skill)
        {
#pragma warning disable MAAI001
            var skillsProvider = new AgentSkillsProvider(skill);
            SetContextProvider(skillsProvider);
#pragma warning restore MAAI001
        }

        public void SetRag(List<TextSearchProvider.TextSearchResult> results)
        {
            Results = results;

            TextSearchProviderOptions textSearchOptions = new()
            {
                SearchTime = TextSearchProviderOptions.TextSearchBehavior.BeforeAIInvoke,
                RecentMessageMemoryLimit = 6, // janela de histórico para busca
            };

            var Rag = new TextSearchProvider(SearchAdapter, textSearchOptions);
            SetContextProvider(Rag);
        }

        public void SetMensagem(string mensagem)
        {
            var mensagemInicial = new MessagemModel();
            mensagemInicial.AddMessagemSystem(mensagem);
            SetContextProvider(mensagemInicial);
        }

        public void SetMensagem(List<string> mensagens)
        {
            var mensagemInicial = new MessagemModel();
            mensagemInicial.AddMessagensSystem(mensagens);
            SetContextProvider(mensagemInicial);
        }

        public async Task<AgentResponse<T>?> RunAsync<T>(string Prompt)
        {
            if (GuardRailIn != null) if (!GuardRailIn(Prompt)) return null;

            if (AgentGuardRailIn != null)
            {
                var responseGuardRailIn = await AgentGuardRailIn.RunAsync<bool>(Prompt);
                if (!responseGuardRailIn.Result) return null;
            }

            if (Agent == null) return null;

            var prompt = $"{Prompt}";
            var Response = await Agent.RunAsync<T>(prompt, Session);

            if (Response != null && Response.Result != null)
            {
                if (GuardRailOut != null) if (!GuardRailOut(Response.Result?.ToString())) return null;

                if (AgentGuardRailOut != null)
                {
                    var responseGuardRailOut = await AgentGuardRailOut.RunAsync<bool>(Response.Result.ToString());
                    if (!responseGuardRailOut.Result) return null;
                }
            }

            return Response;
        }

        public async Task<AgentResponse<T>?> RunAsync<T>(ChatMessage message)
        {
            if (GuardRailIn != null) if (!GuardRailIn(message.Text)) return null;

            if (AgentGuardRailIn != null)
            {
                var responseGuardRailIn = await AgentGuardRailIn.RunAsync<bool>(message);
                if (!responseGuardRailIn.Result) return null;
            }

            if (Agent == null) return null;

            var Response = await Agent.RunAsync<T>(message, Session);

            if (Response != null && Response.Result != null)
            {
                if (GuardRailOut != null) if (!GuardRailOut(Response.Result?.ToString())) return null;

                if (AgentGuardRailOut != null)
                {
                    var responseGuardRailOut = await AgentGuardRailOut.RunAsync<bool>(Response.Result.ToString());
                    if (!responseGuardRailOut.Result) return null;
                }
            }

            return Response;
        }

        public static Task<IEnumerable<TextSearchProvider.TextSearchResult>> SearchAdapter(string query, CancellationToken cancellationToken)
        {
            var results = new List<TextSearchProvider.TextSearchResult>();

            if (query.Contains("return", StringComparison.OrdinalIgnoreCase))
            {
                results.AddRange(Results);
            }

            return Task.FromResult<IEnumerable<TextSearchProvider.TextSearchResult>>(results);
        }
    }
}
