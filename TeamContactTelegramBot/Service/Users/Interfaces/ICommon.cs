using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamContactTelegramBot.Service.Users.Interfaces
{
    public interface ICommon 
    {
        IUsersCommon UsersService { get; }
    }
}
