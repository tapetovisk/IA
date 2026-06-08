using Service.IA.Enum;
using Service.IA.Model;
using Service.IA.Model.Mistral;
using Service.IA.Provedor.Base;
using Service.IA.Provedor.Interface;
using System.ComponentModel;

namespace Service.IA.Provedor
{
    public class ProvedorMistral : ProvedorBase, IProvedorMistral
    {
        public override string Descricao { get; set; } = "Mistral";
        public override string UrlPadrao { get; set; } = "https://api.mistral.ai/v1";
        public override string TagKey { get; set; } = "Authorization";


        [Description("Retorna a lista de todos os modelos disponíveis no servidor Mistral via GET /api/models.")]
        public async Task<List<ModeloMistral>> GetListaModelos()
        {
            var response = await _httpClient.GetAsync("v1/models");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var modelos = System.Text.Json.JsonSerializer.Deserialize<LstModelosMistral>(content);
                return modelos?.data ?? new List<ModeloMistral>();
            }
            return new List<ModeloMistral>();
        }

        public async override Task<List<Modelos>> ModeloPadrao()
        {
            var lista = await GetListaModelos();

            var modelos = new List<Modelos>();

            foreach (var item in lista)
            {
                var modelo = new Modelos
                {
                    Descricao = item.description,
                    Modelo = item.id
                };

                var tipos = new List<EnumTipoModelo>();
                if (item.capabilities.completion_chat) tipos.Add(EnumTipoModelo.Texto);
                if (item.capabilities.function_calling) tipos.Add(EnumTipoModelo.Ferramenta);
                if (item.capabilities.fine_tuning) tipos.Add(EnumTipoModelo.FineTuning);
                if (item.capabilities.vision) tipos.Add(EnumTipoModelo.Imagem);
                if (item.capabilities.ocr) tipos.Add(EnumTipoModelo.Ocr);
                if (item.capabilities.classification) tipos.Add(EnumTipoModelo.Classificacao);
                if (item.capabilities.audio) tipos.Add(EnumTipoModelo.Audio);
                if (item.capabilities.audio_transcription) tipos.Add(EnumTipoModelo.Audio);
                if (item.capabilities.audio_transcription_realtime) tipos.Add(EnumTipoModelo.Audio);
                if (item.capabilities.audio_speech) tipos.Add(EnumTipoModelo.Audio);

                modelo.TipoModelo = tipos.ToArray();
                modelos.Add(modelo);
            }

            return modelos;
        }
    }
}
