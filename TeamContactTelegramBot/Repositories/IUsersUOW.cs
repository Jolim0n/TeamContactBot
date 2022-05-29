
using TeamContactTelegramBot.Repositories.Users;

namespace TeamContactTelegramBot.Repositories
{
    public interface IUsersUOW : IDisposable
    {
        IUsersRepository Users { get; }
    }
}
