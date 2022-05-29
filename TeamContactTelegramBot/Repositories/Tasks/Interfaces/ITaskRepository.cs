
using TeamContactTelegramBot.Domain.Task;

namespace TeamContactTelegramBot.Repositories.Tasks.Interfaces
{
    public interface ITaskRepository
    {
        Task<Data.Tasks.Task> AddAsync(Data.Tasks.Task model);

        Task<List<TaskDTO>> GetAllTasksAsync();
        Task<List<TaskDTO>> GetAllTasksAnyStateAsync();

        Task<bool> CloseTaskAsync(string code);
    }
}
