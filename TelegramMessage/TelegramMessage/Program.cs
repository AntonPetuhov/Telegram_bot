using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramMessage
{
    class Program
    {
        public static string token = "5713460548:AAHAem3It_bVQQrMcRvX2QNy7n5m_IUqLMY";
        // id = 199746847
        public static int ID = 199746847;

        public static string msg = "POSITIVE";

        public static TelegramBotClient bot;

        public static async Task SendMessage(string token, int destID, string text)
        {
            try
            {
                bot = new TelegramBotClient(token); // Подключение к боту
                Message ss = await bot.SendTextMessageAsync(destID, text); // Отправка сообщения
                Console.WriteLine($"Сообщение боту");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex);
            }
        }

        static void Main(string[] args)
        {

            Console.WriteLine("Sending message");
            SendMessage(token, ID, msg);
            Console.ReadLine();

        }
    }
}
