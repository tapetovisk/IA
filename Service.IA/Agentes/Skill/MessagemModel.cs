using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace Service.IA.Agentes.Skill
{
    public class MessagemModel : AIContextProvider
    {

        private List<ChatMessage> Messagens = new List<ChatMessage>();


        public void AddMessagens(List<ChatMessage> messagens) => Messagens.AddRange(messagens);
        public void AddMessagensSystem(List<string> messagens)
        {
            foreach (var messagem in messagens) Messagens.Add(new ChatMessage(ChatRole.System, messagem));
        }
        public void AddMessagemSystem(string messagens) => Messagens.Add(new ChatMessage(ChatRole.System, messagens));
        public void AddMessagemUser(string messagens) => Messagens.Add(new ChatMessage(ChatRole.User, messagens));
        public void AddMessagem(ChatRole Role, string messagens) => Messagens.Add(new ChatMessage(Role, messagens));

        protected override ValueTask<AIContext> ProvideAIContextAsync(InvokingContext context, CancellationToken cancellationToken)
        {
            var aiContext = new AIContext
            {
                Messages = Messagens
            };

            return ValueTask.FromResult(aiContext);
        }
    }
}
