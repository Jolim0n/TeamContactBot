using TeamContactTelegramBot.Controllers.Requests;
using TeamContactTelegramBot.Service;
using TeamContactTelegramBot.Service.Users.Interfaces;

namespace TeamContactTelegramBot.Controllers
{
    public class MessageController
    {
        private readonly ICommon _service = new Common();

        private ICommon Service { get; }

        public MessageController(ICommon common)
        {
            _service = common;
        }

        public MessageController()
        {
        }

        public async Task<string> ProcessText(ApiStringRequest req)
        {
            if (req == null)
                return string.Empty;

            if (req.StringRequest.Contains("/start"))
            {
                return "Введіть логін та пароль: (/login {log} {pass})";
            }
            else
            {
                if (req.StringRequest.Contains("/login"))
                {
                    req.StringRequest = req.StringRequest.Replace("/login ", "");
                    var login = req.StringRequest.Substring(0, req.StringRequest.IndexOf(' '));
                    req.StringRequest = req.StringRequest.Replace(login + " ", "");
                    var password = req.StringRequest;

                    var ifReg = await _service.UsersService.Users.CheckIfRegAsync(login, password);
                    if (!ifReg)
                        return "Не вірно введений логін або пароль.";
                }
            }

            return "123";
        }
    }
}
