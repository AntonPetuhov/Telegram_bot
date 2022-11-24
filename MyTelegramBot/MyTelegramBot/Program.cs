using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Configuration;

using System.Collections.Generic;


namespace MyTelegramBot
{
    class Program
    {


        //static ITelegramBotClient botClient = new TelegramBotClient("5713460548:AAHAem3It_bVQQrMcRvX2QNy7n5m_IUqLMY");

        // Когда пользователь отправляет сообщение, вызывается метод HandleUpdateAsync с объектом обновления Update, переданным в качестве аргумента
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // информация о пользователе и сообщении
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));

            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;

                if (message.Text.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Бот анализатора Bactec FX запущен.");
                    Console.WriteLine(message.Text);
                    Console.WriteLine(message.Chat.Id);
                    Console.WriteLine(message.From.FirstName + " " + message.From.LastName);
                    string name = (message.From.FirstName + " " + message.From.LastName);
                    long chatId = message.Chat.Id;
                    AddAppSettings(name, chatId.ToString());
                    return;
                }

                await botClient.SendTextMessageAsync(message.Chat, "Бот анализатора Bactec FX работает.");

                // var chatId = message.Chat.Id;
                // Console.WriteLine(message.Text);
                // Console.WriteLine(chatId);
            }

            //Console.WriteLine($"Received a '{}' message in chat {chatId}.");
        }
        
        
        public static async Task SendNotification(ITelegramBotClient botClient, string chat)
        {
            int ChatId = 199746847;
            //int ChatId = 1218086595;
            int chat_ = Int32.Parse(chat);
            string messageText = "Флакон № 0000000001" + "\n" + "Результат: Положительный";



            // Echo received message text
            Message sentMessage = await botClient.SendTextMessageAsync(
                                                                        chat_,
                                                                        messageText
                                                                        );

            
        }

        
        public static async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception;
            Console.WriteLine(ErrorMessage);

            /*
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
            */
        }

        // добавление значений в файл конфигурации
        static void AddAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                // если такого ключа нет, то добавить
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                // если есть - перезаписать значение
                else
                {
                    //settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }


        static void Main(string[] args)
        
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var botClient = new TelegramBotClient("5713460548:AAHAem3It_bVQQrMcRvX2QNy7n5m_IUqLMY");

            
        //AddAppSettings("A", "7654321");

        Console.WriteLine("Запущен бот " + botClient.GetMeAsync().Result.FirstName + " ID: " + botClient.GetMeAsync().Id);

            //var me = botClient.GetMeAsync();
            //Console.WriteLine(value: $"Start listening for @{me.Result.FirstName}");

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types

            };

            botClient.StartReceiving(HandleUpdateAsync,
                                     HandlePollingErrorAsync,
                                     receiverOptions,
                                     cancellationToken);

            // Отправка сообщения пользователю
            //SendNotification(botClient);

            var appSettings = ConfigurationManager.AppSettings;

            foreach (var key in appSettings.AllKeys)
            {
                SendNotification(botClient, appSettings[key]);
            }

            


                Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();

        }
    }
}
