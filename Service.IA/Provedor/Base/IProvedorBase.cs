using Microsoft.Extensions.AI;
using OpenAI;
using Service.IA.Model;

namespace Service.IA.Provedor.Base
{
    /// <summary>
    /// Classe base para provedores de IA. Centraliza a configuração do cliente HTTP e do cliente OpenAI,
    /// permitindo que provedores específicos herdem e personalizem o comportamento de autenticação e endpoint.
    /// </summary>
    public interface IProvedorBase
    {
        /// <summary>
        /// Cliente HTTP configurado com o endpoint base e os cabeçalhos de autenticação do provedor.
        /// Recriado a cada chamada de <see cref="SetProvedor(string, Tuple{string,string}, int)"/>.
        /// </summary>
        HttpClient _httpClient { get; set; }
        /// <summary>
        /// Instância do cliente OpenAI utilizada para criar clientes de chat e outros recursos da API.
        /// Será nula até que <see cref="SetProvedor()"/> seja chamado.
        /// </summary>
        OpenAIClient? openAIClient { get; set; }
        /// <summary>
        /// URL padrão do provedor. Usada quando nenhuma URL é fornecida explicitamente em <see cref="SetProvedor(string, Tuple{string,string}, int)"/>.
        /// Deve ser sobrescrita por cada provedor concreto.
        /// </summary>
        string UrlPadrao { get; set; }
        /// <summary>
        /// Nome do cabeçalho HTTP utilizado para enviar a chave de API.
        /// Padrão: "Authorization" (envia como Bearer token). Altere para o nome correto do header quando o provedor exigir outro formato.
        /// </summary>
        string TagKey { get; set; }
        int TimeoutMinutes { get; set; }
        string Descricao { get; set; }

        IProvedorBase SetProvedor(string url, Tuple<string, string> apiKey, int timeoutMinutes);
        IProvedorBase SetProvedor(string apiKey);
        IChatClient SetMedolo(string model);
        List<Modelos> ModeloPadrao();
    }
}
