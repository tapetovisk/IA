using IA.TesteAgente.Data;
using Microsoft.Agents.AI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Pgvector;
using Service.IA.Provedor.Base;

namespace IA.TesteAgente.Servico.RAG
{
    public class RAGBase(IDbContextFactory<dbContext> _dbContextFactory, IServiceProvider _serviceProvider) : AIContextProvider
    {

        protected override async ValueTask<AIContext> ProvideAIContextAsync(InvokingContext context, CancellationToken cancellationToken = default) =>
            await RagAIContext(context);

        public virtual async ValueTask<AIContext> RagAIContext(InvokingContext context)
        {
            var inputMessages = context.AIContext.Messages?.ToList() ?? new List<ChatMessage>();
            string perguntaUsuario = inputMessages.LastOrDefault(m => m.Role == ChatRole.User)?.Text ?? string.Empty;

            if (string.IsNullOrEmpty(perguntaUsuario)) return context.AIContext;

            var trechosManuais = await BuscaDados(perguntaUsuario);

            string manualContexto = string.Join("\n\n", trechosManuais);

            context.AIContext.Messages?.Append(new ChatMessage(ChatRole.System, $@"
        [CONTEXTO]
        Use as informações abaixo para responder a pergunta do usuário. Se não souber, diga que não encontrou no contexto.

        {manualContexto}"));

            return context.AIContext;
        }

        public virtual async Task<string[]> BuscaDados(string Texto) => new string[0]; 

        public async Task<string> BuscaRelacao(string NomeAgente, string Tabela)
        {
            using var _dbContext = await _dbContextFactory.CreateDbContextAsync();
            var AgenteModel = await _dbContext.AgenteIA.Where(a => a.Nome == NomeAgente).FirstAsync();
            var relacao = await _dbContext.Rel_Agente_Relacao.Where(a => a.idAgenteAi == AgenteModel.id && a.Tabela == Tabela).FirstAsync();
            return relacao.idRelacao;
        }

        public async Task<Vector> GerarEmbeddingDaPerguntaAsync(string texto)
        {
            using var _dbContext = await _dbContextFactory.CreateDbContextAsync();
            var AgenteModel = await _dbContext.AgenteIA.Where(a => a.Referencia == "Embedding").FirstAsync();
            var provedorModel = await _dbContext.Provedor.Where(a => a.id == AgenteModel.idProvedor).FirstAsync();
            var ModeloModel = await _dbContext.ModeloLLM.Where(a => a.id == AgenteModel.idModeloLLM).FirstOrDefaultAsync();

            var provedor = _serviceProvider.GetRequiredKeyedService<IProvedorBase>(provedorModel.ServicoProvedor);
            provedor.SetProvedor(provedorModel.Url, new Tuple<string, string>(provedorModel.TagKey, provedorModel.ApiKey), provedorModel.TimeoutMinutes);
            var ChatClient = provedor.SetMedolo(ModeloModel.Modelo);

            provedor.SetEmbeddingClient(ModeloModel.Modelo);
            if (provedor.embeddingClient == null) return new Vector(new float[0]);

            var queryEmbedding = await provedor.embeddingClient.GenerateEmbeddingAsync(texto);
            return new Vector(queryEmbedding?.Value.ToFloats().ToArray());
        }
    }
}
