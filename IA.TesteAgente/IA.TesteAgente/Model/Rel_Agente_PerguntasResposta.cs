using System.ComponentModel.DataAnnotations;

namespace IA.TesteAgente.Model
{
    public class Rel_Agente_PerguntasResposta
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string idAgente { get; set; } = string.Empty;
        public string idPerguntasResposta { get; set; } = string.Empty;
    }
}
