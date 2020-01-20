using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ReminderBot.Models.Commands
{
    public class GetListingCommand : CommandBase
    {
        public override string Name => @"/listing";

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var text = message.Text;
            var chatId = message.Chat.Id;

            var timer = ReminderTimer.Source;

            if (text == "/listing")
            {
                var records = timer.Records;
                for (var i = 0; i < timer.OldRecords.Count; i++)
                {
                    await botClient.SendTextMessageAsync(chatId, $"{i}. {records[i].Message} | {records[i].Ticks} seconds");
                }
            }
            else if (text == "/listing past records")
            {
                var records = timer.OldRecords;
                for (var i = 0; i < timer.OldRecords.Count; i++)
                {
                    await botClient.SendTextMessageAsync(chatId, $"{i}. {records[i].Message}");
                }
            }
            else
            {
                await botClient.SendTextMessageAsync(chatId, "Неверный формат команды /listing");
                return;
            }
        }
    }
}
