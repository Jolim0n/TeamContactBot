using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamContactTelegramBot.Repositories;

namespace TeamContactTelegramBot.Service.Interfaces.System
{
    public interface ICommonUOW
    {
        IUsersUOW UsersRepository { get; }
    }
}
