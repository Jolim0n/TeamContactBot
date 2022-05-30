using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using TeamContactTelegramBot.Controllers;
using TeamContactTelegramBot.Controllers.Requests;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TeamContactTelegramBot.CoreSystem
{
    class StartUp
    {
        static ITelegramBotClient bot = new TelegramBotClient("5301791346:AAHYNy6OLsS50qnLVcQFbET61zZ99BHge-Q");
        static MessageController messController = new MessageController();
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update.Message));
            if (update.Type == UpdateType.Message)
            { 
                var message = update.Message;
                var procces = await messController.ProcessText(new ApiStringRequest { MessageRequest = message, botClient = botClient });
                if (!string.IsNullOrEmpty(procces.Message))
                    await botClient.SendTextMessageAsync(message.Chat, procces.Message);

                // ----------------------------
                if (procces.Role != 0)
                {
                    ReplyKeyboardMarkup keyboard = new(new[]
                    {
                        new KeyboardButton[] {
                            procces.Role == 1 ? "Створити задачу" : procces.Role == 2 ? "Активні задачі" : "",
                            procces.Role == 1 ? "Закрити задачу" : procces.Role == 2 ? "Заплановані зустрічі" : "Заплановані зустрічі" },
                        new KeyboardButton[] {
                            procces.Role == 1 ? "Створити віртуальну зустріч" : procces.Role == 2 ? "Поставити статус Need Testing" : "Задачі, які потребують тестуванню",
                            procces.Role == 1 ? "Запланувати віртуальну зустріч" : procces.Role == 2 ? "" : "" },
                        new KeyboardButton[] { 
                            procces.Role == 1 ? "Подивитися всі задачі" : procces.Role == 2 ? "" : "",
                            procces.Role == 1 ? "Додати нового співробітника" : procces.Role == 2 ? "" : "" }
                    })
                    {
                        ResizeKeyboard = true
                    };
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Вибір:", replyMarkup: keyboard);
                    return;
                }
                // ----------------------------

                return;
            }

            if (update.Type == UpdateType.CallbackQuery)
            {
                //var message = update.Message;
                //var procces = await messController.ProcessText(new ApiStringRequest { StringRequest = message.Text });
                //await botClient.SendTextMessageAsync(message.Chat, procces);
                //return;
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        static void Main(string[] args)
        {

            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
    }
}
