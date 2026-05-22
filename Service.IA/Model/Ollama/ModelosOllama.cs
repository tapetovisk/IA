namespace Service.IA.Model.Ollama
{
    public class ModelosOllama
    {
        public string name { get; set; }
        public string model { get; set; }
        public int size { get; set; }
        public string digest { get; set; }
        public Details details { get; set; }
        public DateTime expires_at { get; set; }
        public int size_vram { get; set; }
        public int context_length { get; set; }
    }

    public class Details
    {
        public string parent_model { get; set; }
        public string format { get; set; }
        public string family { get; set; }
        public string[] families { get; set; }
        public string parameter_size { get; set; }
        public string quantization_level { get; set; }
    }
}
