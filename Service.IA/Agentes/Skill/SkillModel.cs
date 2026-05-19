using Microsoft.Agents.AI;
using System.ComponentModel;

#pragma warning disable MAAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates.

namespace Service.IA.Agentes.Skill
{
    public class SkillModel(string _name, string _description, string Instrucao)
        : AgentClassSkill<SkillModel>
    {
        
        public override AgentSkillFrontmatter Frontmatter { get; } = new(
        name: _name,
        description: _description);

        protected override string Instructions => Instrucao;

        

    }
}

#pragma warning restore MAAI001
