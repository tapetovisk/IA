using Service.IA.Model.Ollama;
using Service.IA.Provedor.Base;

namespace Service.IA.Provedor.Interface
{
    public interface IProvedorOllama : IProvedorBase
    {
        /// <summary>
        /// Configura o provedor Ollama com URL e chave de API opcionais.
        /// Se <paramref name="url"/> for nulo, usa <see cref="UrlPadrao"/>.
        /// A chave é formatada automaticamente como <c>Bearer {apiKey}</c>.
        /// </summary>
        /// <param name="url">URL base do servidor Ollama. Usa <see cref="UrlPadrao"/> se nulo.</param>
        /// <param name="apiKey">Chave de API (pode ser vazia para servidores locais sem autenticação).</param>
        /// <returns>A própria instância (<c>this</c>), permitindo encadeamento de chamadas.</returns>
        IProvedorBase SetProvedor(string url, string apiKey);

        /// <summary>
        /// Retorna a lista de todos os modelos disponíveis no servidor Ollama via <c>GET /api/models</c>.
        /// </summary>
        /// <returns>
        /// Lista de <see cref="ModelosOllama"/> com os modelos instalados,
        /// ou uma lista vazia em caso de falha na requisição.
        /// </returns>
        Task<List<ModelosOllama>> GetListaModelos();

        /// <summary>
        /// Obtém os detalhes de um modelo específico no Ollama via <c>POST /api/show</c>,
        /// incluindo metadados como parâmetros, template e licença.
        /// </summary>
        /// <param name="model">Nome do modelo cujos detalhes serão consultados (ex.: "llama3").</param>
        /// <returns>
        /// Um <see cref="ModeloDetalhe"/> com as informações do modelo,
        /// ou uma instância vazia em caso de falha.
        /// </returns>
        Task<ModeloDetalhe> GetDetalhesModelo(string model);

        /// <summary>
        /// Faz o download (pull) de um modelo do repositório Ollama para o servidor local via <c>POST /api/pull</c>.
        /// A operação pode ser demorada dependendo do tamanho do modelo.
        /// </summary>
        /// <param name="model">Nome do modelo a ser baixado (ex.: "llama3", "phi3").</param>
        /// <returns>
        /// Conteúdo da resposta em caso de sucesso, ou uma string de erro com o código HTTP
        /// ou a mensagem da exceção em caso de falha.
        /// </returns>
        Task<string> PegarModelo(string model);

        /// <summary>
        /// Remove um modelo instalado do servidor Ollama via <c>POST /api/delete</c>.
        /// </summary>
        /// <param name="model">Nome do modelo a ser excluído (ex.: "llama3").</param>
        /// <returns>
        /// Conteúdo da resposta em caso de sucesso, ou uma string de erro com o código HTTP
        /// ou a mensagem da exceção em caso de falha.
        /// </returns>
        Task<string> EscluirModelo(string model);
    }
}
