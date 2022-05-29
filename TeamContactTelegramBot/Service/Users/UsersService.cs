using TeamContactTelegramBot.Service.Interfaces;
using TeamContactTelegramBot.Service.Interfaces.System;

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

        public async Task<bool> CheckIfRegAsync(string log, string pass)
        {
            return await Common.UsersRepository.Users.CheckIfRegAsync(log, pass);

        }
    }
}
