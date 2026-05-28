using IA.TesteAgente.Data;
using IA.TesteAgente.Model;
using Microsoft.Agents.AI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;

namespace IA.TesteAgente.Servico
{
    public class MemoriaPercistenteServico(IDbContextFactory<dbContext> _dbContextFactory, string IdAgente, string IdSessao) : ChatHistoryProvider
    {
        protected override async ValueTask<IEnumerable<ChatMessage>> ProvideChatHistoryAsync(InvokingContext context, CancellationToken cancellationToken = default)
        {
            ChatClientAgentSession typedSession = (ChatClientAgentSession)context.Session;
            string sessionId = IdSessao;

            using var BDcontext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

            var dbMessages = await BDcontext.Messagem
                .Where(m => m.idSessao == sessionId)
                .OrderBy(m => m.Data)
                .ToListAsync(cancellationToken);

            var history = dbMessages.Select(m => new ChatMessage(new ChatRole(m.role), m.content)).ToList();

            return history;
        }

        protected override async ValueTask StoreChatHistoryAsync(InvokedContext context, CancellationToken cancellationToken = default)
        {
            ChatClientAgentSession typedSession = (ChatClientAgentSession)context.Session;

            var todasAsMensagensDaSessao = context.RequestMessages.Where(m => m.GetAgentRequestMessageSourceType() != AgentRequestMessageSourceType.ChatHistory); ;

            if (todasAsMensagensDaSessao == null || !todasAsMensagensDaSessao.Any()) return;

            string sessionId = IdSessao;

            using var BDcontext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

            var novasMensagens = todasAsMensagensDaSessao.ToList();
            var entitiesToSave = novasMensagens.Select(m => new Messagem
            {
                idSessao = sessionId,
                idAgente = IdAgente,
                role = m.Role.Value,
                content = m.Text ?? string.Empty,
                Data = DateTime.UtcNow
            }).ToList();

            await BDcontext.Messagem.AddRangeAsync(entitiesToSave, cancellationToken);
            await BDcontext.SaveChangesAsync(cancellationToken);
        }
    }
}
