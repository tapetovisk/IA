using Microsoft.Extensions.AI;
using Service.IA.Provedor.Base;

namespace Service.IA.ServicoProvedor.Base
{
    public class ServicoProvedorBase : ProvedorBase, IServicoProvedorBase
    {
        public IChatClient? _iChatClient { get; set; } = null;
        public IChatClient? _iChatClientEmbedding { get; set; } = null;
        public IChatClient? _iChatClientVision { get; set; } = null;

        public IChatClient SetModelAi(string model)
        {
            _iChatClient = (IChatClient)openAIClient.GetChatClient(model);
            return _iChatClient;
        }
    }
}
