
namespace TeamContactTelegramBot.Repositories.Users
{
    public interface IUsersRepository
    {
        Task<List<Data.Users.Users>> GetListAsync();

        Task<bool> CheckIfRegAsync(string log, string pass);
    }
}
