using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamContactTelegramBot.CoreSystem;

namespace TeamContactTelegramBot.Service.Interfaces.Users
{
    public interface IUsersService : ITeamContactDI
    {
        Task<List<Data.Users.Users>> GetListAsync();

        Task<(bool, byte, int)> CheckIfRegAsync(string log, string pass);

        Task<List<Data.Users.Users>> GetAllProgrammersAsync();

        Task<List<Data.Users.Users>> GetAllAnalystsAsync();

        Task<Data.Users.Users> AddAsync(Data.Users.Users user);
    }
}
