using IA.TesteAgente.Data;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace IA.TesteAgente.Servico.RAG
{
    public class PerguntasRespostaRagSevice(IDbContextFactory<dbContext> _dbContextFactory, IServiceProvider _serviceProvider, string Produto) : RAGBase(_dbContextFactory, _serviceProvider)
    {
        public override async Task<string[]> BuscaDados(string perguntaUsuario)
        {
            Vector vetorPergunta = await GerarEmbeddingDaPerguntaAsync(perguntaUsuario);

            using var _dbContext = await _dbContextFactory.CreateDbContextAsync();
            var Resp = _dbContext.PerguntasRespostas
                .Where(a => a.Produto == Produto)
                .OrderBy(m => m.EmbeddingPergunta.CosineDistance(vetorPergunta))
                .Take(3)
                .Select(m => m.Resposta).ToArray();

            var Resp2 = _dbContext.PerguntasRespostas
                .Where(a => a.Produto == Produto)
                .OrderBy(m => m.EmbeddingRestosta.CosineDistance(vetorPergunta))
                .Take(3)
                .Select(m => m.Pergunta).ToArray();

            Resp = Resp.Concat(Resp2).ToArray();

            return Resp;
        }
    }
}
