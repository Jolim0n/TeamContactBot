using System.Text;
using TeamContactTelegramBot.Controllers.Requests;
using TeamContactTelegramBot.Controllers.Results;
using TeamContactTelegramBot.Data.Users;
using TeamContactTelegramBot.Domain.Task;
using TeamContactTelegramBot.Service;
using TeamContactTelegramBot.Service.Interfaces.System;
using Telegram.Bot;

namespace TeamContactTelegramBot.Controllers
{
    public class MessageController
    {
        private readonly ICommon _service = new Common();

        private byte role;
        private int userId;
        private ICommon Service { get; }

        public MessageController(ICommon common)
        {
            _service = common;
        }

        public MessageController()
        {
        }

        public async Task<List<Users>> GetAllProgrammers()
        {
            return await _service.UsersService.Users.GetAllProgrammersAsync(); 
        }

        public async Task<MessageResult> ProcessText(ApiStringRequest req)
        {
            if (req == null)
                return new MessageResult();

            if (req.MessageRequest.Text.Contains("/start"))
            {
                return new MessageResult { Message = "Введіть логін та пароль: (/login {log} {pass})", Role = 0 };
            }
            else
            {
                if (req.MessageRequest.Text.Contains("/login"))
                {
                    req.MessageRequest.Text = req.MessageRequest.Text.Replace("/login ", "");
                    var login = req.MessageRequest.Text.Substring(0, req.MessageRequest.Text.IndexOf(' '));
                    req.MessageRequest.Text = req.MessageRequest.Text.Replace(login + " ", "");
                    var password = req.MessageRequest.Text;

                    var ifReg = await _service.UsersService.Users.CheckIfRegAsync(login, password);
                    role = ifReg.Item2;
                    userId = ifReg.Item3;
                    if (ifReg.Item1) // bool
                    {
                        if (ifReg.Item2 == 1) // Role
                            return new MessageResult { Message = "Ви успішно ввійшли як менеджер.\n Choose commands: /inline | /keyboard.", Role = 1 };
                        else if (ifReg.Item2 == 2)
                            return new MessageResult { Message = "Ви успішно ввійшли як программіст.", Role = 2 };
                        else if (ifReg.Item2 == 3)
                            return new MessageResult { Message = "Ви успішно ввійшли як аналітик.", Role = 3 };
                    }
                    else
                    {
                        return new MessageResult { Message = "Не вірно введений логін або пароль.", Role = 0 };
                    }
                }
                else
                {
                    if (role == 3)
                    {
                        if (req.MessageRequest.Text.Contains("Задачі, які потребують тестуванню"))
                        {
                            var activeTasks = await _service.TaskService.Task.GetActiveTasksForAnalystAsync(userId);
                            var str = new StringBuilder("Задачі, які потрібно протестувати: \n\n");
                            foreach (var item in activeTasks)
                            {
                                str.Append($"Найменування задачі: {item.Name},\n" +
                                    $"Код задачі: {item.Code},\n" +
                                    $"ФІО программіста: {item.ProgName},\n" +
                                    $"ФІО аналітика: {item.AnalystName}, \n" +
                                    $"Опис задачі: {item.Description}, \n" +
                                    $"Залишилося часу: {item.DateEnd.Subtract(item.DateCreate)}, \n" +
                                    $"Дата створення: {item.DateCreate}, \n" +
                                    $"Термін виконання: {item.DateEnd}.\n\n");
                            }
                            await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, str.ToString());

                            // 1 or 0 {
                            await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Якщо хочете закрити задачу введіть 1, якщо ні 0:");
                            var updates = await req.botClient.GetUpdatesAsync();
                            var strCode = updates.FirstOrDefault().Message.Text;
                            do
                            {
                                updates = await req.botClient.GetUpdatesAsync();
                                strCode = updates.LastOrDefault().Message.Text;
                            } while (strCode.Equals(req.MessageRequest.Text));
                            var codeTask = Convert.ToByte(strCode);
                            req.MessageRequest.Text = strCode;
                            if (codeTask == 1)
                            {
                                await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Введіть код задачі, яку хочете закрити:");
                                do
                                {
                                    updates = await req.botClient.GetUpdatesAsync();
                                    strCode = updates.LastOrDefault().Message.Text;
                                } while (strCode.Equals(req.MessageRequest.Text));
                                var cdTask = strCode;
                                req.MessageRequest.Text = strCode;

                                await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Якщо в задачі немає помилки введіть ТАК щоб закрити НІ щоб повернути програмісту:");
                                do
                                {
                                    updates = await req.botClient.GetUpdatesAsync();
                                    strCode = updates.LastOrDefault().Message.Text;
                                } while (strCode.Equals(req.MessageRequest.Text));
                                var oneTask = strCode;
                                req.MessageRequest.Text = strCode;

                                var updateTask = new TaskDTO();

                                if (oneTask.Contains("ТАК"))
                                    updateTask = await _service.TaskService.Task.UpdateStatusAsync(cdTask, 3);
                                else
                                    updateTask = await _service.TaskService.Task.UpdateStatusAsync(cdTask, 1);

                                if (updateTask.Rcd == 0)
                                    await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Невірно введений код, повторіть спробу.");
                                else
                                    await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Статус задачі успішно змінений.");
                            }
                            else
                            {
                                await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Виберіть, що хочете зробити:");
                            }
                            // 1 or 0 }
                        }
                        else
                        {
                            if (req.MessageRequest.Text.Contains("Заплановані зустрічі"))
                            {
                                await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Заплановані зустрічі за посиланням: https://meet.google.com/");
                            }
                        }
                    }
                    else
                    {
                        if (role == 2)
                        {
                            if (req.MessageRequest.Text.Contains("Активні задачі"))
                            {
                                var activeTasks = await _service.TaskService.Task.GetActiveTasksForProgrammerAsync(userId);
                                var str = new StringBuilder("Задачі, які потрібно зробити: \n\n");
                                foreach (var item in activeTasks)
                                {
                                    str.Append($"Найменування задачі: {item.Name},\n" +
                                        $"Код задачі: {item.Code},\n" +
                                        $"ФІО программіста: {item.ProgName},\n" +
                                        $"ФІО аналітика: {item.AnalystName}, \n" +
                                        $"Опис задачі: {item.Description}, \n" +
                                        $"Залишилося часу: {item.DateEnd.Subtract(item.DateCreate)}, \n" +
                                        $"Дата створення: {item.DateCreate}, \n" +
                                        $"Термін виконання: {item.DateEnd}.\n\n");
                                }
                                await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, str.ToString());
                            }
                            else
                            {
                                if (req.MessageRequest.Text.Contains("Заплановані зустрічі"))
                                {
                                    await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Заплановані зустрічі за посиланням: https://meet.google.com/");
                                }
                                else
                                {
                                    if (req.MessageRequest.Text.Contains("Поставити статус Need Testing"))
                                    {
                                        var activeTasks = await _service.TaskService.Task.GetActiveTasksForProgrammerAsync(userId);
                                        var str = new StringBuilder("Виберіть задачу, котрій хочете поставити статус Need Testing: \n\n");
                                        foreach (var item in activeTasks)
                                        {
                                            str.Append($"Найменування задачі: {item.Name},\n" +
                                                $"Код задачі: {item.Code},\n" +
                                                $"ФІО программіста: {item.ProgName},\n" +
                                                $"ФІО аналітика: {item.AnalystName}, \n" +
                                                $"Опис задачі: {item.Description}, \n" +
                                                $"Залишилося часу: {item.DateEnd.Subtract(item.DateCreate)}, \n" +
                                                $"Дата створення: {item.DateCreate}, \n" +
                                                $"Термін виконання: {item.DateEnd}.\n\n");
                                        }
                                        await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, str.ToString());

                                        // Code task {
                                        await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Введіть код задачі:");
                                        var updates = await req.botClient.GetUpdatesAsync();
                                        var strCode = updates.FirstOrDefault().Message.Text;
                                        do
                                        {
                                            updates = await req.botClient.GetUpdatesAsync();
                                            strCode = updates.LastOrDefault().Message.Text;
                                        } while (strCode.Equals(req.MessageRequest.Text));
                                        var codeTask = strCode;
                                        req.MessageRequest.Text = strCode;

                                        var updateTask = await _service.TaskService.Task.UpdateStatusAsync(strCode, 2);

                                        if (updateTask.Rcd == 0)
                                            await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Невірно введений код, повторіть спробу.");
                                        else
                                            await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Статус задачі успішно змінений.");
                                        // Code task }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (req.MessageRequest.Text.Contains("Створити задачу"))
                            {
                                // Name task {
                                await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Введіть найменування задачі:");
                                var updates = await req.botClient.GetUpdatesAsync();
                                var str = updates.FirstOrDefault().Message.Text;
                                do
                                {
                                    updates = await req.botClient.GetUpdatesAsync();
                                    str = updates.LastOrDefault().Message.Text;
                                } while (str.Equals(req.MessageRequest.Text));
                                var nameTask = str;
                                req.MessageRequest.Text = str;
                                // Name task }

                                // Code task {
                                await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Введіть код задачі:");
                                do
                                {
                                    updates = await req.botClient.GetUpdatesAsync();
                                    str = updates.LastOrDefault().Message.Text;
                                } while (str.Equals(req.MessageRequest.Text));
                                var codeTask = str;
                                req.MessageRequest.Text = str;
                                // Code task }

                                // Programmer {
                                var programmers = await _service.UsersService.Users.GetAllProgrammersAsync();
                                var strProg = new StringBuilder("Додайте программіста - список программістів: \n");
                                foreach (var item in programmers)
                                {
                                    strProg.Append($"ФІО: {item.Name}, Code: {item.Code} \n");
                                }
                                await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, strProg.ToString());
                                do
                                {
                                    updates = await req.botClient.GetUpdatesAsync();
                                    str = updates.LastOrDefault().Message.Text;
                                } while (str.Equals(req.MessageRequest.Text));

                                var prog = str;
                                req.MessageRequest.Text = str;
                                // Programmer }

                                // Analyst {
                                var analysts = await _service.UsersService.Users.GetAllAnalystsAsync();
                                var strAnalysts = new StringBuilder("Додайте аналітика - Список аналітиків: \n");
                                foreach (var item in analysts)
                                {
                                    strAnalysts.Append($"ФІО: {item.Name}, Code: {item.Code} \n");
                                }
                                await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, strAnalysts.ToString());
                                do
                                {
                                    updates = await req.botClient.GetUpdatesAsync();
                                    str = updates.LastOrDefault().Message.Text;
                                } while (str.Equals(req.MessageRequest.Text));

                                var analystCode = str;
                                req.MessageRequest.Text = str;
                                // Analyst }

                                // Description {
                                await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Введіть опис задачі:");
                                do
                                {
                                    updates = await req.botClient.GetUpdatesAsync();
                                    str = updates.LastOrDefault().Message.Text;
                                } while (str.Equals(req.MessageRequest.Text));

                                var description = str;
                                req.MessageRequest.Text = str;
                                // Description }

                                await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Введіть термін виконання задачі в форматі (2022-05-29 16:00:00):");
                                do
                                {
                                    updates = await req.botClient.GetUpdatesAsync();
                                    str = updates.LastOrDefault().Message.Text;
                                } while (str.Equals(req.MessageRequest.Text));
                                var dtEnd = Convert.ToDateTime(str);

                                var createTask = await _service.TaskService.Task.AddAsync(new Data.Tasks.Task
                                {
                                    Code = codeTask,
                                    Name = nameTask,
                                    ProgRcd = Convert.ToInt32(prog),
                                    AnalystRcd = Convert.ToInt32(analystCode),
                                    Description = description,
                                    DateCreate = DateTime.Now,
                                    DateEnd = dtEnd,
                                    State = 1 // Передана программісту
                                });

                                if (createTask != null)
                                    return new MessageResult { Message = "Задача успішно додана.\nВиберіть, що хочете зробити:", Role = 1 };
                            }
                            else
                            {
                                if (req.MessageRequest.Text.Contains("Закрити задачу"))
                                {
                                    var allReadyTasks = await _service.TaskService.Task.GetAllTasksAsync();
                                    if (allReadyTasks == null || allReadyTasks.Count == 0)
                                    {
                                        await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Немає виконаних задач. Подивіться на всі задачі.");
                                    }
                                    else
                                    {
                                        var str = new StringBuilder("Задачі, які зроблені і перевірені: \n\n");
                                        foreach (var item in allReadyTasks)
                                        {
                                            str.Append($"Найменування задачі: {item.Name},\n" +
                                                $"Код задачі: {item.Code},\n" +
                                                $"ФІО программіста: {item.ProgName},\n" +
                                                $"ФІО аналітика: {item.AnalystName}, \n" +
                                                $"Опис задачі: {item.Description}, \n" +
                                                $"Дата створення: {item.DateCreate}, \n" +
                                                $"Термін виконання: {item.DateEnd}.\n\n");
                                        }
                                        await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, str.ToString());
                                        await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Виберіть код задачі, яку хочете закрити:");
                                        var updates = await req.botClient.GetUpdatesAsync();
                                        var strNew = updates.FirstOrDefault().Message.Text;
                                        do
                                        {
                                            updates = await req.botClient.GetUpdatesAsync();
                                            strNew = updates.LastOrDefault().Message.Text;
                                        } while (strNew.Equals(req.MessageRequest.Text));
                                        var codeTask = strNew;

                                        if (await _service.TaskService.Task.CloseTaskAsync(codeTask))
                                            await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Задача успішно закрита.");
                                        else
                                            await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Невірно введений код.");
                                    }
                                }
                                else
                                {
                                    if (req.MessageRequest.Text.Contains("Подивитися всі задачі"))
                                    {
                                        var allReadyTasks = await _service.TaskService.Task.GetAllTasksAnyStateAsync();
                                        var str = new StringBuilder("Список задач: \n\n");
                                        foreach (var item in allReadyTasks)
                                        {
                                            str.Append($"Найменування задачі: {item.Name},\n" +
                                                $"Код задачі: {item.Code},\n" +
                                                $"ФІО программіста: {item.ProgName},\n" +
                                                $"ФІО аналітика: {item.AnalystName}, \n" +
                                                $"Опис задачі: {item.Description}, \n" +
                                                $"Дата створення: {item.DateCreate}, \n" +
                                                $"Термін виконання: {item.DateEnd}, \n" +
                                                $"Залишилося часу: {item.DateEnd.Subtract(item.DateCreate)}, \n" + // вичісляємо оставшийся час
                                                $"Стан задачі: {item.StateName}.\n\n");
                                        }
                                        await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, str.ToString());
                                    }
                                    else
                                    {
                                        if (req.MessageRequest.Text.Contains("Додати нового співробітника"))
                                        {
                                            await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Введіть ФІО співробітника:");
                                            var updates = await req.botClient.GetUpdatesAsync();
                                            var strNew = updates.FirstOrDefault().Message.Text;
                                            do
                                            {
                                                updates = await req.botClient.GetUpdatesAsync();
                                                strNew = updates.LastOrDefault().Message.Text;
                                            } while (strNew.Equals(req.MessageRequest.Text));
                                            var nameWorker = strNew;
                                            req.MessageRequest.Text = strNew;

                                            await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Введіть особовий код співробітника:");
                                            do
                                            {
                                                updates = await req.botClient.GetUpdatesAsync();
                                                strNew = updates.LastOrDefault().Message.Text;
                                            } while (strNew.Equals(req.MessageRequest.Text));
                                            var codeWorker = strNew;
                                            req.MessageRequest.Text = strNew;

                                            await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Введіть логін співробітника:");
                                            do
                                            {
                                                updates = await req.botClient.GetUpdatesAsync();
                                                strNew = updates.LastOrDefault().Message.Text;
                                            } while (strNew.Equals(req.MessageRequest.Text));
                                            var loginWorker = strNew;
                                            req.MessageRequest.Text = strNew;

                                            await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Введіть пароль співробітника:");
                                            do
                                            {
                                                updates = await req.botClient.GetUpdatesAsync();
                                                strNew = updates.LastOrDefault().Message.Text;
                                            } while (strNew.Equals(req.MessageRequest.Text));
                                            var passWorker = strNew;
                                            req.MessageRequest.Text = strNew;

                                            await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Введіть роль співробітника \n(1 - менеджер, \n2 - програміст, \n3 - аналітик):");
                                            do
                                            {
                                                updates = await req.botClient.GetUpdatesAsync();
                                                strNew = updates.LastOrDefault().Message.Text;
                                            } while (strNew.Equals(req.MessageRequest.Text));
                                            var roleWorker = Convert.ToByte(strNew);
                                            req.MessageRequest.Text = strNew;

                                            var newWorker = await _service.UsersService.Users.AddAsync(new Users
                                            {
                                                Name = nameWorker,
                                                Code = codeWorker,
                                                Login = loginWorker,
                                                Password = passWorker,
                                                Role = roleWorker
                                            });
                                            await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, $"Додано нового співробітника: \n" +
                                                $"ФІО: {newWorker.Name},\n" +
                                                $"Код: {newWorker.Code},\n" +
                                                $"Логін: {newWorker.Login},\n" +
                                                $"Пароль: {newWorker.Password}.");
                                        }
                                        else
                                        {
                                            if (req.MessageRequest.Text.Contains("Створити віртуальну зустріч"))
                                            {
                                                await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Створіть віртуальну зустріч за посиланням: https://meet.google.com/");
                                            }
                                            else
                                            {
                                                if (req.MessageRequest.Text.Contains("Запланувати віртуальну зустріч"))
                                                {
                                                    await req.botClient.SendTextMessageAsync(req.MessageRequest.Chat.Id, "Заплануйте віртуальну зустріч за посиланням: https://meet.google.com/");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return new MessageResult { Message = String.Empty, Role = 0 };
        }
    }
}
