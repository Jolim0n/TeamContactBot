
using TeamContactTelegramBot.Domain.Task;

namespace TeamContactTelegramBot.Repositories.Tasks.Interfaces
{
    public interface ITaskRepository
    {
        Task<Data.Tasks.Task> AddAsync(Data.Tasks.Task model);

        Task<List<TaskDTO>> GetAllTasksAsync();
        Task<List<TaskDTO>> GetAllTasksAnyStateAsync();

        Task<TaskDTO> UpdateStatusAsync(string code, byte state);

        Task<List<TaskDTO>> GetActiveTasksForProgrammerAsync(int userId);

        Task<List<TaskDTO>> GetActiveTasksForAnalystAsync(int userId);
        Task<bool> CloseTaskAsync(string code);
    }
}
