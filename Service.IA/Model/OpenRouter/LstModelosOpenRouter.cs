namespace Service.IA.Model.OpenRouter
{
    public class LstModelosOpenRouter
    {
        public List<ModeloOpenRouter> data { get; set; }
    }

    public class ModeloOpenRouter
    {
        public string id { get; set; }
        public string canonical_slug { get; set; }
        public string hugging_face_id { get; set; }
        public string name { get; set; }
        public int created { get; set; }
        public string description { get; set; }
        public int context_length { get; set; }
        public Architecture architecture { get; set; }
        public Pricing pricing { get; set; }
        public Top_Provider top_provider { get; set; }
        public object per_request_limits { get; set; }
        public string[] supported_parameters { get; set; }
        public Default_Parameters default_parameters { get; set; }
        public object supported_voices { get; set; }
        public string knowledge_cutoff { get; set; }
        public string expiration_date { get; set; }
        public Links links { get; set; }
    }

    public class Architecture
    {
        public string modality { get; set; }
        public string[] input_modalities { get; set; }
        public string[] output_modalities { get; set; }
        public string tokenizer { get; set; }
        public string instruct_type { get; set; }
    }
    public class Pricing
    {
        public string prompt { get; set; }
        public string completion { get; set; }
        public string input_cache_read { get; set; }
        public string input_cache_write { get; set; }
        public string web_search { get; set; }
        public string image { get; set; }
        public string audio { get; set; }
        public string internal_reasoning { get; set; }
    }

    public class Top_Provider
    {
        public int? context_length { get; set; }
        public int? max_completion_tokens { get; set; }
        public bool is_moderated { get; set; }
    }

    public class Default_Parameters
    {
        public float? temperature { get; set; }
        public float? top_p { get; set; }
        public int? top_k { get; set; }
        public object frequency_penalty { get; set; }
        public object presence_penalty { get; set; }
        public float? repetition_penalty { get; set; }
    }

    public class Links
    {
        public string details { get; set; }
    }
}
