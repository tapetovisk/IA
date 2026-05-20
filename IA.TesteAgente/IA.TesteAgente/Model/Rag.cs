using Pgvector;
using System.ComponentModel.DataAnnotations;

namespace IA.TesteAgente.Model
{
    public class Rag
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string idAgente { get; set; } = string.Empty;
        public string idModeloLLM { get; set; } = string.Empty;
        public string Produto { get; set; } = string.Empty;
        public int Paragrafo { get; set; }
        public string Texto { get; set; } = string.Empty;
        public Vector EmbeddingTexto { get; set; }
    }
}
