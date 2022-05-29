using TeamContactTelegramBot.Data.Tasks;
using TeamContactTelegramBot.Data.Users;
using TeamContactTelegramBot.Repositories.Tasks;
using TeamContactTelegramBot.Repositories.Tasks.Interfaces;
using TeamContactTelegramBot.Repositories.Users.Interfaces;

namespace TeamContactTelegramBot.Repositories.Users
{
    public class TaskUOW : ITaskUOW
    {
        private TaskContext _context;

        private ITaskRepository _taskRepository;

        public TaskUOW()
        {
            _context = new TaskContext();
        }

        public ITaskRepository Task => _taskRepository ??= new TaskRepository(this, _context);

        #region IDisposable
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context?.Dispose();
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~TaskUOW()
        {
            Dispose(false);
        }
        #endregion
    }
}
