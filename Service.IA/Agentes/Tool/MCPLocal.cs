using Microsoft.Extensions.AI;
using System.Diagnostics;

namespace Service.IA.Agentes.Tool
{
    public class MCPLocal : AITool
    {
        private readonly string _fileName;
        private readonly string _arguments;

        public MCPLocal(string name, string fileName, string arguments)
        {
            _fileName = fileName;
            _arguments = arguments;
        }

        public async Task<string> InvokeAsync(object input)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = _fileName,
                Arguments = _arguments,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = processStartInfo };
            process.Start();

            // Escreve input no MCP
            if (input != null)
            {
                await process.StandardInput.WriteLineAsync(input.ToString());
                await process.StandardInput.FlushAsync();
            }

            // Lê saída do MCP
            string output = await process.StandardOutput.ReadToEndAsync();
            process.WaitForExit();

            return output;
        }

    }
}
