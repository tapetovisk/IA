using System.ComponentModel.DataAnnotations;

namespace IA.TesteAgente.Model
{
    public class Rel_Agente_Rag
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string idAgente { get; set; } = string.Empty;
        public string idRag { get; set; } = string.Empty;
    }
}
