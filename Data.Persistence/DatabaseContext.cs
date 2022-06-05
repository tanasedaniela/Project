using Microsoft.EntityFrameworkCore;

namespace Data.Persistence
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
       
        public DbSet<Data.Domain.Entities.Consult> Consults { get; set; }

        public DbSet<Data.Domain.Entities.Medicine> Medicines { get; set; }

        public DbSet<Data.Domain.Entities.Answer> Answers { get; set; }

        public DbSet<Data.Domain.Entities.Question> Questions { get; set; }

    }
}
