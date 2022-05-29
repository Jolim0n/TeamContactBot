using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamContactTelegramBot.Service.Tasks.Interfaces;
using TeamContactTelegramBot.Service.Users.Interfaces;

namespace TeamContactTelegramBot.Service.Interfaces.System
{
    public interface ICommon
    {
        IUsersCommon UsersService { get; }
        ITaskCommon TaskService { get; }
    }
}
