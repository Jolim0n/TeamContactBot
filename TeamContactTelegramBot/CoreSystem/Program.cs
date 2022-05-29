using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using TeamContactTelegramBot.Controllers;
using TeamContactTelegramBot.Controllers.Requests;
using TeamContactTelegramBot.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using TeamContactTelegramBot.Service.Users;
using TeamContactTelegramBot.Service.Users.Interfaces;

namespace TeamContactTelegramBot.CoreSystem
{
    class StartUp
    {
        static ITelegramBotClient bot = new TelegramBotClient("5301791346:AAHYNy6OLsS50qnLVcQFbET61zZ99BHge-Q");
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var messController = new MessageController();
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                var procces = await messController.ProcessText(new ApiStringRequest { StringRequest = message.Text });
                await botClient.SendTextMessageAsync(message.Chat, procces);
                return;
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
