using System.ComponentModel.DataAnnotations;

namespace IA.TesteAgente.Model
{
    public class Messagem
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string idSessao { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;
        public string content { get; set; } = string.Empty;
        public string idAgente { get; set; } = string.Empty;
        public string idModeloLLM { get; set; } = string.Empty;
        public DateTime Data { get; set; }

    }
}
