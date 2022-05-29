using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamContactTelegramBot.CoreSystem;

namespace TeamContactTelegramBot.Service.Interfaces
{
    public interface IUsersService : ITeamContactDI
    {
        Task<List<Data.Users.Users>> GetListAsync();

        Task<bool> CheckIfRegAsync(string log, string pass);
    }
}
