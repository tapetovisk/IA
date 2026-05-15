using OpenAI;

namespace Service.IA.Provedor.Base
{
    public class ProvedorBase : IProvedorBase
    {
        public OpenAIClient? openAIClient { get; set; } = null;
        public HttpClient _httpClient { get; set; } = new HttpClient();
        public virtual string UrlPadrao { get; set; } = "";
        public virtual string TagKey { get; set; } = "Authorization";
        internal string url { get; set; } = "";
        internal Tuple<string, string> apiKey { get; set; } = new Tuple<string, string>("", "");

        public virtual IProvedorBase SetProvedor(string url, Tuple<string, string> apiKey, int timeoutMinutes = 10)
        {
            this.url = url;
            this.apiKey = apiKey;

            if (string.IsNullOrEmpty(url)) url = UrlPadrao;
            SetProvedor();

            if (string.IsNullOrEmpty(url)) return this;

            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromMinutes(timeoutMinutes);
            _httpClient.BaseAddress = new Uri(url);

            if (string.IsNullOrEmpty(apiKey.Item1) || string.IsNullOrEmpty(apiKey.Item2))
            {
                if(TagKey == "Authorization")
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey.Item2);
                } else
                {
                    _httpClient.DefaultRequestHeaders.Add(apiKey.Item1, apiKey.Item2);
                }

                apiKey = this.apiKey;
            }

            return this;
        }

        public virtual IProvedorBase SetProvedor(string apiKey)
            => string.IsNullOrEmpty(UrlPadrao) ? 
            this : 
            SetProvedor(UrlPadrao, new Tuple<string, string>(TagKey, apiKey), 10);

        internal virtual OpenAIClient SetProvedor()
        {
            openAIClient = UrlPadrao == url ?
                new OpenAIClient(apiKey.Item2) :
                new OpenAIClient(
                    new System.ClientModel.ApiKeyCredential(apiKey.Item2),
                    new OpenAIClientOptions
                    {
                        Endpoint = new Uri(this.url)
                    }
                );
            return openAIClient;
        }
    }
}
