using Google.GenAI;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using Service.IA.Provedor.Base;
using System.Reflection;

namespace Service.IA.Util
{
    public class BuscaProvedor
    {
        public ProvedorBase Criar(string nomeProvedor)
        {
            var tipoNome = $"Service.IA.Provedor.Provedor{nomeProvedor}";
            var tipo = Type.GetType(tipoNome);

            if (tipo == null)
                throw new ArgumentException("Provedor não encontrado");

            return (ProvedorBase)Activator.CreateInstance(tipo)!;
        }

        public AITool? CriarToolMetodo(string caminhoClasse, string nomeMetodo)
        {
            Type? tipo = ResolverTipo(caminhoClasse);
            if (tipo == null) return null;

            MethodInfo? metodo = tipo.GetMethod(
                nomeMetodo,
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.Static);

            if (metodo == null) return null;

            object? instancia = null;
            if (!metodo.IsStatic)
            {
                instancia = Activator.CreateInstance(tipo);
                if (instancia == null) return null;
            }

            return AIFunctionFactory.Create(metodo, instancia);
        }

        public async Task<(McpClient Client, IList<AITool> Tools)> CriarToolHttp(string url, string argumentos)
        {
            var argumentosDict = new Dictionary<string, string>();

            if(!string.IsNullOrEmpty(argumentos))
            {
                argumentosDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(argumentos) ?? 
                    new Dictionary<string, string>();
            }

            var Config = new HttpClientTransport(new HttpClientTransportOptions
            {
                Endpoint = new Uri(url),
                TransportMode = HttpTransportMode.StreamableHttp,
                ConnectionTimeout = TimeSpan.FromSeconds(120),
                AdditionalHeaders = new Dictionary<string, string>(argumentosDict)
            });

            var client = await McpClient.CreateAsync(Config);
            var tools = await client.ListToolsAsync();
            return (client, tools.Cast<AITool>().ToList());
        }

        public async Task<(McpClient Client, IList<AITool> Tools)> CriarToolMCPLocal(string nome, string comando, string argumentos)
        {
            var serverConfig = new StdioClientTransportOptions
            {
                Name = nome,
                Command = comando,
                Arguments = argumentos.Split(',')
            };

            var mcpClient = await McpClient.CreateAsync(new StdioClientTransport(serverConfig));
            var tools = await mcpClient.ListToolsAsync();
            return (mcpClient, tools.Cast<AITool>().ToList());
        }

        private Type? ResolverTipo(string caminhoClasse)
        {
            Type? tipo = Type.GetType(caminhoClasse);
            if (tipo != null) return tipo;

            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == caminhoClasse);
        }

    }
}
