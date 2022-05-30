using TeamContactTelegramBot.CoreSystem;
using TeamContactTelegramBot.Domain.Task;

namespace TeamContactTelegramBot.Service.Interfaces.Tasks
{
    public interface ITaskService : ITeamContactDI
    {
        Task<Data.Tasks.Task> AddAsync(Data.Tasks.Task model);

        Task<List<TaskDTO>> GetAllTasksAsync();
        Task<List<TaskDTO>> GetAllTasksAnyStateAsync();

        Task<List<TaskDTO>> GetActiveTasksForProgrammerAsync(int userId);

        Task<List<TaskDTO>> GetActiveTasksForAnalystAsync(int userId);
        Task<TaskDTO> UpdateStatusAsync(string code ,byte state);

        Task<bool> CloseTaskAsync(string code);

        Task GoogleAsync();
    }
}
