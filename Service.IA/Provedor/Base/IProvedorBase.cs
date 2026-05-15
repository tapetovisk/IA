using OpenAI;

namespace Service.IA.Provedor.Base
{
    public interface IProvedorBase
    {
        HttpClient _httpClient { get; set; }
        OpenAIClient? openAIClient { get; set; }
        string UrlPadrao { get; set; }
        string TagKey { get; set; }

        IProvedorBase SetProvedor(string url, Tuple<string, string> apiKey, int timeoutMinutes);
        IProvedorBase SetProvedor(string apiKey);
    }
}
