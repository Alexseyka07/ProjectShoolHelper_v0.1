using System.Net;
using System.Net.Sockets;
using Telegram.Bot.Args;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using System.Text.Json;
using Telegram.Bot.Types.ReplyMarkups;
using SchoolChatGPT_v1._0.NeuralNetworkClasses;

using Repository.Models;
using Repository;
using Server_ProjectMathHelper_v1._0.Models;

namespace Server_ProjectMathHelper_v1._0.Classes
{
    public class NetRepository
    {
        private static string token = "6965102213:AAEeF0AZP9H2GeosUSv0Dj27Lzy4qaVWvaM";
        private static Message msg;
        private static double result;
        private static string resultExample;
        private static int question = 0;
        private static TelegramBotClient client;
        private static DataDb dataDb = new DataDb();
        string a = "1";
        private static NeuralNetworkRepository neuralNetworkRepository;
        private List<SecondNeuralNetwork> secondNeuralNetworks;
        private FirstNeuralNetwork firstNeuralNetwork;
        private static Data data;

        public NetRepository()
        {
            secondNeuralNetworks = new List<SecondNeuralNetwork>();
            firstNeuralNetwork = new FirstNeuralNetwork("FirstNN");
            DataDb dataDb = new DataDb();
            for (int i = 1; i < dataDb.Rules.Where(x => x.Id <= 4).ToList().Count + 1; i++)
            {
                secondNeuralNetworks.Add(new SecondNeuralNetwork($"secondNN{i}", i));

            }
        }
        public void Connect()
        {
            
            neuralNetworkRepository = new NeuralNetworkRepository();
            
            //neuralNetwork = new NeuralNetwork(new Topology(data, inputCount: data.WordsData.Count, outputCount: 1, learningRate: 0.4, layers: new int[] { 30 }));
            
            client = new TelegramBotClient(token);
            client.OnMessage += Client_OnMessage;   
            StartBot();
            msg.Text = "/start";
        }

        private void StartBot()
        {
            question = 0;
            client.StartReceiving();
            
            Console.ReadLine();
            client.StopReceiving();
        }

        private async void Client_OnMessage(object? sender, MessageEventArgs e)
        {
            msg = e.Message;
            switch(question)
            {
                case 0: 
                    
                    Work0();
                    break;
                case 1:
                    Work1(); 
                    break;
                case 2:
                    Work2();
                    break;
                case 3:
                    Work3();
                    break;
                case 4:
                    Work4();
                    break;
                

            }
        }
        private void Start()
        {
            question = 0;
        }
        private void Work4()
        {
            if (resultExample == msg.Text)
            {
                SendMessage("Отлично! Это правильный ответ!");
                Thread.Sleep(100);
                SendMessage("Я надеюсь помог вам разобраться в правиле, пишите ещё!");
                question = 0;
            }
            if (msg.Text == "/start")
            {
                Start();

            }
            else
            {
                SendMessage("Эхххх... Это не правильный ответ:( Попробуйте ещё!");
                question = 4;
            }
           
        }
        private void Work3()
        {
            switch (msg.Text)
            {
                case "Да":
                    SendMessage("Отлично! Ловите задачу)");
                    SendMessage(dataDb.Examples.Where(x => x.Property.Id == int.Parse(a)).First().Description);
                    SendMessage("Напишите ответ:");
                    break;
                case "Нет":
                    SendMessage("Хорошо, я помог вам всем тем чем мог. Если ещё понадоблюсь, обязательно пишите!");
                    break;
            }
            question = 4;
        }
        private void Work2()
        {
            switch (msg.Text)
            {
                case "Да":
                    SendMessage("Здорово! Не хотите решить несколько задач на эту тему, чтобы укрепить знания по этой теме?");
                    break;
                case "Нет":
                    SendMessage("Жаль((( Я вам рассказал всё что знал, но я могу дать вам ссылки на интересные ресурсы, которые возможно смогут вам помочь ССЫЛКА. Изучив ссылки, не хотите проверить свои знания на реальных задачах? Это может помочь вам лучше понять тему...");
                    break;
            }
            question = 3;
        }
        private void Work1()
        {
            if("Нет" == msg.Text)
            {
                SendMessage(" Тогда давай подробно разберём правило...");
           

                SendMessage(dataDb.Properties.Where(x => x.Id == int.Parse(a)).First().Description);

                SendMessage("Стало ли более понятно?");
            }
            if (msg.Text == "/start")
            {
                Start();
                
            }
            question = 2;
        }
        private void Work0()
        {
            switch (msg.Text)
            {
                case "/start":
                    SendMessageAndButtons("Привет! Я могу помочь тебе решить задачу по математике", new List<List<KeyboardButton>>
                    {
                       
                    });
                   
                    break;
                case "/learn":
                    SendMessage("start learning");
                    break;
                case "/info":
                    SendMessage("InfoText");
                    break;
                default:
                    {
                        result = neuralNetworkRepository.FindClosestOutput(firstNeuralNetwork.Work(msg.Text), firstNeuralNetwork.Data.TrainingData);
                         a = (result*10).ToString();
                        result = neuralNetworkRepository.FindClosestOutput(secondNeuralNetworks[int.Parse(a)].Work(msg.Text), firstNeuralNetwork.Data.TrainingData);
                        a = (result * 10).ToString();
                        SendMessage($"Давайте попробуем решить эту задачу вместе!\nКак вы думайте, по каком правилу или теореме решается задача? Если знаете, то напишите название, чтобы я понимал что вы не ошибаетесь)");
                        question = 1;
                        break;
                    }
            }
        }
       
       

        private async void SendMessage(string sendMessage)
        {
            await client.SendTextMessageAsync(msg.Chat.Id, sendMessage);
        }
        private async void SendImage(string sendImageAddress)
        {
            await client.SendPhotoAsync(msg.Chat.Id, sendImageAddress);
        }
        private async void SendMessageAndButtons(string sendMessage, List<List<KeyboardButton>> keyboards)
        {
            await client.SendTextMessageAsync(msg.Chat.Id, sendMessage, replyMarkup: GetButtons(keyboards));
              
        }
        private static IReplyMarkup GetButtons(List<List<KeyboardButton>> keyboards)
        {
            var keyboard = new ReplyKeyboardMarkup()
            {
                Keyboard = keyboards
            };
            return keyboard;
        }

       
    }

    
}