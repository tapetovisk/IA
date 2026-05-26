using Pgvector;
using System.ComponentModel.DataAnnotations;

namespace IA.TesteAgente.Model
{
    public class Rags
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string idAgente { get; set; } = string.Empty;
        public string idModeloLLM { get; set; } = string.Empty;
        public string Produto { get; set; } = string.Empty;
        public int Paragrafo { get; set; }
        public string CapituloSecao { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
        public string[] PalavrasChave { get; set; } = Array.Empty<string>();

        public Vector EmbeddingTexto { get; set; }
        public Vector EmbeddingPalavrasChave { get; set; }
    }
}
