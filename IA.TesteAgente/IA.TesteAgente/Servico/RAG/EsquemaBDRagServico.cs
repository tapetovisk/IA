using IA.TesteAgente.Data;
using IA.TesteAgente.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace IA.TesteAgente.Servico.RAG
{
    /// <summary>
    /// Serviço que lê o esquema (tabelas + colunas) de um banco SQL Server,
    /// monta textos descritivos no formato adequado para busca vetorial
    /// e os armazena na tabela RAG do PostgreSQL (pgvector).
    /// </summary>
    public class EsquemaBDRagServico(
        IDbContextFactory<dbContext> dbContextFactory,
        IServiceProvider serviceProvider,
        string connectionStringSqlServer,
        string produto = "EsquemaBD")
    {
        private readonly RAGBase _ragBase = new(dbContextFactory, serviceProvider);

        // ─────────────────────────────────────────────────────────────
        // Consulta SQL que traz tabela → coluna com detalhes de tipo
        // ─────────────────────────────────────────────────────────────
        private const string SqlEsquema = """
            SELECT
                t.TABLE_SCHEMA                          AS Esquema,
                t.TABLE_NAME                            AS Tabela,
                c.COLUMN_NAME                           AS Coluna,
                c.DATA_TYPE                             AS Tipo,
                c.CHARACTER_MAXIMUM_LENGTH              AS TamanhoMax,
                c.NUMERIC_PRECISION                     AS Precisao,
                c.NUMERIC_SCALE                         AS Escala,
                c.IS_NULLABLE                           AS Nullable,
                c.COLUMN_DEFAULT                        AS ValorPadrao,
                c.ORDINAL_POSITION                      AS Ordem,
                CASE WHEN pk.COLUMN_NAME IS NOT NULL
                     THEN 'SIM' ELSE 'NÃO' END          AS ChavePrimaria
            FROM INFORMATION_SCHEMA.TABLES  t
            JOIN INFORMATION_SCHEMA.COLUMNS c
                ON  c.TABLE_SCHEMA = t.TABLE_SCHEMA
                AND c.TABLE_NAME   = t.TABLE_NAME
            LEFT JOIN (
                SELECT ku.TABLE_SCHEMA, ku.TABLE_NAME, ku.COLUMN_NAME
                FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS  tc
                JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE   ku
                    ON  ku.TABLE_SCHEMA     = tc.TABLE_SCHEMA
                    AND ku.TABLE_NAME       = tc.TABLE_NAME
                    AND ku.CONSTRAINT_NAME  = tc.CONSTRAINT_NAME
                WHERE tc.CONSTRAINT_TYPE = 'PRIMARY KEY'
            ) pk
                ON  pk.TABLE_SCHEMA = c.TABLE_SCHEMA
                AND pk.TABLE_NAME   = c.TABLE_NAME
                AND pk.COLUMN_NAME  = c.COLUMN_NAME
            WHERE t.TABLE_TYPE = 'BASE TABLE'
            ORDER BY t.TABLE_SCHEMA, t.TABLE_NAME, c.ORDINAL_POSITION;
            """;

        // ─────────────────────────────────────────────────────────────
        // Método principal: lê o esquema e popula o RAG
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Lê todas as tabelas/colunas do SQL Server, agrupa por tabela,
        /// gera um texto descritivo por tabela, calcula o embedding e
        /// grava no banco vetorial. Retorna quantos registros foram inseridos.
        /// </summary>
        public async Task<int> PopularRagComEsquemaAsync(CancellationToken ct = default)
        {
            var linhas = await LerEsquemaAsync(ct);
            var grupos = AgruparPorTabela(linhas);

            using var db = await dbContextFactory.CreateDbContextAsync(ct);

            // Remove entradas antigas do mesmo produto antes de reinserir
            var antigas = db.Rags.Where(r => r.Produto == produto);
            db.Rags.RemoveRange(antigas);
            await db.SaveChangesAsync(ct);

            int total = 0;
            foreach (var (tabela, colunas) in grupos)
            {
                string textoDescritivo = MontarTextoTabela(tabela, colunas);
                var embedding = await _ragBase.GerarEmbeddingDaPerguntaAsync(textoDescritivo);

                var rag = new Rags
                {
                    Produto         = produto,
                    CapituloSecao   = tabela,
                    Texto           = textoDescritivo,
                    PalavrasChave   = ExtrairPalavrasChave(tabela, colunas),
                    EmbeddingTexto  = embedding,
                    EmbeddingPalavrasChave = embedding
                };

                db.Rags.Add(rag);
                total++;
            }

            await db.SaveChangesAsync(ct);
            return total;
        }

        // ─────────────────────────────────────────────────────────────
        // Leitura do SQL Server
        // ─────────────────────────────────────────────────────────────

        private async Task<List<ColunaInfo>> LerEsquemaAsync(CancellationToken ct)
        {
            var resultado = new List<ColunaInfo>();

            await using var conn = new SqlConnection(connectionStringSqlServer);
            await conn.OpenAsync(ct);

            await using var cmd = new SqlCommand(SqlEsquema, conn);
            await using var reader = await cmd.ExecuteReaderAsync(ct);

            while (await reader.ReadAsync(ct))
            {
                resultado.Add(new ColunaInfo(
                    Esquema:      reader.GetString(0),
                    Tabela:       reader.GetString(1),
                    Coluna:       reader.GetString(2),
                    Tipo:         reader.GetString(3),
                    TamanhoMax:   reader.IsDBNull(4)  ? null : reader.GetInt32(4),
                    Precisao:     reader.IsDBNull(5)  ? null : reader.GetByte(5),
                    Escala:       reader.IsDBNull(6)  ? null : reader.GetInt32(6),
                    Nullable:     reader.GetString(7) == "YES",
                    ValorPadrao:  reader.IsDBNull(8)  ? null : reader.GetString(8),
                    ChavePrimaria: reader.GetString(10) == "SIM"
                ));
            }

            return resultado;
        }

        // ─────────────────────────────────────────────────────────────
        // Agrupamento e formatação
        // ─────────────────────────────────────────────────────────────

        private static Dictionary<string, List<ColunaInfo>> AgruparPorTabela(List<ColunaInfo> linhas)
            => linhas
                .GroupBy(l => $"{l.Esquema}.{l.Tabela}")
                .ToDictionary(g => g.Key, g => g.ToList());

        private static string MontarTextoTabela(string nomeCompleto, List<ColunaInfo> colunas)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Tabela: {nomeCompleto}");
            sb.AppendLine($"Total de colunas: {colunas.Count}");
            sb.AppendLine();
            sb.AppendLine("Colunas:");

            foreach (var col in colunas)
            {
                string detalhe = col.Tipo.ToLower() switch
                {
                    "varchar" or "nvarchar" or "char" or "nchar"
                        => col.TamanhoMax == -1
                            ? $"{col.Tipo}(MAX)"
                            : $"{col.Tipo}({col.TamanhoMax})",
                    "decimal" or "numeric"
                        => $"{col.Tipo}({col.Precisao},{col.Escala})",
                    _ => col.Tipo
                };

                string flags = string.Join(", ", new[]
                {
                    col.ChavePrimaria ? "PK"   : null,
                    col.Nullable      ? "NULL" : "NOT NULL",
                    col.ValorPadrao is not null ? $"DEFAULT {col.ValorPadrao}" : null
                }.Where(f => f is not null));

                sb.AppendLine($"  - {col.Coluna} [{detalhe}] ({flags})");
            }

            return sb.ToString();
        }

        private static string[] ExtrairPalavrasChave(string nomeCompleto, List<ColunaInfo> colunas)
        {
            var partes = nomeCompleto.Split('.');
            var palavras = new List<string>(partes);
            palavras.AddRange(colunas.Select(c => c.Coluna));
            return palavras.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
        }

        // ─────────────────────────────────────────────────────────────
        // Modelo interno
        // ─────────────────────────────────────────────────────────────

        private record ColunaInfo(
            string  Esquema,
            string  Tabela,
            string  Coluna,
            string  Tipo,
            int?    TamanhoMax,
            byte?   Precisao,
            int?    Escala,
            bool    Nullable,
            string? ValorPadrao,
            bool    ChavePrimaria);
    }
}
