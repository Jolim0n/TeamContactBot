namespace TeamContactTelegramBot.Repositories.Tasks.Interfaces
{
    public interface ITaskUOW : IDisposable
    {
        ITaskRepository Task { get; }
    }
}
