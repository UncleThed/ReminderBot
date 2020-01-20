using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ReminderBot.Models
{
    public sealed class ReminderTimer
    {
        private const uint MaxTick = uint.MaxValue / 2;

        private static readonly Lazy<ReminderTimer> lazy =
            new Lazy<ReminderTimer>(() => new ReminderTimer());

        private uint _tickCount = 0;

        private List<ReminderRecord> _records = new List<ReminderRecord>();

        private List<ReminderRecord> _oldRecords = new List<ReminderRecord>();

        private string _newMessage;

        public IReadOnlyList<ReminderRecord> Records => _records.AsReadOnly();
        
        public IReadOnlyList<ReminderRecord> OldRecords => _oldRecords.AsReadOnly();

        public static ReminderTimer Source { get { return lazy.Value; } }

        private ReminderTimer()
        {
            TimerCallback timerCallback = new TimerCallback(CheckTimer);

            var timer = new Timer(timerCallback, null, 0, 1000);
        }

        public void AddRecord(long chatId, uint ticks, string message)
        {
            _records.Add(new ReminderRecord(chatId, ticks + _tickCount, message));
            _records.OrderBy(r => r.Ticks);

            if (_tickCount == MaxTick)
            {
                foreach (var record in _records)
                {
                    record.Ticks -= _tickCount;
                }

                _tickCount = 0;
            }
        }

        public void AddMessageForNewRecord(string message)
        {
            _newMessage = message;
        }

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

        public void RemoveRecord(int index)
        {
            var currentRecord = _records.ElementAt(index);
            _records.Remove(currentRecord);
            _oldRecords.Add(currentRecord);
        }

        private async void CheckTimer(object obj)
        {
            _tickCount++;

            if (_records.Any())
            {
                var currentRecord = _records.First();
                if (currentRecord.Ticks == _tickCount)
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
