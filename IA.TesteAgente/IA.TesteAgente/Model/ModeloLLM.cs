using Service.IA.Enum;
using System.ComponentModel.DataAnnotations;

namespace IA.TesteAgente.Model
{
    public class ModeloLLM
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public EnumProvedor ServicoProvedor { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public EnumTipoModelo[] TipoModelo { get; set; } = Array.Empty<EnumTipoModelo>();
    }
}
