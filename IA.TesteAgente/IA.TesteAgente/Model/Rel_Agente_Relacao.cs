using System.ComponentModel.DataAnnotations;

namespace IA.TesteAgente.Model
{
    public class Rel_Agente_Relacao
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string idAgenteAi { get; set; } = string.Empty;

        public string Tabela { get; set; } = string.Empty;

        public string idRelacao { get; set; } = string.Empty;

    }
}
