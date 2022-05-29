using TeamContactTelegramBot.Repositories.Tasks.Interfaces;
using TeamContactTelegramBot.Repositories.Users.Interfaces;

namespace TeamContactTelegramBot.Repositories
{
    public interface ICommonUOW
    {
        IUsersUOW UsersRepository { get; }
        ITaskUOW TaskRepository { get; }
    }
}
