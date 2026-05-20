using System.ComponentModel.DataAnnotations;

namespace IA.TesteAgente.Model
{
    public class Rel_Agente_Ferramentas
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string idAgenteAi { get; set; } = string.Empty;
        public string idFerramentas { get; set; } = string.Empty;
    }
}
