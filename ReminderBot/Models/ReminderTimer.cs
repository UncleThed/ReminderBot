using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ReminderBot.Models
{
    /// <summary>
    /// Таймер напоминаний.
    /// </summary>
    public sealed class ReminderTimer
    {
        /// <summary>
        /// Максимальное количество тиков таймера до сброса.
        /// </summary>
        private const uint MaxTick = uint.MaxValue / 2;

        /// <summary>
        /// Объект таймера ленивой загрузкой.
        /// </summary>
        private static readonly Lazy<ReminderTimer> _lazy =
            new Lazy<ReminderTimer>(() => new ReminderTimer());

        /// <summary>
        /// Счетчик тиков таймера.
        /// </summary>
        private uint _tickCount = 0;

        /// <summary>
        /// Актуальные записи напоминаний.
        /// </summary>
        private List<ReminderRecord> _records = new List<ReminderRecord>();

        /// <summary>
        /// Старые записи напоминаний.
        /// </summary>
        private List<ReminderRecord> _oldRecords = new List<ReminderRecord>();

        /// <summary>
        /// Сообщение для новой записи.
        /// </summary>
        private string _newMessage;

        /// <summary>
        /// Актуальные записи напоминаний.
        /// </summary>
        public IReadOnlyList<ReminderRecord> Records => _records.AsReadOnly();
        
        /// <summary>
        /// Старые записи напоминаний.
        /// </summary>
        public IReadOnlyList<ReminderRecord> OldRecords => _oldRecords.AsReadOnly();

        /// <summary>
        /// Объект таймера.
        /// </summary>
        public static ReminderTimer Source { get { return _lazy.Value; } }

        /// <summary>
        /// Приватный конструктор.
        /// </summary>
        private ReminderTimer()
        {
            TimerCallback timerCallback = new TimerCallback(CheckTimer);

            var timer = new Timer(timerCallback, null, 0, 1000);
        }

        /// <summary>
        /// Добавить запись.
        /// </summary>
        /// <param name="chatId">ID чата с ботом.</param>
        /// <param name="ticks">Число тиков таймера до напоминания.</param>
        /// <param name="message">Сообщение.</param>
        public void AddRecord(long chatId, uint ticks, string message)
        {
            _records.Add(new ReminderRecord(chatId, ticks + _tickCount, message));
            _records = _records.OrderBy(r => r.Ticks).ToList();

            if (_tickCount == MaxTick)
            {
                foreach (var record in _records)
                {
                    record.Ticks -= _tickCount;
                }

                _tickCount = 0;
            }
        }

        /// <summary>
        /// Добавить сообщение для новой записи.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        public void AddMessageForNewRecord(string message)
        {
            _newMessage = message;
        }

        /// <summary>
        /// Добавить время для новой записи.
        /// </summary>
        /// <param name="chatId">ID чата с ботом.</param>
        /// <param name="ticks">Число тиков таймера до напоминания.</param>
        public async void AddTimeForNewRecord(long chatId, uint ticks)
        {
            if (_newMessage != null)
            {
                AddRecord(chatId, ticks, _newMessage);
            }
            else
            {
                var botClient = await Bot.GetBotClientAsync();
                await botClient.SendTextMessageAsync(chatId, $"Отсутствует сообщение для новой записи");
            }
        }

        /// <summary>
        /// Удалить запись.
        /// </summary>
        /// <param name="index">Индекс записи.</param>
        public void RemoveRecord(int index)
        {
            var currentRecord = _records.ElementAt(index);
            _records.Remove(currentRecord);
            _oldRecords.Add(currentRecord);
        }

        /// <summary>
        /// Проверить напоминания.
        /// </summary>
        /// <param name="obj">Пустой параметр.</param>
        private async void CheckTimer(object obj)
        {
            _tickCount++;

            if (_records.Any())
            {
                var currentRecord = _records.First();
                if (currentRecord.Ticks <= _tickCount)
                {
                    var botClient = await Bot.GetBotClientAsync();
                    await botClient.SendTextMessageAsync(currentRecord.ChatId, $"Ты просил меня напомнить тебе {currentRecord.Message}");
                    _records.Remove(currentRecord);
                    _oldRecords.Add(currentRecord);
                }
            } 
        }
    }
}
