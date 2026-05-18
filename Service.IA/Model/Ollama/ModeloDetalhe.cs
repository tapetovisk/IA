using OllamaSharp.Models;

namespace Service.IA.Model.Ollama
{
    public class ModeloDetalhe
    {
        public string modelfile { get; set; }
        public string template { get; set; }
        public Detalhes details { get; set; }
        public string remote_model { get; set; }
        public string remote_host { get; set; }
        public Model_Info model_info { get; set; }
        public string[] capabilities { get; set; }
        public DateTime modified_at { get; set; }
    }

    public class Detalhes
    {
        public string parent_model { get; set; }
        public string format { get; set; }
        public string family { get; set; }
        public string[] families { get; set; }
        public string parameter_size { get; set; }
        public string quantization_level { get; set; }
    }

    public class Model_Info
    {
        public string generalarchitecture { get; set; }
        public string generalbasename { get; set; }
        public int gptosscontext_length { get; set; }
        public int gptossembedding_length { get; set; }
    }

}
