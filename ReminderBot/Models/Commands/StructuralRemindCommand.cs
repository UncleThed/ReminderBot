using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ReminderBot.Models.Commands
{
    /// <summary>
    /// Строгая команда добавления записи.
    /// </summary>
    public class StructuralRemindCommand : CommandBase
    {
        /// <summary>
        /// Имя команды.
        /// </summary>
        public override string Name => @"/new ";

        /// <summary>
        /// Выполнить команду.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="client">Клиент бота.</param>
        /// <returns>Выполнение асинхронных операций.</returns>
        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var text = message.Text.Remove(0, Name.Length);
            var chatId = message.Chat.Id;

            string[] words = text.Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
            var time = words[0];

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
            timer.AddRecord(chatId, number * unit, words[1]);

            //TODO: parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown
            await botClient.SendTextMessageAsync(chatId, "Напоминание добавлено");
        }
    }
}
