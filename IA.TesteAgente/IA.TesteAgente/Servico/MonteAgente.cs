using IA.TesteAgente.Data;
using IA.TesteAgente.Model;
using IA.TesteAgente.Servico.RAG;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.AI;
using OpenAI.Embeddings;
using Service.IA.Agentes.Agente;
using Service.IA.Enum;
using Service.IA.Provedor.Base;
using Service.IA.Util;

namespace IA.TesteAgente.Servico
{
    public class MonteAgente(IDbContextFactory<dbContext> DbFactory, IServiceProvider _serviceProvider)
    {
        public Agente Agente { get; set; } = new();
        public EmbeddingClient? embeddingClient { get; set; } = null;

        public async Task SetAgente(string idAgente)
        {
            using var context = await DbFactory.CreateDbContextAsync();

            var AgenteModel = await context.AgenteIA.FirstAsync(a => a.id == idAgente);
            var provedorModel = await context.Provedor.Where(a => a.id == AgenteModel.idProvedor).FirstAsync();
            var ModeloModel = await context.ModeloLLM.Where(a => a.id == AgenteModel.idModeloLLM).FirstOrDefaultAsync();

            var provedor = _serviceProvider.GetRequiredKeyedService<IProvedorBase>(provedorModel.ServicoProvedor);
            provedor.SetProvedor(provedorModel.Url, new Tuple<string, string>(provedorModel.TagKey, provedorModel.ApiKey), provedorModel.TimeoutMinutes);
            var ChatClient = provedor.SetMedolo(ModeloModel.Modelo);

            if (ModeloModel.TipoModelo.Any(a => a == EnumTipoModelo.Embedding))
            {
                provedor.SetEmbeddingClient(ModeloModel.Modelo);
                embeddingClient = provedor.embeddingClient;
            }

            Agente = new Agente();

            if (AgenteModel.Memoria)
            {
                var Se = await Agente.GetSession();
                var Memo = new MemoriaPercistenteServico(DbFactory);
                Agente.SetHistorico(Memo);
            }

            await SetGuardRail(AgenteModel);
            await SetRelacao(idAgente);

            await Agente.SetAgentAsync(ChatClient, AgenteModel.Nome, AgenteModel.Descricao, ModeloModel.Modelo, AgenteModel.Instrucoes);
        }

        public async Task SetGuardRail(AgenteIA AgenteModel)
        {
            if (!string.IsNullOrEmpty(AgenteModel.GuardRailEntrada))
                await SetFerramenta(AgenteModel.GuardRailEntrada);

            if (!string.IsNullOrEmpty(AgenteModel.idAgenteGuardRailEntrada))
                await SetAgenteGuardRail(AgenteModel.idAgenteGuardRailEntrada);

            if (!string.IsNullOrEmpty(AgenteModel.GuardRailSaida))
                await SetFerramenta(AgenteModel.GuardRailSaida);

            if (!string.IsNullOrEmpty(AgenteModel.idAgenteGuardRailSaida))
                await SetAgenteGuardRail(AgenteModel.idAgenteGuardRailSaida);
        }

        public async Task SetRelacao(string idAgente)
        {
            using var context = await DbFactory.CreateDbContextAsync();
            var LstRelacao = context.Rel_Agente_Relacao.Where(a => a.idAgenteAi == idAgente).ToList();

            foreach (var item in LstRelacao)
            {
                switch (item.Tabela)
                {
                    case "Agente":
                        await SetSubAgente(item.idRelacao);
                        break;

                    case "RAG":
                        await SetRag(item.idRelacao);
                        break;

                    case "Ferramentas":
                        await SetFerramenta(item.idRelacao);
                        break;

                    case "Perguntas Resposta":
                        await SetFerramenta(item.idRelacao);
                        break;

                    default: break;
                }
            }
        }

        public async Task SetFerramenta(string idFerramenta)
        {
            using var context = await DbFactory.CreateDbContextAsync();

            var Ferramenta = await context.Ferramentas.Where(a => a.id == idFerramenta).FirstOrDefaultAsync();
            if (Ferramenta == null) return;

            List<AITool>? Tool = new List<AITool>();

            if (Ferramenta.TipoFerramenta == EnumTipoFerramenta.Metodo || Ferramenta.TipoFerramenta == EnumTipoFerramenta.GuardRail)
            {
                var ToolLocal = new BuscaProvedor().CriarToolMetodo(Ferramenta.Classe, Ferramenta.Command);
                if (ToolLocal == null) return;

                Tool.Add(ToolLocal);
            }
            else if (Ferramenta.TipoFerramenta == EnumTipoFerramenta.MCPLocal)
            {
                var ToolLocal = await new BuscaProvedor().CriarToolMCPLocal(Ferramenta.Nome, Ferramenta.Command, Ferramenta.JsonArguments);
                if (ToolLocal == null) return;
                Tool = ToolLocal.ToList();

            }
            else if (Ferramenta.TipoFerramenta == EnumTipoFerramenta.MCPHttp)
            {
                var ToolLocal = await new BuscaProvedor().CriarToolHttp(Ferramenta.Url, Ferramenta.JsonArguments);
                if (ToolLocal == null) return;
                Tool = ToolLocal.ToList();
            }

            Agente.SetFuncao(Tool);
        }

        public async Task SetAgenteGuardRail(string idSubAgente) => await SetSubAgente(idSubAgente);

        public async Task SetSubAgente(string idSubAgente)
        {
            var MonteAgenteSubAgente = new MonteAgente(DbFactory, _serviceProvider);
            await MonteAgenteSubAgente.SetAgente(idSubAgente);

            if (MonteAgenteSubAgente?.Agente?.Agent == null) return;
            Agente.SetFuncao(MonteAgenteSubAgente.Agente.Agent);
        }

        public async Task SetRag(string Produto)
        {
            var RagServico = new RagServico(DbFactory, _serviceProvider, Produto);
            Agente.SetContextProvider(RagServico);
        }

        public async Task SetPerguntasResposta(string Produto)
        {
            var RagServico = new PerguntasRespostaRagSevice(DbFactory, _serviceProvider, Produto);
            Agente.SetContextProvider(RagServico);
        }

    }
}
