using OllamaSharp.Models;

namespace Service.IA.Model.Ollama
{
    public class ModeloDetalhe
    {
            public string license { get; set; }
            public string modelfile { get; set; }
            public string parameters { get; set; }
            public string template { get; set; }
            public MDetails details { get; set; }
            public Model_Info model_info { get; set; }
            public Tensor[] tensors { get; set; }
            public string[] capabilities { get; set; }
            public DateTime modified_at { get; set; }  
    }

    public class MDetails
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
        public int gemma3attentionhead_count { get; set; }
        public int gemma3attentionhead_count_kv { get; set; }
        public int gemma3attentionkey_length { get; set; }
        public float gemma3attentionlayer_norm_rms_epsilon { get; set; }
        public int gemma3attentionsliding_window { get; set; }
        public int gemma3attentionvalue_length { get; set; }
        public int gemma3block_count { get; set; }
        public int gemma3context_length { get; set; }
        public int gemma3embedding_length { get; set; }
        public int gemma3feed_forward_length { get; set; }
        public int gemma3final_logit_softcapping { get; set; }
        public int gemma3ropeglobalfreq_base { get; set; }
        public int gemma3ropelocalfreq_base { get; set; }
        public string generalarchitecture { get; set; }
        public int generalfile_type { get; set; }
        public int generalparameter_count { get; set; }
        public int generalquantization_version { get; set; }
        public bool tokenizerggmladd_bos_token { get; set; }
        public bool tokenizerggmladd_eos_token { get; set; }
        public bool tokenizerggmladd_padding_token { get; set; }
        public bool tokenizerggmladd_unknown_token { get; set; }
        public int tokenizerggmlbos_token_id { get; set; }
        public int tokenizerggmleos_token_id { get; set; }
        public object tokenizerggmlmerges { get; set; }
        public string tokenizerggmlmodel { get; set; }
        public int tokenizerggmlpadding_token_id { get; set; }
        public string tokenizerggmlpre { get; set; }
        public object tokenizerggmlscores { get; set; }
        public object tokenizerggmltoken_type { get; set; }
        public object tokenizerggmltokens { get; set; }
        public int tokenizerggmlunknown_token_id { get; set; }
    }

    public class Tensor
    {
        public string name { get; set; }
        public string type { get; set; }
        public int[] shape { get; set; }
    }

}
