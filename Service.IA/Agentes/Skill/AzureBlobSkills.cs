using Azure.Storage.Blobs;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

namespace Service.IA.Agentes.Skill
{
    public sealed class AzureBlobSkills : AIContextProvider
    {
#pragma warning disable MAAI001

        private readonly BlobContainerClient _container;
        private readonly string _skillsPrefix;        
        private List<AgentInlineSkill>? _skills;

        public AzureBlobSkills( BlobContainerClient container, string skillsPrefix = "")
        {
            _container = container;
            _skillsPrefix = skillsPrefix;
        }

        private async Task<List<AgentInlineSkill>> LoadSkillsAsync(CancellationToken ct)
        {
            if (_skills is not null) return _skills;

            var skills = new List<AgentInlineSkill>();

            // Lista todos os blobs com o prefixo configurado
            await foreach (var blob in _container.GetBlobsAsync(prefix: _skillsPrefix, cancellationToken: ct))
            {
                if (!blob.Name.EndsWith("SKILL.md", StringComparison.OrdinalIgnoreCase))
                    continue;

                var blobClient = _container.GetBlobClient(blob.Name);
                var response = await blobClient.DownloadContentAsync(ct);
                var content = response.Value.Content.ToString();

                try
                {
                    var (frontmatter, instructions) = SkillMarkdownParser.Parse(content);

                    // Cria a skill inline com os dados lidos do blob
                    var skill = new AgentInlineSkill(
                        name: frontmatter.Name,
                        description: frontmatter.Description,
                        instructions: instructions);

                    skills.Add(skill);
                } catch (Exception ex)
                {
                    // Loga e ignora skills malformadas (mesmo comportamento do FileAgentSkillsProvider)
                    Console.Error.WriteLine($"[BlobSkillsProvider] Ignorando '{blob.Name}': {ex.Message}");
                }
            }

            _skills = skills;
            return _skills;
        }

        public async Task<IList<AIContent>> ProvideContextAsync( IList<ChatMessage> messages, CancellationToken cancellationToken = default)
        {
            var skills = await LoadSkillsAsync(cancellationToken);

            // Delega para um AgentSkillsProvider in-memory construído com as skills carregadas
            var provider = new AgentSkillsProviderBuilder()
                .UseSkills(skills)
                .Build();

            return await provider.ProvideContextAsync(messages, cancellationToken);
        }

#pragma warning restore MAAI001
    }
}
