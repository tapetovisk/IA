using Service.IA.Model;

namespace IA.TesteAgente.Model
{
    public class Consumo : ConsumoBase
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string idProvedor { get; set; } = string.Empty;
        public string idModeloLLM { get; set; } = string.Empty;
        public string idAgenteIa { get; set; } = string.Empty;
        public string Sessao { get; set; } = string.Empty;
    }
}
