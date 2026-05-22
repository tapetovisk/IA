using System.ComponentModel.DataAnnotations;

namespace IA.TesteAgente.Model
{
    public class AgenteIA
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string idProvedor { get; set; } = string.Empty;
        public string idModeloLLM { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Instrucoes { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string GuardRailEntrada { get; set; } = string.Empty;
        public string idAgenteGuardRailEntrada { get; set; } = string.Empty;
        public string GuardRailSaida { get; set; } = string.Empty;
        public string idAgenteGuardRailSaida { get; set; } = string.Empty;
        public bool Memoria { get; set; }
        public string TipoSaida { get; set; } = typeof(string).Name;
        public string Referencia { get; set; } = string.Empty;
    }
}
