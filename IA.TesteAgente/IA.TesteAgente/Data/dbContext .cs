using Microsoft.EntityFrameworkCore;

namespace IA.TesteAgente.Data
{
    public class dbContext : DbContext
    {
        public dbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(
                "Host=srv754196.hstgr.cloud;Database=vectordb;Username=postgres;Password=postgres123",
                o => o.UseVector()
            );
        }
    }
}
