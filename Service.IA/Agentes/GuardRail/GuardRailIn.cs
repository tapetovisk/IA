using static System.Net.Mime.MediaTypeNames;

namespace Service.IA.Agentes.GuardRail
{
    public class GuardRailIn
    {
        public string[] TextoBloqueado { get; set; } =
        {
            "ignore previous instructions",
            "reveal your system prompt",
            "bypass security",
            "jailbreak",
            "prompt injection",
            "ignorar instruções anteriores",
            "exibir o prompt do sistema",
            "contornar a segurança",
            "injeção de prompt",
        };

        public bool ValidaTextoBloqueado(string prompt) => ValidaTexto(TextoBloqueado, prompt);
        
        public bool ValidaInjecaoSQL(string prompt) => ValidaTexto(new string[]{
                "select ",
                "insert ",
                "update ",
                "delete ",
                "drop ",
                "alter ",
                "create ",
                "truncate "
            }, prompt);

        public bool ValidaInjecaoCodigo(string prompt) 
            => ValidaTexto(new string[]
                {
                    "<script",
                    "</script>",
                    "<iframe",
                    "</iframe>",
                    "<object",
                    "</object>",
                    "javascript:",
                    "onerror=",
                    "onload=",
                    "eval(",
                    "exec(",
                    "system(",
                    "cmd.exe",
                    "/bin/bash",
                    "/bin/sh"
                }, prompt);

        public bool ValidaInjecaoComado(string prompt) => ValidaTexto(new string[]{
                "enviar e-mail ",
                "mandar email ",
                "apagar arquivo ",
                "delete ",
                "drop ",
                "alter ",
                "create ",
                "truncate "
            }, prompt);




        private bool ValidaTexto(string[] Textos, string prompt)
        {
            var lower = prompt.ToLowerInvariant();
            return !Textos.Any(p => lower.Contains(p));
        }
    }
}
