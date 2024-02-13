using System.Net;
using System.Net.Sockets;
using Telegram.Bot.Args;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using System.Text.Json;
using Telegram.Bot.Types.ReplyMarkups;
using SchoolChatGPT_v1._0.NeuralNetworkClasses;
using RepositoryDb.Models;
using RepositoryDb.Data;
using Repository.Models;

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
        private static NeuralNetwork neuralNetwork;
        private static NeuralNetworkRepository neuralNetworkRepository;
        private static Data data;
        public void Connect()
        {
            
            neuralNetworkRepository = new NeuralNetworkRepository();
            data = neuralNetworkRepository.SetSentensesForData();
            neuralNetwork = new NeuralNetwork(new Topology(data, inputCount: data.WordsData.Count, outputCount: 1, learningRate: 0.4, layers: new int[] { 30 }));
            
            client = new TelegramBotClient(token);
            client.OnMessage += Client_OnMessage;   
            StartBot();
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
                SendMessage("Я надеюсь помог вам разобраться в правиле, пишите ещё!");
                question = 0;
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
                    var examle = GetExemple(result);
                    resultExample = examle.Answer;
                    SendMessage(examle.Description);
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
            if("По теореме Пифагора" == msg.Text)
            {
                SendMessage("Отлично! Тогда давай подробно разберём правило...");
                var prop = Getprop(result);
                SendImage(prop.Image);
                Thread.Sleep(100);
                SendMessage(prop.Name + "\n\n" + prop.Description);
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
                    SendMessageAndButtons("Привет! Я помогу тебе решить любую задачу по математике", new List<List<KeyboardButton>>
                    {
                        new List<KeyboardButton>
                        {
                            new KeyboardButton {Text = "/start" }, new KeyboardButton {Text = "/info"}
                        }  ,
                        new List<KeyboardButton>
                        {
                             new KeyboardButton{Text = "/learn"}
                        }
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
                        //result = neuralNetworkRepository.FindClosestOutput(neuralNetworkRepository.Work(neuralNetwork, msg.Text), data.TrainingData);
                        result = 0.1;
                        SendMessage($"Давай попробуем решить эту задачу вместе! \n Как вы думайте, по каком правилу или теоеме решается задача? Если знаете, то напишите, чтобы я понимал что вы не ошибаетесь)");
                        question = 1;
                        break;
                    }
            }
        }
        private Property Getprop(double res)
        {
            var context = new RulesContext();
            var property = context.Properties.ToList().Where(x => x.Id == res * 10).FirstOrDefault();
            return property;

        }
        private Example GetExemple(double res)
        {
            var context = new RulesContext();
            var rules =  context.Rules.ToList();
            var property = context.Properties.ToList();
            var exemple = context.Examples.ToList();
            return property.Where(x => x.Id == res * 10).FirstOrDefault().Examples.First();

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