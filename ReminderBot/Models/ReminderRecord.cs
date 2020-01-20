namespace ReminderBot.Models
{
    public class ReminderRecord
    {
        public long ChatId { get; }
        public uint Ticks { get; set; }
        public string Message { get; }

        public ReminderRecord(long chatId, uint ticks, string message)
        {
            ChatId = chatId;
            Ticks = ticks;
            Message = message;
        }
    }
}
