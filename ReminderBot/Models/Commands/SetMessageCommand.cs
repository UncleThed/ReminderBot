using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ReminderBot.Models.Commands
{
    public class SetMessageCommand : CommandBase
    {
        public override string Name => @"/message ";

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var text = message.Text.Remove(0, Name.Length);
            var timer = ReminderTimer.Source;
            timer.AddMessageForNewRecord(text);
        }
    }
}
