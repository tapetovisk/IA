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
        

        public bool ValidateInformações(string modelOutput, string userId)
        {
            var redacted = modelOutput;
            var hadPii = true;

            if (CpfRegex.IsMatch(redacted))  hadPii = false;
            if (EmailRegex.IsMatch(redacted)) hadPii = false;
            
            return hadPii;
        }
    }
}
