using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ReminderBot.Models.Commands
{
    public class FreestyleRemindCommand : CommandBase
    {
        public override string Name => @"Напомни мне через ";

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var text = message.Text.Remove(0, Name.Length);
            var chatId = message.Chat.Id;

            string[] words = text.Split(new char[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);

            if (!uint.TryParse(words[0], out uint number))
            {
                await botClient.SendTextMessageAsync(chatId, "Неверный формат команды \"Напомни мне\"\n" +
                "Не удалось получить число для времени таймера");
                return;
            }

            uint unit;

            switch (words[1])
            {
                case "секунд":
                    unit = 1;
                    break;
                case "минут":
                    unit = 60;
                    break;
                case "часов":
                    unit = 3600;
                    break;
                case "дней":
                    unit = 86400;
                    break;
                default:
                    await botClient.SendTextMessageAsync(chatId, "Неверный формат команды \"Напомни мне\"\n" +
                    "\nНе удалось получить единицу измерения времени");
                    return;
            }

            var timer = ReminderTimer.Source;
            timer.AddRecord(chatId, number * unit, words[2]);

            //TODO: parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown
            await botClient.SendTextMessageAsync(chatId, "Напоминание добавлено");
        }
    }
}
