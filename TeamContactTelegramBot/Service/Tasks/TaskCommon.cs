using TeamContactTelegramBot.Repositories;
using TeamContactTelegramBot.Service.Interfaces.Tasks;
using TeamContactTelegramBot.Service.Interfaces.Users;
using TeamContactTelegramBot.Service.Tasks.Interfaces;

namespace TeamContactTelegramBot.Service.Tasks
{
    public class TaskCommon : ITaskCommon
    {
        private readonly ICommonUOW _commonUOW;

        private ITaskService _taskService;

        public ITaskService Task => _taskService ??= new TaskService(_commonUOW);

        public TaskCommon(ICommonUOW commonUOW)
        {
            _commonUOW = commonUOW;
        }
    }
}
