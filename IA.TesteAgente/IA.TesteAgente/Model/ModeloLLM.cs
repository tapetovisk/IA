using Service.IA.Enum;
using Service.IA.Model;
using System.ComponentModel.DataAnnotations;

namespace IA.TesteAgente.Model
{
    public class ModeloLLM : Modelos
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string IdProvedor { get; set; } = string.Empty;
        public EnumProvedor ServicoProvedor { get; set; }
    }
}
