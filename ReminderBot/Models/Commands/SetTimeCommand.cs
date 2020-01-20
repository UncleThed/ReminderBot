using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ReminderBot.Models.Commands
{
    public class SetTimeCommand : CommandBase
    {
        public override string Name => @"/time ";

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var time = message.Text.Remove(0, Name.Length);
            var chatId = message.Chat.Id;

            if (!uint.TryParse(time.Remove(time.Length - 1), out uint number))
            {
                await botClient.SendTextMessageAsync(chatId, "Неверный формат команды /new\n" +
                "Первым параметром необходимо указать время таймера в виде <число><символ>," +
                "где символ может принимать следующие значения:\ns - секунды\nm - минуты\nh - часы\nd - дни" +
                "(Не удалось получить число для времени таймера)");
                return;
            }

            uint unit;

            switch (time.Substring(time.Length - 1))
            {
                case "s":
                    unit = 1;
                    break;
                case "m":
                    unit = 60;
                    break;
                case "h":
                    unit = 3600;
                    break;
                case "d":
                    unit = 86400;
                    break;
                default:
                    await botClient.SendTextMessageAsync(chatId, "Неверный формат команды /new\n" +
                    "Первым параметром необходимо указать время таймера в виде <число><символ>," +
                    "где символ может принимать следующие значения:\ns - секунды\nm - минуты\nh - часы\nd - дни" +
                    "\n(Не удалось получить единицу измерения времени)");
                    return;
            }

            var timer = ReminderTimer.Source;
            timer.AddTimeForNewRecord(chatId, number * unit);
        }
    }
}
