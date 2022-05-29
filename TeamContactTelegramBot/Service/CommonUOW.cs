using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamContactTelegramBot.Repositories;
using TeamContactTelegramBot.Service.Interfaces;
using TeamContactTelegramBot.Service.Interfaces.System;

namespace TeamContactTelegramBot.Service
{
    public class CommonUOW : ICommonUOW
    {
        //private readonly ICommonUOW _commonUOW;
        private IUsersUOW _usersUOW;
     
        public IUsersUOW UsersRepository => _usersUOW ?? new UsersUOW();
    }
}
