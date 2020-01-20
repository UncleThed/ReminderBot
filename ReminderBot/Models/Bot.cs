using ReminderBot.Models.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;

namespace ReminderBot.Models
{
    public static class Bot
    {
        private static TelegramBotClient _botClient;
        private static List<CommandBase> _commands;

        public static IReadOnlyList<CommandBase> Commands => _commands.AsReadOnly();

        public static async Task<TelegramBotClient> GetBotClientAsync()
        {
            if (_botClient != null)
            {
                return _botClient;
            }

            _commands = new List<CommandBase>();
            _commands.Add(new StartCommand());
            //TODO: Add more commands

            _botClient = new TelegramBotClient(AppSettings.Key);
            string hook = string.Format(AppSettings.Url, "api/message/update");
            await _botClient.SetWebhookAsync(hook);

            return _botClient;
        }
    }
}
