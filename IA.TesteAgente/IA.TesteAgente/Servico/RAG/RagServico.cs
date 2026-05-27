using IA.TesteAgente.Data;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace IA.TesteAgente.Servico.RAG
{
    public class RagServico(IDbContextFactory<dbContext> _dbContextFactory, IServiceProvider _serviceProvider, string Produto) : RAGBase(_dbContextFactory, _serviceProvider)
    {
        public override async Task<string[]> BuscaDados(string perguntaUsuario)
        {
            Vector vetorPergunta = await GerarEmbeddingDaPerguntaAsync(perguntaUsuario);

            using var _dbContext = await _dbContextFactory.CreateDbContextAsync();
            var trechosManuais = _dbContext.Rags
                .Where(a => a.Produto == Produto)
                .OrderBy(m => m.EmbeddingTexto.CosineDistance(vetorPergunta))
                .Take(3)
                .Select(m => m.Texto).ToArray();

            return trechosManuais;
        }
    }
}
