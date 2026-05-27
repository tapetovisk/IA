using IA.TesteAgente.Model;
using Microsoft.EntityFrameworkCore;

namespace IA.TesteAgente.Data
{
    public class dbContext : DbContext
    {
        public DbSet<AgenteIA> AgenteIA { get; set; }
        public DbSet<Ferramentas> Ferramentas { get; set; }
        public DbSet<Messagem> Messagem { get; set; }
        public DbSet<ModeloLLM> ModeloLLM { get; set; }
        public DbSet<PerguntasResposta> PerguntasRespostas { get; set; }
        public DbSet<Provedor> Provedor { get; set; }
        public DbSet<Rags> Rags { get; set; }
        public DbSet<Rel_Agente_Relacao> Rel_Agente_Relacao { get; set; }

        public dbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(
                "Host=srv754196.hstgr.cloud:32798;Database=vetorBD;Username=Admin;Password=Tapeto@96",
                o => o.UseVector()
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasPostgresExtension("vector");

            modelBuilder.Entity<Rags>(entity =>
            {
                entity.ToTable("rag");
                entity.Property(e => e.EmbeddingTexto)
                .HasColumnName("EmbeddingTexto")
                .HasColumnType("vector(768)");
            });

            modelBuilder.Entity<Rags>(entity =>
            {
                entity.ToTable("rag");
                entity.Property(e => e.EmbeddingPalavrasChave)
                .HasColumnName("EmbeddingPalavrasChave")
                .HasColumnType("vector(768)");
            });

            modelBuilder.Entity<PerguntasResposta>(entity =>
            {
                entity.ToTable("perguntasResposta");
                entity.Property(e => e.EmbeddingPergunta)
                .HasColumnName("EmbeddingPergunta")
                .HasColumnType("vector(768)");
            });

            modelBuilder.Entity<PerguntasResposta>(entity =>
            {
                entity.ToTable("perguntasResposta");
                entity.Property(e => e.EmbeddingRestosta)
                .HasColumnName("EmbeddingRestosta")
                .HasColumnType("vector(768)");
            });
        }
    }
}
