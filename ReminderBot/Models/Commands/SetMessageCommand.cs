using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ReminderBot.Models.Commands
{
    /// <summary>
    /// Команда для задания сообщения напоминания.
    /// </summary>
    public class SetMessageCommand : CommandBase
    {
        /// <summary>
        /// Имя команды.
        /// </summary>
        public override string Name => @"/message ";

        /// <summary>
        /// Выполнить команду.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="client">Клиент бота.</param>
        /// <returns>Выполнение асинхронных операций.</returns>
        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var text = message.Text.Remove(0, Name.Length);
            var timer = ReminderTimer.Source;
            timer.AddMessageForNewRecord(text);
        }
    }
}
