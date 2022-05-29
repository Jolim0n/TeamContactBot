using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamContactTelegramBot.Service.Interfaces;
using TeamContactTelegramBot.Service.Interfaces.System;
using TeamContactTelegramBot.Service.Users;
using TeamContactTelegramBot.Service.Users.Interfaces;

namespace TeamContactTelegramBot.Service
{
    public class Common : ICommon
    {
        private readonly ICommonUOW _commonUOW;
        private IUsersCommon _users;

        public IUsersCommon UsersService => _users ?? (_users = new UsersCommon(_commonUOW));

        public Common()
        {
            _commonUOW = new CommonUOW();
        }
    }
}
