using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace IsTheKittenFed
{
    class Program
    {
        private static readonly string apiKey = "1884918837:AAHUH8ejLEf0Tfe9DyewL2FxVJBW1mC-mNY";
        private static TelegramBotClient tgClient;
        private static DateTime lastFeedTime;
        private static string lastUser = string.Empty;

        static void Main(string[] args)
        {
            tgClient = new TelegramBotClient(apiKey);
            tgClient.StartReceiving();
            tgClient.OnMessage += OnMessageHandler;
            Console.ReadLine();
            tgClient.StopReceiving();
        }

        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            var mes = e.Message;
            if (mes.Text != null)
            {
                switch (mes.Text)
                {
                    case "Котенка надо кормить?":
                        if (DateTime.Now.Ticks - lastFeedTime.Ticks > lastFeedTime.AddHours(4).Ticks)
                            await tgClient.SendTextMessageAsync(mes.Chat.Id, $"Да. Время последней кормежки: {lastFeedTime.ToString()}", replyMarkup: GetButtons());
                        else
                            await tgClient.SendTextMessageAsync(mes.Chat.Id, $"Нет. Время последней кормежки: {lastFeedTime.ToString()}\nПокормил(-a): {lastUser}", replyMarkup: GetButtons());
                        break;
                    case "Котенок покормлен":
                        await tgClient.SendTextMessageAsync(mes.Chat.Id, "Ок.");
                        lastUser = mes.From.Username;
                        lastFeedTime = DateTime.Now;
                        break;
                    default:
                        await tgClient.SendTextMessageAsync(mes.Chat.Id, "Выберите команду: ", replyMarkup: GetButtons());
                        break;
                }
            }
        }

        private static IReplyMarkup GetButtons()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{ new KeyboardButton { Text = "Котенка надо кормить?"}, new KeyboardButton { Text = "Котенок покормлен" } }
                }
            };
        }
    }
}
