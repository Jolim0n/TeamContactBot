using TeamContactTelegramBot.Data.Users;
using TeamContactTelegramBot.Repositories.Users;

namespace TeamContactTelegramBot.Repositories
{
    public class UsersUOW : IUsersUOW
    {
        private UsersContext _context;

        private IUsersRepository _usersRepository;

        public UsersUOW()
        {
            _context = new UsersContext();
        }

        public IUsersRepository Users => _usersRepository ??= new UserRepository(this, _context);

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
        ~UsersUOW()
        {
            Dispose(false);
        }
        #endregion
    }
}
