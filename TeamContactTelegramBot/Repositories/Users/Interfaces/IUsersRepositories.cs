namespace TeamContactTelegramBot.Repositories.Users.Interfaces
{
    public interface IUsersRepository
    {
        Task<List<Data.Users.Users>> GetListAsync();

        Task<(bool, byte)> CheckIfRegAsync(string log, string pass);

        Task<List<Data.Users.Users>> GetAllProgrammersAsync();

        Task<List<Data.Users.Users>> GetAllAnalystsAsync();

        Task<Data.Users.Users> GetAsync(int userRcd);

        Task<Data.Users.Users> AddAsync(Data.Users.Users user);
    }
}
