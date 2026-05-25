using IA.TesteAgente.Data;
using IA.TesteAgente.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Service.IA.Agentes.Agente;
using Service.IA.Enum;
using Service.IA.Provedor.Base;
using Service.IA.Util;

namespace IA.TesteAgente.Servico
{
    public class MonteAgente(IDbContextFactory<dbContext> DbFactory, IServiceProvider _serviceProvider)
    {
        public Agente Agente { get; set; } = new();

        public async Task SetAgente(string idAgente)
        {
            using var context = await DbFactory.CreateDbContextAsync();

            var AgenteModel = await context.AgenteIA.FirstAsync(a => a.id == idAgente);
            var provedorModel = await context.Provedor.Where(a => a.id == AgenteModel.idProvedor).FirstAsync();
            var ModeloModel = await context.ModeloLLM.Where(a => a.id == AgenteModel.idModeloLLM).FirstOrDefaultAsync();

            var provedor = _serviceProvider.GetRequiredKeyedService<IProvedorBase>(provedorModel.ServicoProvedor);
            provedor.SetProvedor(provedorModel.Url, new Tuple<string, string>(provedorModel.TagKey, provedorModel.ApiKey), provedorModel.TimeoutMinutes);
            var ChatClient = provedor.SetMedolo(ModeloModel.Modelo);

            Agente = new Agente();

            if (AgenteModel.Memoria) Agente.SetHistorico();

            await SetGuardRail(AgenteModel);

            await Agente.SetAgentAsync(ChatClient, AgenteModel.Nome, AgenteModel.Descricao, ModeloModel.Modelo, AgenteModel.Instrucoes);
        }

        public async Task SetGuardRail(AgenteIA AgenteModel)
        {
            if (!string.IsNullOrEmpty(AgenteModel.GuardRailEntrada))
                await SetFerramenta(AgenteModel.GuardRailEntrada);

            if (!string.IsNullOrEmpty(AgenteModel.idAgenteGuardRailEntrada))
                await SetSubAgente(AgenteModel.idAgenteGuardRailEntrada);

            if (!string.IsNullOrEmpty(AgenteModel.GuardRailSaida))
                await SetFerramenta(AgenteModel.GuardRailSaida);

            if (!string.IsNullOrEmpty(AgenteModel.idAgenteGuardRailSaida))
                await SetSubAgente(AgenteModel.idAgenteGuardRailSaida);
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

        public async Task SetSubAgente(string idAgente)
        {
            var MonteAgenteSubAgente = new MonteAgente(DbFactory, _serviceProvider);
            await MonteAgenteSubAgente.SetAgente(idAgente);

            if (MonteAgenteSubAgente?.Agente?.Agent == null) return;
            Agente.SetFuncao(MonteAgenteSubAgente.Agente.Agent);
        }
    }
}
