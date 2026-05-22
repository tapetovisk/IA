using System.ComponentModel;

namespace Service.IA.Agentes.Tool
{
    public class Arquivo
    {
        [Description("Busca o conteúdo de um arquivo de prompt para um agente específico.")]
        public async Task<string> BuscaPromptAsync([Description("Nome do agente")] string AgentName) =>
            await BuscaArquivoAsync($"Service.IA.Agentes.Prompts.{AgentName}.md");

        [Description("Grava o conteúdo de um arquivo de prompt para um agente específico.")]
        public async Task GravarPromptAsync(
            [Description("Nome do agente")] string AgentName,
            [Description("Conteúdo do prompt")] string content) =>
            await GravarArquivoAsync($"Service.IA.Agentes.Prompts", $"{AgentName}.md", content);

        [Description("Apaga o arquivo de prompt para um agente específico.")]
        public async Task DeletarPromptAsync([Description("Nome do agente")] string AgentName) =>
            await DeletarArquivoAsync($"Service.IA.Agentes.Prompts.{AgentName}.md");

        [Description("Busca o conteúdo de um arquivo de recurso incorporado.")]
        public async Task<string> BuscaArquivoAsync([Description("Caminho do arquivo")] string filePath)
        {
            var Assembly = typeof(Arquivo).Assembly;

            using var stream = Assembly.GetManifestResourceStream(filePath);
            if (stream == null)
            {
                throw new InvalidOperationException($"Resource '{filePath}' not found.");
            }

            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        [Description("Grava o conteúdo de um arquivo em um caminho específico.")]
        public async Task GravarArquivoAsync(
            [Description("Caminho da pasta")] string folderPath,
            [Description("Nome do arquivo")] string fileName,
            [Description("Conteúdo do arquivo")] string content)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fullPath = Path.Combine(folderPath, fileName);

                File.WriteAllText(fullPath, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar arquivo: {ex.Message}");
            }
        }

        [Description("Apaga um arquivo em um caminho específico.")]
        public async Task DeletarArquivoAsync([Description("Caminho do arquivo")] string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                else
                {
                    Console.WriteLine("Arquivo não encontrado.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao apagar arquivo: {ex.Message}");
            }
        }
    }
}
