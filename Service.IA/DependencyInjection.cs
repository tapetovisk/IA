using Microsoft.Extensions.DependencyInjection;
using Service.IA.Enum;
using Service.IA.Provedor;
using Service.IA.Provedor.Base;

namespace Service.IA
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAgents(this IServiceCollection services)
        {
            services.AddKeyedTransient<IProvedorBase, ProvedorAnthropic>(EnumProvedor.Anthropic);
            services.AddKeyedTransient<IProvedorBase, ProvedorArquivoGGUF>(EnumProvedor.ArquivoGGUF);
            services.AddKeyedTransient<IProvedorBase, ProvedorAzureOpenAI>(EnumProvedor.AzureOpenAI);
            services.AddKeyedTransient<IProvedorBase, ProvedorDeepSeek>(EnumProvedor.DeepSeek);
            services.AddKeyedTransient<IProvedorBase, ProvedorGitHubModels>(EnumProvedor.GitHubModels);
            services.AddKeyedTransient<IProvedorBase, ProvedorGoogle>(EnumProvedor.Google);
            services.AddKeyedTransient<IProvedorBase, ProvedorGrok>(EnumProvedor.Grok);
            services.AddKeyedTransient<IProvedorBase, ProvedorGroq>(EnumProvedor.Groq);
            services.AddKeyedTransient<IProvedorBase, ProvedorHuggingFace>(EnumProvedor.HuggingFace);
            services.AddKeyedTransient<IProvedorBase, ProvedorLmStudio>(EnumProvedor.LmStudio);
            services.AddKeyedTransient<IProvedorBase, ProvedorMicrosoftFoundry>(EnumProvedor.MicrosoftFoundry);
            services.AddKeyedTransient<IProvedorBase, ProvedorMistral>(EnumProvedor.Mistral);
            services.AddKeyedTransient<IProvedorBase, ProvedorOllama>(EnumProvedor.Ollama);
            services.AddKeyedTransient<IProvedorBase, ProvedorOpenAI>(EnumProvedor.OpenAI);
            services.AddKeyedTransient<IProvedorBase, ProvedorOpenRouter>(EnumProvedor.OpenRouter);
            return services;
        }
    }
}
