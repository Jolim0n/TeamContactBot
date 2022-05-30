using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using TeamContactTelegramBot.Domain.Task;
using TeamContactTelegramBot.Repositories;
using TeamContactTelegramBot.Service.Interfaces.Tasks;

namespace TeamContactTelegramBot.Service.Tasks
{
    public class TaskService : ITaskService
    {
        private ICommonUOW Common { get; }
        public TaskService(ICommonUOW sender)
        {
            Common = sender;
        }
        public async Task<Data.Tasks.Task> AddAsync(Data.Tasks.Task model)
        {
            return await Common.TaskRepository.Task.AddAsync(model);
        }

        public async Task<List<TaskDTO>> GetAllTasksAsync()
        {
            var allDoneTasks = await Common.TaskRepository.Task.GetAllTasksAsync();
            await CompleteModel(allDoneTasks);
            return allDoneTasks;
        }

        public async Task<List<TaskDTO>> GetAllTasksAnyStateAsync()
        {
            var allDoneTasks = await Common.TaskRepository.Task.GetAllTasksAnyStateAsync();
            await CompleteModel(allDoneTasks);
            return allDoneTasks;
        }

        public async Task<List<TaskDTO>> GetActiveTasksForProgrammerAsync(int userId)
        {
            var allDoneTasks = await Common.TaskRepository.Task.GetActiveTasksForProgrammerAsync(userId);
            await CompleteModel(allDoneTasks);
            return allDoneTasks;
        }

        public async Task<List<TaskDTO>> GetActiveTasksForAnalystAsync(int userId)
        {
            var allDoneTasks = await Common.TaskRepository.Task.GetActiveTasksForAnalystAsync(userId);
            await CompleteModel(allDoneTasks);
            return allDoneTasks;
        }
        public async Task<TaskDTO> UpdateStatusAsync(string code, byte state)
        {
            return await Common.TaskRepository.Task.UpdateStatusAsync(code, state);
        }

        public async Task<bool> CloseTaskAsync(string code)
        {
            return await Common.TaskRepository.Task.CloseTaskAsync(code);
        }

        public async Task GoogleAsync()
        {
            CalendarService service;
            GoogleCredential credential;

            try
            {
                string[] scopes = new string[] { CalendarService.Scope.Calendar };
                using (var stream = new FileStream(@"D:\meet.json", FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(scopes);
                }
                service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential
                });


                Event calendarEvent = new Event();
                DateTime start = DateTime.Now;
                calendarEvent.Kind = "";
                calendarEvent.Summary = "prueba";
                calendarEvent.Status = "confirmed";
                calendarEvent.Visibility = "public";
                calendarEvent.Description = "prueba";

                calendarEvent.Creator = new Event.CreatorData
                {
                    Email = "denyskosinskiy@gmail.com", //email@example.com
                    Self = true
                };

                calendarEvent.Organizer = new Event.OrganizerData
                {
                    Email = "metallogan13@gmail.com",
                    Self = true
                };

                calendarEvent.Start = new EventDateTime
                {
                    DateTime = start,
                    TimeZone = "Ukraine/Kyiv_City"
                };

                calendarEvent.End = new EventDateTime
                {
                    DateTime = start.AddHours(1),
                    TimeZone = "Ukraine/Kyiv_City"
                };

                calendarEvent.Recurrence = new String[] { "RRULE:FREQ=DAILY;COUNT=1" };
                calendarEvent.Sequence = 0;
                calendarEvent.HangoutLink = "";

                calendarEvent.ConferenceData = new ConferenceData
                {
                    CreateRequest = new CreateConferenceRequest
                    {
                        RequestId = "1234abcdef",
                        ConferenceSolutionKey = new ConferenceSolutionKey
                        {
                            Type = "hangoutsMeet"
                        },
                        Status = new ConferenceRequestStatus
                        {
                            StatusCode = "success"
                        }
                    },
                    EntryPoints = new List<EntryPoint>
                    {
                        new EntryPoint
                        {
                            EntryPointType = "video",
                            Uri = "",
                            Label = ""
                        }
                    },
                    ConferenceSolution = new ConferenceSolution
                    {
                        Key = new ConferenceSolutionKey
                        {
                            Type = "hangoutsMeet"
                        },
                        Name = "Google Meet",
                        IconUri = ""
                    },
                    ConferenceId = ""
                };

                //calendarEvent.EventType = "default";


                EventsResource.InsertRequest request = service.Events.Insert(calendarEvent, "denyskosinskiy@gmail.com");
                request.ConferenceDataVersion = 0;
                Event createEvent = request.Execute();
                string url = createEvent.HangoutLink;
            }
            catch (Exception ex)
            {

            }
        }
        
        #region Private
        private async Task CompleteModel(List<TaskDTO> models)
        {
            if (models == null || models.Count == 0)
                return;

            for (int i = 0; i < models.Count; i++)
            {
                var userModelProg = await Common.UsersRepository.Users.GetAsync(models[i].ProgRcd);
                models[i].ProgName = userModelProg.Name;
                var userModelAnalyst = await Common.UsersRepository.Users.GetAsync(models[i].AnalystRcd);
                models[i].AnalystName = userModelAnalyst.Name;
                models[i].StateName = models[i].State == 1 ? "Передано програмісту" :
                     models[i].State == 2 ? "На тестуванні аналітика" :
                     models[i].State == 3 ? "На перевірці менеджера" : "Задача закрита";
            }
            
        }
        #endregion
    }
}
