using SchoolChatGPT_v1._0.NeuralNetworkClasses;
using System;
using System.Collections.Generic;
using SetWordsForNeuralNetwork;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RepositoryDb.Repository;
using RepositoryDb.Models;
using System.Formats.Asn1;
using ProjectShoolHelper_v0._1;

namespace NNAnswerTheQuestion
{
    public class Program
    {
        private static NeuralNetwork neuralNetwork;
        private static Topology topology;
        private static List<Tuple<double, double[]>> trainingData;
        private static Dictionary<string, int> wordsData;
        private static DataNeuralNetwork dataNeuralNetwork;
        private static Data data;
        private static string dataName;
        private static string dataNNName;
        private static double error;
        static void Main(string[] args)
        {
           SetSentensesForData();
            Init();

           
            Console.ReadLine();
        }
        private List<Rule> GetDb()
        {
            return new TheoryRepository().AllRules();
        }
        private static void Init()
        {

            dataName = "testData";
            dataNNName = "dataNNtest";
            data = new Data(dataName);
            data.GetData();
            trainingData = data.TrainingData;
            wordsData = data.WordsData;
            topology = new Topology(data, inputCount: wordsData.Count, outputCount: 1, learningRate: 0.6, layers: new int[] {30});
            dataNeuralNetwork = new DataNeuralNetwork(dataNNName, topology);
            Learning();


        }

        private static void Learning()
        {
            double error = FirstLearning(topology);
            Console.WriteLine($"Ошибка после обучения: {error}");

            Thread thread = new Thread(Working);
            thread.Start();
        }

        private static void Working()
        {
            Console.WriteLine("[WORKING]");
            while (true)
            {
                var text = Console.ReadLine();
                if (text == "l")
                {
                    error = neuralNetwork.Learn(trainingData, epoch: int.Parse(Console.ReadLine()));

                    Console.WriteLine($"Ошибка после обучения: {error}");
                }
                else if (text == "stop") break;
                else if(text == "t")
                {
                    int res = 0;
                    
                    foreach (var item in trainingData)
                    {
                        Neuron outputNeuron1 = neuralNetwork.FeedForward(text, wordsData);
                        var resNeuron = outputNeuron1.Output;
                        if (item.Item1 == Math.Round(resNeuron, 1)) res++;
                    }
                    Console.WriteLine(res + "/" + trainingData.Count);
                }
                else if (text == "s")
                {
                    dataNeuralNetwork.SetData(neuralNetwork.Layers);
                    Console.WriteLine("Save finish");
                }
                else
                {
                    Neuron outputNeuron1 = neuralNetwork.FeedForward(text, wordsData);
                    var res = outputNeuron1.Output;
                    Console.WriteLine(res);
                }
            }
        }
        private static double FirstLearning(Topology topology)
        {
            double error = 100;
            Console.WriteLine("[Этап 1]");
            while (error > 15)
            {
                Console.WriteLine($"[LEARNING]restart: {error}");
                neuralNetwork = new NeuralNetwork(topology);
                error = neuralNetwork.Learn(trainingData, epoch: 30);
            }
            Console.WriteLine(error);
            Console.WriteLine("[Этап 2]");
            
            //while (error > 0.005)
            {
               // Console.WriteLine($"[LEARNING]more learning: {error}");
               // error = neuralNetwork.Learn(trainingData, epoch: 100);
            }
            Console.WriteLine("[LEARNING] learning end");
            Console.WriteLine($"[LEARNING] error: {error}");
            Console.WriteLine("---------------------------------------------------------");
            return error;
        }
        private static bool SaveData(Data d)
        {
            try
            {
                data.SetData(d.WordsData, d.TrainingData);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void SetSentensesForData()
        {
           data = new Data(dataName);
            data.GetData();
            string result = SetDataInTuple();
            data = SetSentenses.SetSentences(result);
            
           
            SaveData(data);
        }
        private static string SetDataInTuple()
        {
            string result = "";
            List<Tuple<double, string>> trainingData = SetDataQuestions.SetWords();
            foreach (var item in trainingData)
            {
                result += item.Item2 + " $" + item.Item1 + ": ";
            }
            return result;
        }
        private static string SetDataInConsole()
        {
            string result = "";
            string input = "";
            string expected = "";
            while (true)
            {
                Console.WriteLine("Введите вопрос");
                input = Console.ReadLine();
                if (input == "stop")
                    break;
                Console.WriteLine("Введите ожидаемый результат");
                string inputExpected = Console.ReadLine();
                if (inputExpected == null)
                    inputExpected = expected;
                else expected = inputExpected;
                result += input + " $" + expected + ": ";
            }
            return result;
        }
    }
}
