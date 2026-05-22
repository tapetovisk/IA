using IA.TesteAgente.Data;
using IA.TesteAgente.Model;
using Service.IA.Agentes.Agente;

namespace IA.TesteAgente.Service
{
    public class GeraAgente(dbContext BD)
    {
        public Agente Agente { get; set; } = null;


        public void setAgente(AgenteIA ModelAgente)
        {
            Agente = new Agente();





        }





    }
}
