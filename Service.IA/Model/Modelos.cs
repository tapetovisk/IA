using Service.IA.Enum;

namespace Service.IA.Model
{
    public class Modelos
    {
        public string Descricao { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public EnumTipoModelo[] TipoModelo { get; set; } = Array.Empty<EnumTipoModelo>();
        public string Quantizacao { get; set; } = string.Empty;
    }
}
