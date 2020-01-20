using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ReminderBot.Models.Commands
{
    public class RemoveCommand : CommandBase
    {
        public override string Name => @"/remove ";

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var text = message.Text.Remove(0, Name.Length);
            var chatId = message.Chat.Id;

            if (!int.TryParse(text.Remove(text.Length - 1), out int index))
            {
                await botClient.SendTextMessageAsync(chatId, "Неверный формат команды /remove\n" +
                "Параметром команды является индекс удаляемого элемента");
                return;
            }

            var timer = ReminderTimer.Source;
            timer.RemoveRecord(index);
        }
    }
}
