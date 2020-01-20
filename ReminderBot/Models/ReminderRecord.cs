namespace ReminderBot.Models
{
    /// <summary>
    /// Запись напоминания.
    /// </summary>
    public class ReminderRecord
    {
        /// <summary>
        /// ID чата с телеграм ботом.
        /// </summary>
        public long ChatId { get; }

        /// <summary>
        /// Число тиков таймера до напоминания.
        /// </summary>
        public uint Ticks { get; set; }

        /// <summary>
        /// Сообщение.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="chatId">ID чата с телеграм ботом.</param>
        /// <param name="ticks">Число тиков таймера до напоминания.</param>
        /// <param name="message">Сообщение.</param>
        public ReminderRecord(long chatId, uint ticks, string message)
        {
            ChatId = chatId;
            Ticks = ticks;
            Message = message;
        }
    }
}
