namespace IA.TesteAgente.Model
{
    public class DetalhesRag
    {
        public int id_fragmento { get; set; }
        public Metadados metadados { get; set; }
        public string[] perguntas_hipoteticas { get; set; }
        public string conteudo_para_vetorizacao { get; set; }
    }

    public class Metadados
    {
        public string capitulo_secao { get; set; }
        public string titulo_contextual { get; set; }
        public string[] palavras_chave { get; set; }
    }
}
