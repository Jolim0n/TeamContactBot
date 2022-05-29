using TeamContactTelegramBot.Service.Interfaces.Tasks;

namespace TeamContactTelegramBot.Service.Tasks.Interfaces
{
    public interface ITaskCommon
    {
        ITaskService Task { get; }
    }
}
