using Microsoft.EntityFrameworkCore.Metadata;
using Repository;
using Server_ProjectMathHelper_v1._0.Classes;
using Server_ProjectMathHelper_v1._0.Models;
using Server_ProjectMathHelper_v1._0.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_ProjectMathHelper_v1._0.Controllers
{
    class TelegramBotController
    {
        private readonly View.IView _view;
        private readonly Dictionary<string, Action> _commands;
        private readonly Dictionary<int, Action<string>> _works;

        private AnswerNeuralNetwork answerNeuralNetwork;
        private List<SecondNeuralNetwork> secondNeuralNetworks;
        private FirstNeuralNetwork firstNeuralNetwork;
        private Tuple<double, double> result;
        private DataDb dataDb;

        private int _numQusestion;
        private int _badAnswer;
        private int _solvedExamples;
        private bool _exampleNotAnswer;
        private string _resultExample;

        public TelegramBotController()
        {
            _view = new TelegramBotView();
            _view.StartReceiving();
            _commands = new Dictionary<string, Action>() 
            {
                {"/start", Start },
            };
            _works = new Dictionary<int, Action<string>>()
            {
                {0, Work0 },
                {1, Work1 },
                {2, Work2 },
                {3, Work3 },
                {4, Work4 },

            };
            _numQusestion = 0;
            _badAnswer = 0;
            _solvedExamples = 0;
            _exampleNotAnswer = false;


            answerNeuralNetwork = new AnswerNeuralNetwork("AnswerNN");
            firstNeuralNetwork = new FirstNeuralNetwork("FirstNN");
            dataDb = new DataDb();
            

            Work();
        }

        private void Work0(string msg)
        {
            try
            {
                var item1 = firstNeuralNetwork.FindClosestOutput(firstNeuralNetwork.Work(msg), firstNeuralNetwork.Data.TrainingData);
                var secondNeuralNetwork = new SecondNeuralNetwork($"secondNN{item1}", (int)(item1 * 10));
                var item2 = secondNeuralNetwork.FindClosestOutput(secondNeuralNetwork.Work(msg), secondNeuralNetwork.Data.TrainingData) * 10;

                if(item1 * 10 > 2)
                    item2 = int.Parse("1" + item2.ToString());
                 result = new Tuple<double, double>(item1, item2);
                _view.SendMessage($"Давайте попробуем решить эту задачу вместе!\nКак вы думайте, по каком правилу или теореме решается задача? Если знаете, то напишите название, чтобы я понимал что вы не ошибаетесь)");
                _numQusestion = 1;

            }
            catch
            {
                Error();
            }
        }
        private void Work1(string msg)
        {

            _view.SendMessage("Тогда давай подробно разберём правило по которому решается задача...");
            _view.SendImage(dataDb.Properties.Where(x => x.Id == (int)result.Item2).First().Image);
            _view.SendMessage(dataDb.Properties.Where(x => x.Id == (int)result.Item2).First().Description);

            Thread.Sleep(100);
            _view.SendMessage("Стало ли более понятно?");

            _numQusestion = 2;
        }
        private void Work2(string msg)
        {
            switch (msg)
            {
                case "Да":
                    _view.SendMessage("Здорово! Не хотите решить несколько задач на эту тему, чтобы укрепить знания по этой теме?");
                    break;
                case "Нет":
                    _view.SendMessage("Жаль((( Я вам рассказал всё что знал, но я могу дать вам ссылки на интересные ресурсы, которые возможно смогут вам помочь ССЫЛКА. Изучив ссылки, не хотите проверить свои знания на реальных задачах? Это может помочь вам лучше понять тему...");
                    break;
            }
            _numQusestion = 3;
        }
        private void Work3(string msg)
        {
            switch (answerNeuralNetwork.WorkStringAnswer(msg))
            {
                case "Да":
                    _view.SendMessage("Отлично! Ловите задачу)");
                    _view.SendMessage(dataDb.Examples.Where(x => x.Property.Id == (int)result.Item2).ToList()[_solvedExamples].Description);
                    _resultExample = dataDb.Examples.Where(x => x.Property.Id == (int)result.Item2).ToList()[_solvedExamples].Answer;
                    if (_resultExample == "чтд" || _resultExample == "н")
                        _view.SendMessage("Это задание не имеет точного ответа. Введите своё решение, а потом сравним с моим!");
                    else
                        _view.SendMessage("Напишите ответ:");
                    break;
                case "Нет":
                    _view.SendMessage("Хорошо, я помог вам всем тем чем мог. Если ещё понадоблюсь, обязательно пишите!");
                    break;
            }

            _numQusestion = 4;
        }
        private void Work4(string msg)
        {
            if (_resultExample == msg)
            {
                _view.SendMessage("Отлично! Это правильный ответ!");
                _solvedExamples++;
                _badAnswer = 0;
                Thread.Sleep(100);

                _view.SendMessage("Хочешь ещё задачку?");
                _numQusestion = 3;
            }
            else
            {
                _badAnswer++;
                if (_badAnswer == 3)
                {
                    _view.SendMessage("Походу эта задача слишком сложная. Вот решение на задачу, проанализируй его и пойми, что у вас не получилось:");
                    var example = dataDb.Examples.Where(x => x.Property.Id == (int)result.Item2).ToList()[_solvedExamples];
                    _view.SendMessage($"{example.Solution}\n Ответ:{example.Answer}");
                    _solvedExamples++;
                    _view.SendMessage("Хочешь ещё задачку?");
                    _numQusestion = 3;
                }
                else
                {
                    _view.SendMessage("Эхххх... Это не правильный ответ:( Попробуйте ещё!");
                    _numQusestion = 4;
                }
            }

        }

        private void Start()
        {
            _numQusestion = 0;
            _badAnswer = 0;
            _solvedExamples = 0;
            _view.SendMessage("Привет! Я могу помочь тебе решить задачу по математике");
            _view.SendMessage("Введите свою задачу:");
        }
        private void Error()
        {
            _view.SendMessage("Ой, что-то случилось на сервере. Попробуйте отправить задачу ещё рази или повторите попытку позже");
            _numQusestion = 0;
        }
        private bool isCommand(string msg)
        {
            if(_commands.ContainsKey(msg))
            {
                _commands.FirstOrDefault(c => c.Key == msg).Value.Invoke();
                return true;
            }
            return false;
        }
        public void Work()
        {
            while(true)
            {
                var msg = _view.GetMessage();
                if(!isCommand(msg)) 
                _works.FirstOrDefault(w => w.Key == _numQusestion).Value.Invoke(msg);
            }
        }
    }
}
