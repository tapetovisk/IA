using System.ComponentModel.DataAnnotations;

namespace IA.TesteAgente.Model
{
    public class Rel_Agente_SubAgente
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string idAgente { get; set; } = string.Empty;
        public string idSubAgente { get; set; } = string.Empty;
    }
}
