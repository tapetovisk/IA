using System.ComponentModel.DataAnnotations;
using Pgvector;

namespace IA.TesteAgente.Model
{
    public class PerguntasResposta
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string idAgenteEmbedding { get; set; } = string.Empty;
        public string idModeloLLMEmbedding { get; set; } = string.Empty;
        public string Produto { get; set; } = string.Empty;
        public string Pergunta { get; set; } = string.Empty;
        public string Resposta { get; set; } = string.Empty;
        public Vector EmbeddingPergunta { get; set; }
        public Vector EmbeddingRestosta { get; set; }
    }
}
