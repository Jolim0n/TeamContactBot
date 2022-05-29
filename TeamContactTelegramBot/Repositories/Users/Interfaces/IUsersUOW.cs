namespace TeamContactTelegramBot.Repositories.Users.Interfaces
{
    public interface IUsersUOW : IDisposable
    {
        IUsersRepository Users { get; }
    }
}
