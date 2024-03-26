using Newtonsoft.Json.Bson;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Server_ProjectMathHelper_v1._0.View
{
    public class TelegramBotView : IView
    {
        private readonly string _token;
        private readonly TelegramBotClient _client;
        private Message _message;
        private bool _isMessage;

        public TelegramBotView()
        {
            _token = "6965102213:AAEeF0AZP9H2GeosUSv0Dj27Lzy4qaVWvaM";
            _client = new TelegramBotClient(_token);
            _client.OnMessage += Client_OnMessage;
            _isMessage = false;
            
           
        }

        private void Client_OnMessage(object? sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            _message = e.Message;
            _isMessage = true;
        }

        public async void SendMessage(string message)
        {
            await _client.SendTextMessageAsync(_message.Chat.Id, message);
        }
        
        public string GetMessage()
        {
            while (!_isMessage)
            {
                 
            }
            _isMessage = false;
            return _message.Text;
        }
        public void StartReceiving()
        {
            _client.StartReceiving();
            Console.WriteLine("[BOT]StartReceiving");
        }
        public void StopReceiving()
        {
            _client.StopReceiving();
            Console.WriteLine("[BOT]StopReceiving");
        }

        public async void SendImage(string imageAdress)
        {
            await _client.SendPhotoAsync(_message.Chat.Id, imageAdress);
        }
    }
}
