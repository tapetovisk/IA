using System.ComponentModel;

namespace Service.IA.Agentes.Tool
{
    public class GuardRailIn
    {
        [Description("Valida se o prompt de entrada é seguro para ser processado pelo modelo, " +
            " verificando contra várias categorias de ameaças, como tentativas de contornar as proteções do modelo," +
            " injeção de SQL, injeção de código, injeção de comandos e injeção de binários." +
            " Retorna false se qualquer ameaça for detectada.")]
        public bool ValidaTudo([Description("Prompt de entrada a ser validado")] string prompt)
        {
            if (!ValidaTextoBloqueado(prompt)) return false;
            if (!ValidaInjecaoSQL(prompt)) return false;
            if (!ValidaInjecaoCodigo(prompt)) return false;
            if (!ValidaInjecaoComado(prompt)) return false;
            if (!ValidaInjecaoBinarios(prompt)) return false;
            return true;
        }

        [Description("Valida se o prompt de entrada contém tentativas de contornar as proteções do modelo," +
            " como instruções para ignorar as regras, revelar o prompt do sistema, ou realizar injeção de prompt." +
            " Retorna false se qualquer tentativa for detectada.")]
        public bool ValidaTextoBloqueado([Description("Prompt de entrada a ser validado")] string prompt)
            => ValidaTexto(new string[]{
            "ignore previous instructions",
            "reveal your system prompt",
            "bypass security",
            "jailbreak",
            "prompt injection",
            "ignorar instruções anteriores",
            "exibir o prompt do sistema",
            "contornar a segurança",
            "injeção de prompt",
        }, prompt);

        [Description("Valida se o prompt de entrada contém tentativas de injeção de SQL," +
            " como comandos comuns de manipulação de banco de dados." +
            " Retorna false se qualquer tentativa for detectada.")]
        public bool ValidaInjecaoSQL([Description("Prompt de entrada a ser validado")] string prompt) => ValidaTexto(new string[]{
                "select ",
                "insert ",
                "update ",
                "delete ",
                "drop ",
                "alter ",
                "create ",
                "truncate "
            }, prompt);

        [Description("Valida se o prompt de entrada contém tentativas de injeção de código," +
            " como tags HTML/JS ou comandos de sistema." +
            " Retorna false se qualquer tentativa for detectada.")]
        public bool ValidaInjecaoCodigo([Description("Prompt de entrada a ser validado")] string prompt)
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

        [Description("Valida se o prompt de entrada contém tentativas de injeção de comandos," +
            " como instruções para enviar e-mails, apagar arquivos, ou comandos comuns de terminal." +
            " Retorna false se qualquer tentativa for detectada.")]
        public bool ValidaInjecaoComado([Description("Prompt de entrada a ser validado")] string prompt)
            => ValidaTexto(new string[]{
                "enviar e-mail ",
                "mandar email ",
                "apagar arquivo ",
                "delete ",
                "drop ",
                "alter ",
                "create ",
                "truncate ",
                ".-",
                "..",
                "---",
                "..-",

            }, prompt);

        [Description("Valida se o prompt de entrada contém tentativas de injeção de binários," +
            " como arquivos executáveis ou formatos comuns de arquivos binários." +
            " Retorna false se qualquer tentativa for detectada.")]
        public bool ValidaInjecaoBinarios([Description("Prompt de entrada a ser validado")] string prompt)
        {
            if (prompt.Length > 2 && prompt[0] == 'M' && prompt[1] == 'Z')
                return false;

            if (prompt.Length > 4 && prompt[0] == 0x7F && prompt[1] == (byte)'E' && prompt[2] == (byte)'L' && prompt[3] == (byte)'F')
                return false;

            return true;
        }


        private bool ValidaTexto(string[] Textos, string prompt)
        {
            var lower = prompt.ToLowerInvariant();
            return !Textos.Any(p => lower.Contains(p));
        }
    }
}
