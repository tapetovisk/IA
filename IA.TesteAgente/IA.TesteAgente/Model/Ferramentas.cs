using Service.IA.Enum;
using System.ComponentModel.DataAnnotations;

namespace IA.TesteAgente.Model
{
    public class Ferramentas
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public EnumTipoFerramenta TipoFerramenta { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public string JsonArguments { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
