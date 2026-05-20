using Service.IA.Enum;
using System.ComponentModel.DataAnnotations;

namespace IA.TesteAgente.Model
{
    public class Provedor
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string Descicao { get; set; } = string.Empty;
        public EnumProvedor ServicoProvedor { get; set; }
        public string Url { get; set; } = string.Empty;
        public int TimeoutMinutes { get; set; }
        public string ApiKey { get; set; } = string.Empty;
        public string TagKey { get; set; }
    }
}
