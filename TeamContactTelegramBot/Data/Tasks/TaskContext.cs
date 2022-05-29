using Microsoft.EntityFrameworkCore;

namespace TeamContactTelegramBot.Data.Tasks
{
    public class TaskContext : DbContext
    {
        public virtual DbSet<Task> DbTask { get; set; }

        public TaskContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-N0A0TTI\SQLEXPRESS;Initial Catalog=TeamContact_001;Integrated Security=True");
        }
    }
}
