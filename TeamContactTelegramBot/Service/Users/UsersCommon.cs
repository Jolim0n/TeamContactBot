using TeamContactTelegramBot.Repositories;
using TeamContactTelegramBot.Service.Interfaces.Users;
using TeamContactTelegramBot.Service.Users.Interfaces;

namespace TeamContactTelegramBot.Service.Users
{
    public class UsersCommon : IUsersCommon
    {
        private readonly ICommonUOW _commonUOW;

        private IUsersService _usersService;

        public IUsersService Users => _usersService ??= new UsersService(_commonUOW);

        public UsersCommon(ICommonUOW commonUOW)
        {
            _commonUOW = commonUOW;
        }
    }
}
