using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ReminderBot.Models.Commands
{
    /// <summary>
    /// Команда получения листинга всех напоминаний.
    /// </summary>
    public class GetListingCommand : CommandBase
    {
        /// <summary>
        /// Имя команды.
        /// </summary>
        public override string Name => @"/listing";

        /// <summary>
        /// Выполнить команду.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="client">Клиент бота.</param>
        /// <returns>Выполнение асинхронных операций.</returns>
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
