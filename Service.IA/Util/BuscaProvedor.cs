using Service.IA.Provedor.Base;

namespace Service.IA.Util
{
    public class BuscaProvedor
    {

        public static ProvedorBase Criar(string nomeProvedor)
        {
            var tipoNome = $"Service.IA.Provedor.Provedor{nomeProvedor}";
            var tipo = Type.GetType(tipoNome);

            if (tipo == null)
                throw new ArgumentException("Provedor não encontrado");

            return (ProvedorBase)Activator.CreateInstance(tipo)!;
        }

    }
}
