using Microsoft.Extensions.AI;
using Service.IA.Provedor.Base;

namespace Service.IA.ServicoProvedor.Base
{
    public interface IServicoProvedorBase : IProvedorBase
    {
        IChatClient? _iChatClient { get; set; }
        IChatClient? _iChatClientEmbedding { get; set; }
        IChatClient? _iChatClientVision { get; set; }
    }
}
