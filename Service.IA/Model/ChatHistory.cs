namespace Service.IA.Model
{
    public class ChatHistory
    {
        public string id { get; set; }
        public string sessionId { get; set; }
        public string role { get; set; }
        public string content { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
