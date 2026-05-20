using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Service.IA.Agentes.GuardRail
{
    public class GuardRailOut
    {
        private Regex CpfRegex = new Regex(@"\b\d{3}\.\d{3}\.\d{3}-\d{2}\b", RegexOptions.Compiled);
        private Regex EmailRegex = new Regex(@"\b\S+@\S+\.\S+\b", RegexOptions.Compiled);
        private Regex PhoneRegex = new Regex(@"\b\d{10,11}\b", RegexOptions.Compiled);
        private Regex CnpjRegex = new Regex(@"\b\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}\b", RegexOptions.Compiled);
        private Regex CreditCardRegex = new Regex(@"\b(?:\d[ -]*?){13,16}\b", RegexOptions.Compiled);
        private Regex IpAddressRegex = new Regex(@"\b(?:\d{1,3}\.){3}\d{1,3}\b", RegexOptions.Compiled);
        private Regex MacAddressRegex = new Regex(@"\b(?:[0-9A-Fa-f]{2}[:\-]){5}[0-9A-Fa-f]{2}\b", RegexOptions.Compiled);
        private Regex StringConexaoRegex = new Regex(@"\b(?:Server|Data Source|Initial Catalog|User ID|Password|Trusted_Connection|Integrated Security)=\S+\b", RegexOptions.Compiled);
        private Regex loginSenhaRegex = new Regex(@"\b(?:login|senha|password|user|username|pass|pwd)\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        [Description("Valida se a saída do modelo contém dados sensíveis, como dados bancários, de login, pessoais," +
            " financeiros ou de redes. Retorna false se qualquer tipo de dado sensível for encontrado.")]
        public bool ValidateTudo([Description("Saída do modelo a ser validada")] string modelOutput)
        {
            if (!ValidateDadosBanco(modelOutput)) return false;
            if (!ValidateDadosLogin(modelOutput)) return false;
            if (!ValidateDadosPessoais(modelOutput)) return false;
            if (!ValidateDadosFinanceiros(modelOutput)) return false;
            if (!ValidateDadosRedes(modelOutput)) return false;
            return true;
        }

        [Description("Valida se a saída do modelo contém dados de conexão de banco de dados. Retorna false se for encontrado.")]
        public bool ValidateDadosBanco([Description("Saída do modelo a ser validada")] string modelOutput)
        {
            if (StringConexaoRegex.IsMatch(modelOutput)) return false;
            return true;
        }

        [Description("Valida se a saída do modelo contém dados de login. Retorna false se for encontrado.")]
        public bool ValidateDadosLogin([Description("Saída do modelo a ser validada")] string modelOutput)
        {
            if (loginSenhaRegex.IsMatch(modelOutput)) return false;
            return true;
        }

        [Description("Valida se a saída do modelo contém dados pessoais. Retorna false se for encontrado.")]
        public bool ValidateDadosPessoais([Description("Saída do modelo a ser validada")] string modelOutput)
        {
            if (CpfRegex.IsMatch(modelOutput)) return false;
            if (EmailRegex.IsMatch(modelOutput)) return false;
            if (PhoneRegex.IsMatch(modelOutput)) return false;
            if (CnpjRegex.IsMatch(modelOutput)) return false;
            return true;
        }

        [Description("Valida se a saída do modelo contém dados financeiros. Retorna false se for encontrado.")]
        public bool ValidateDadosFinanceiros([Description("Saída do modelo a ser validada")] string modelOutput)
        {
            if (CreditCardRegex.IsMatch(modelOutput)) return false;
            return true;
        }

        [Description("Valida se a saída do modelo contém dados de redes. Retorna false se for encontrado.")]
        public bool ValidateDadosRedes([Description("Saída do modelo a ser validada")] string modelOutput)
        {
            if (IpAddressRegex.IsMatch(modelOutput)) return false;
            if (MacAddressRegex.IsMatch(modelOutput)) return false;
            return true;
        }
    }
}
