
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TeamContactTelegramBot.Controllers.Requests
{
    public class ApiStringRequest
    {
        public Message MessageRequest { get; set; }

        public ITelegramBotClient botClient { get; set; }
    }
}
