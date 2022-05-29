using TeamContactTelegramBot.Repositories.Tasks.Interfaces;
using TeamContactTelegramBot.Repositories.Users;
using TeamContactTelegramBot.Repositories.Users.Interfaces;

namespace TeamContactTelegramBot.Repositories
{
    public class CommonUOW : ICommonUOW
    {
        private IUsersUOW _usersUOW;
        private ITaskUOW _taskUOW;

        public IUsersUOW UsersRepository => _usersUOW ?? new UsersUOW();
        public ITaskUOW TaskRepository => _taskUOW ?? new TaskUOW();
    }
}
