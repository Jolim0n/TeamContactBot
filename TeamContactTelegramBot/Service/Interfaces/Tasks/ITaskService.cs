using TeamContactTelegramBot.CoreSystem;
using TeamContactTelegramBot.Domain.Task;

namespace TeamContactTelegramBot.Service.Interfaces.Tasks
{
    public interface ITaskService : ITeamContactDI
    {
        Task<Data.Tasks.Task> AddAsync(Data.Tasks.Task model);

        Task<List<TaskDTO>> GetAllTasksAsync();
        Task<List<TaskDTO>> GetAllTasksAnyStateAsync();

        Task<bool> CloseTaskAsync(string code);

        Task GoogleAsync();
    }
}
