using TeamContactTelegramBot.Repositories;
using TeamContactTelegramBot.Service.Interfaces.System;
using TeamContactTelegramBot.Service.Tasks;
using TeamContactTelegramBot.Service.Tasks.Interfaces;
using TeamContactTelegramBot.Service.Users;
using TeamContactTelegramBot.Service.Users.Interfaces;

namespace TeamContactTelegramBot.Service
{
    public class Common : ICommon
    {
        private readonly ICommonUOW _commonUOW;
        private IUsersCommon _users;
        private ITaskCommon _task;

        public IUsersCommon UsersService => _users ?? (_users = new UsersCommon(_commonUOW));
        public ITaskCommon TaskService => _task ?? (_task = new TaskCommon(_commonUOW));

        public Common()
        {
            _commonUOW = new CommonUOW();
        }
    }
}
