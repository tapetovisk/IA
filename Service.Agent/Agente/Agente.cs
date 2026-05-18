using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace Service.Agent.Agente
{
    public class Agente
    {




        
        public ChatClientAgent SetAgent(IChatClient client, ChatClientAgentOptions sistem)
        {
            var agent = client.AsAIAgent(sistem);
            return agent;
        }
    }
}
