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
        private int badAnswer = 0;
        private int solvedExamples = 0;
        private bool exampleNotAnswer = false;
        private static NeuralNetworkRepository neuralNetworkRepository;
        private AnswerNeuralNetwork answerNeuralNetwork;
        private List<SecondNeuralNetwork> secondNeuralNetworks;
        private FirstNeuralNetwork firstNeuralNetwork;
        private static Data data;

        public NetRepository()
        {
            secondNeuralNetworks = new List<SecondNeuralNetwork>();
            answerNeuralNetwork = new AnswerNeuralNetwork("AnswerNN");
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
            badAnswer = 0;
            solvedExamples = 0;
        }


        private void Work4()
        {
            if (msg.Text == "/start")
            {
                Start();
                return;
            }
            if (msg.Text == "чтд" ||  msg.Text == "н")
            {
                SendMessage("Вот решение этой задачи:");
                var example = dataDb.Examples.Where(x => x.Property.Id == int.Parse(a)).ToList()[solvedExamples];
                SendMessage($"{example.Solution}\n Ответ:{example.Answer}");
                solvedExamples++;
                badAnswer = 0;
                SendMessage("Хочешь ещё задачку?");
                question = 3;
                return;
            }

            if (resultExample == msg.Text)
            {
                SendMessage("Отлично! Это правильный ответ!");
                solvedExamples++;
                badAnswer = 0;
        
                SendMessage("Хочешь ещё задачку?");
                question = 3;
                return;
            }          
            else
            {              
                badAnswer++;
                if (badAnswer == 3)
                {
                    SendMessage("Походу эта задача слишком сложная. Вот решение на задачу, проанализируй его и пойми, что у вас не получилось:");
                    var example = dataDb.Examples.Where(x => x.Property.Id == int.Parse(a)).ToList()[solvedExamples];
                    SendMessage($"{example.Solution}\n Ответ:{example.Answer}");
                    solvedExamples++;
                    badAnswer = 0;
                    SendMessage("Хочешь ещё задачку?");
                    question = 3;
                    return;
                }
                else
                {
                    SendMessage("Эхххх... Это не правильный ответ:( Попробуйте ещё!");
                    question = 4;
                    return;
                }              
            }
           
        }
        private void Work3()
        {
            if (msg.Text == "/start")
            {
                Start();
                return;
            }
            switch (answerNeuralNetwork.WorkStringAnswer(msg.Text))
            {
                case "Да":
                    SendMessage("Отлично! Ловите задачу)");
                    SendMessage(dataDb.Examples.Where(x => x.Property.Id == int.Parse(a)).ToList()[solvedExamples].Description);
                    resultExample = dataDb.Examples.Where(x => x.Property.Id == int.Parse(a)).ToList()[solvedExamples].Answer;
                    if(resultExample == "чтд" || resultExample == "н")
                        SendMessage("Это задание не имеет точного ответа. Введите своё решение, а потом сравним с моим!");
                    else
                        SendMessage("Напишите ответ:");
                    break;
                case "Нет":
                    SendMessage("Хорошо, я помог вам всем чем мог. Если ещё понадоблюсь, обязательно пишите!");
                    question = 0;
                    Start();
                    break;
            }
            
            question = 4;
        }
        private void Work2()
        {
            if (msg.Text == "/start")
            {
                Start();
                return;
            }
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
 
                SendMessage(" Тогда давай подробно разберём правило по которому решается задача...");
                SendImage(dataDb.Properties.Where(x => x.Id == int.Parse(a)).First().Image);
                SendMessage(dataDb.Properties.Where(x => x.Id == int.Parse(a)).First().Description);

                SendMessage("Стало ли более понятно?");
            
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
                        try
                        {
                            result = neuralNetworkRepository.FindClosestOutput(firstNeuralNetwork.Work(msg.Text), firstNeuralNetwork.Data.TrainingData);
                            var r1 = (result * 10).ToString();
                            result = neuralNetworkRepository.FindClosestOutput(secondNeuralNetworks[int.Parse(a) - 1].Work(msg.Text), secondNeuralNetworks[int.Parse(a) - 1].Data.TrainingData);
                            a = (result * 10).ToString();
                            if (int.Parse(r1) > 2)
                                a = 1 + a;
                            SendMessage($"Давайте попробуем решить эту задачу вместе!\nКак вы думайте, по каком правилу или теореме решается задача? Если знаете, то напишите название, чтобы я понимал что вы не ошибаетесь)");
                            question = 1;
                          
                        }
                        catch 
                        {

                            msg.Text = "/start";
                        }
                        break;
                    }
            }
        }
       
       

        private async void SendMessage(string sendMessage)
        {
            await client.SendTextMessageAsync(msg.Chat.Id, sendMessage);
            Thread.Sleep(150);
        }
        private async void SendImage(string sendImageAddress)
        {
            await client.SendPhotoAsync(msg.Chat.Id, sendImageAddress);
            Thread.Sleep(150);
        }
        private async void SendMessageAndButtons(string sendMessage, List<List<KeyboardButton>> keyboards)
        {
            await client.SendTextMessageAsync(msg.Chat.Id, sendMessage, replyMarkup: GetButtons(keyboards));
              Thread.Sleep(150);
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