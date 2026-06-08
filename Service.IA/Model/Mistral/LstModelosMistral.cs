namespace Service.IA.Model.Mistral
{
    public class LstModelosMistral
    {
        public string _object { get; set; }
        public List<ModeloMistral> data { get; set; }
    }


    public class ModeloMistral
    {
        public string id { get; set; }
        public string _object { get; set; }
        public int created { get; set; }
        public string owned_by { get; set; }
        public Capabilities capabilities { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int max_context_length { get; set; }
        public List<string> aliases { get; set; }
        public DateTime? deprecation { get; set; }
        public string deprecation_replacement_model { get; set; }
        public float? default_model_temperature { get; set; }
        public string type { get; set; }
    }

    public class Capabilities
    {
        public bool completion_chat { get; set; }
        public bool function_calling { get; set; }
        public bool reasoning { get; set; }
        public bool completion_fim { get; set; }
        public bool fine_tuning { get; set; }
        public bool vision { get; set; }
        public bool ocr { get; set; }
        public bool classification { get; set; }
        public bool moderation { get; set; }
        public bool audio { get; set; }
        public bool audio_transcription { get; set; }
        public bool audio_transcription_realtime { get; set; }
        public bool audio_speech { get; set; }
    }

}
