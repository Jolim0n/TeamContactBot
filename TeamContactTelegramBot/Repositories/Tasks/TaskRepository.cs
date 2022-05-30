using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.EntityFrameworkCore;
using TeamContactTelegramBot.Data.Tasks;
using TeamContactTelegramBot.Domain.Task;
using TeamContactTelegramBot.Repositories.Tasks.Interfaces;

namespace TeamContactTelegramBot.Repositories.Tasks
{
    public class TaskRepository : ITaskRepository
    {
        private ITaskUOW _uow;
        private TaskContext _context;
        private static MapperConfiguration _conf;
        private static IMapper _mapper;

        public TaskRepository(ITaskUOW taskUOW, TaskContext context)
        {
            _uow = taskUOW;
            _context = context;
            if (_conf == null)
            {
                _conf = new MapperConfiguration(cfg =>
                {
                    cfg.AddExpressionMapping();
                    cfg.CreateMap<Data.Tasks.Task, TaskDTO>().ReverseMap();
                });
                _conf.CompileMappings();
                _mapper = _conf.CreateMapper();
            }
        }

        public async Task<List<TaskDTO>> GetAllTasksAsync()
        {
            using TaskContext context = new TaskContext();
            return (await context.DbTask.Where(x => x.State == 3).ToListAsync())
                ?.Select(x => _mapper.Map<TaskDTO>(x)).ToList();
        }

        public async Task<List<TaskDTO>> GetAllTasksAnyStateAsync()
        {
            using TaskContext context = new TaskContext();
            return (await context.DbTask.ToListAsync())
                ?.Select(x => _mapper.Map<TaskDTO>(x)).ToList();
        }

        public async Task<List<TaskDTO>> GetActiveTasksForProgrammerAsync(int userId)
        {
            using TaskContext context = new TaskContext();
            return (await context.DbTask.Where(x=> x.State == 1 && x.ProgRcd == userId).ToListAsync()) // select * from Tasks where Task_State == 1 and Task_ProgRcd == :userID
                ?.Select(x => _mapper.Map<TaskDTO>(x)).ToList();
        }

        public async Task<List<TaskDTO>> GetActiveTasksForAnalystAsync(int userId)
        {
            using TaskContext context = new TaskContext();
            return (await context.DbTask.Where(x => x.State == 2 && x.AnalystRcd == userId).ToListAsync()) // select * from Tasks where Task_State == 2 and Task_AnalystRcd == :userID
                ?.Select(x => _mapper.Map<TaskDTO>(x)).ToList();
        }

        public async Task<TaskDTO> UpdateStatusAsync(string code, byte state)
        {
            using TaskContext context = new TaskContext();
            var taskForUpdate = await context.DbTask.FirstOrDefaultAsync(x => x.Code == code);

            if (taskForUpdate == null)
                return new TaskDTO();

            taskForUpdate.State = state;
            context.Update(taskForUpdate);
            await context.SaveChangesAsync();

            return _mapper.Map<TaskDTO>(taskForUpdate);
        }

        public async Task<bool> CloseTaskAsync(string code)
        {
            using TaskContext context = new TaskContext();
            var task = await context.DbTask.FirstOrDefaultAsync(x => x.Code == code);

            if (task == null)
                return false;

            task.State = 4;
            context.Update(task);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Data.Tasks.Task> AddAsync(Data.Tasks.Task model)
        {
            using TaskContext context = new TaskContext();
            model.Rcd = await GetNewRcdAsync(model.Rcd);
            await context.DbTask.AddAsync(model);
            await context.SaveChangesAsync();
            return model;
        }

        #region private
        private async Task<int> GetNewRcdAsync(int rcd)
        {
            rcd = 0;
            Data.Tasks.Task checkTask;
            do
            {
                rcd++;
                checkTask = await _context.DbTask.FirstOrDefaultAsync(x => x.Rcd == rcd);
            } while (checkTask != null);
            return rcd;
        }

        #endregion
    }
}
