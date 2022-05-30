using TeamContactTelegramBot.Repositories;
using TeamContactTelegramBot.Service.Interfaces.Users;

namespace TeamContactTelegramBot.Service.Users
{
    public class UsersService : IUsersService
    {
        private ICommonUOW Common { get; }
        public UsersService(ICommonUOW sender)
        {
            Common = sender;
        }
        public string Send()
        {
            return string.Empty;
        }

        public async Task<List<Data.Users.Users>> GetListAsync()
        {
            return await Common.UsersRepository.Users.GetListAsync();
        }
        public async Task<(bool, byte, int)> CheckIfRegAsync(string log, string pass)
        {
            return await Common.UsersRepository.Users.CheckIfRegAsync(log, pass);

        }
        public async Task<List<Data.Users.Users>> GetAllProgrammersAsync()
        {
            return await Common.UsersRepository.Users.GetAllProgrammersAsync();
        }
        public async Task<List<Data.Users.Users>> GetAllAnalystsAsync()
        {
            return await Common.UsersRepository.Users.GetAllAnalystsAsync();
        }

        public async Task<Data.Users.Users> AddAsync(Data.Users.Users user)
        {
            return await Common.UsersRepository.Users.AddAsync(user);
        }
    }
}
