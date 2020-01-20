using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ReminderBot.Models.Commands
{
    /// <summary>
    /// Основа команды.
    /// </summary>
    public abstract class CommandBase
    {
        /// <summary>
        /// Имя команды.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Выполнить команду.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="client">Клиент бота.</param>
        /// <returns>Выполнение асинхронных операций.</returns>
        public abstract Task Execute(Message message, TelegramBotClient client);

        /// <summary>
        /// Соответствует ли имя команды.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <returns>Соответствует ли имя команды.</returns>
        public bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }
    }
}
