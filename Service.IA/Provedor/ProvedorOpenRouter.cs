using Service.IA.Enum;
using Service.IA.Model;
using Service.IA.Model.Mistral;
using Service.IA.Model.OpenRouter;
using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;

namespace Service.IA.Provedor
{
    public class ProvedorOpenRouter : ProvedorBase, IProvedorOpenRouter
    {
        public override string Descricao { get; set; } = "Open Router";
        public override string UrlPadrao { get; set; } = "https://openrouter.ai/api/v1";
        public override string TagKey { get; set; } = "Authorization";

        [Description("Retorna a lista de todos os modelos disponíveis no servidor Mistral via GET /api/models.")]
        public async Task<List<ModeloOpenRouter>> GetListaModelos()
        {
            var response = await _httpClient.GetAsync("v1/models");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var modelos = System.Text.Json.JsonSerializer.Deserialize<LstModelosOpenRouter>(content);
                return modelos?.data ?? new List<ModeloOpenRouter>();
            }
            return new List<ModeloOpenRouter>();
        }

        public async override Task<List<Modelos>> ModeloPadrao()
        {
            var lista = await GetListaModelos();

            var modelos = new List<Modelos>();

            foreach (var item in lista.Where(d => d.pricing.prompt == "0" && d.pricing.completion == "0"))
            {
                var modelo = new Modelos
                {
                    Descricao = item.description,
                    Modelo = item.id
                };

                var tipos = new List<EnumTipoModelo>();
                if (item.architecture.input_modalities.Any(m => m == "text")) tipos.Add(EnumTipoModelo.Texto);
                if (item.architecture.input_modalities.Any(m => m == "image")) tipos.Add(EnumTipoModelo.Imagem);
                if (item.architecture.input_modalities.Any(m => m == "audio")) tipos.Add(EnumTipoModelo.Audio);
                if (item.architecture.input_modalities.Any(m => m == "embeddings")) tipos.Add(EnumTipoModelo.Embedding);
                if (item.architecture.input_modalities.Any(m => m == "video")) tipos.Add(EnumTipoModelo.Video);
                if (item.architecture.input_modalities.Any(m => m == "file")) tipos.Add(EnumTipoModelo.Ferramenta);

                modelo.TipoModelo = tipos.ToArray();
                modelos.Add(modelo);
            }

            return modelos;

        }
    }
}
