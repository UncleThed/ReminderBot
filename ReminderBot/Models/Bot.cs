using ReminderBot.Models.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;

namespace ReminderBot.Models
{
    /// <summary>
    /// Управление клиентом бота.
    /// </summary>
    public static class Bot
    {
        /// <summary>
        /// Клиент бота.
        /// </summary>
        private static TelegramBotClient _botClient;

        /// <summary>
        /// Список команд бота.
        /// </summary>
        private static List<CommandBase> _commands;

        /// <summary>
        /// Список команд бота.
        /// </summary>
        public static IReadOnlyList<CommandBase> Commands => _commands.AsReadOnly();

        /// <summary>
        /// Получить клиент бота.
        /// </summary>
        /// <returns>Клиент бота.</returns>
        public static async Task<TelegramBotClient> GetBotClientAsync()
        {
            if (_botClient != null)
            {
                return _botClient;
            }

            _commands = new List<CommandBase>
            {
                new StartCommand(),
                new StructuralRemindCommand(),
                new FreestyleRemindCommand(),
                new SetMessageCommand(),
                new SetTimeCommand(),
                new GetListingCommand(),
                new RemoveCommand()
            };

            _botClient = new TelegramBotClient(AppSettings.Key);
            string hook = string.Format(AppSettings.Url, "api/message/update");
            await _botClient.SetWebhookAsync(hook);

            return _botClient;
        }
    }
}
