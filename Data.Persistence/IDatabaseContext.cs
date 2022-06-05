using Data.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistence
{
    public interface IDatabaseContext
    {
        DbSet<Consult> Consults { get; set; }

        DbSet<Medicine> Medicines { get; set; }

        DbSet<Answer> Answers { get; set; }

        DbSet<Question> Questions { get; set; }

        int SaveChanges();
    }
}
