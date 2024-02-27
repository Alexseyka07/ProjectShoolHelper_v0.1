using NeuralNetworkProject.NeuralNetworkClasses;
using Repository.Models;
using SchoolChatGPT_v1._0.NeuralNetworkClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server_ProjectMathHelper_v1._0.Models
{
    public class AnswerNeuralNetwork : NeuralNetworkModel
    {
        public AnswerNeuralNetwork(string jsonName) : base(jsonName) 
        {
          LoadData();
        }
        private void LoadData()
        {
            if (!Data.GetData())
                SetData();
            NeuralNetwork = Data.NeuralNetwork;
        }

        public override void SaveData()
        {
            Data.Layers = NeuralNetwork.Layers;
            Data.SetData();
        }
        public string WorkStringAnswer(string input)
        {
            var res = Work(input);
            if (res >= 0.5) return "Да";
            return "Нет";
        }
        private void SetData()
        {
            //Создание WordsData
            var json = JsonSerializer.Deserialize<Dictionary<string, double>>(DataDb.AnswerNeuralNetworkJson.First().DataJson);
            Data.WordsData = new Dictionary<string, int>();
            Data.TrainingData = new List<Tuple<double, double[]>>();
            foreach (var words in json)
            {               
                var exampleWords = Vectorize.NormalizeText(words.Key);
                foreach (var word in exampleWords)
                {
                    if (!Data.WordsData.ContainsKey(word))
                        Data.WordsData.Add(word, Data.WordsData.Count + 1);
                }
            }
            //Создание TrainingData
            foreach (var words in json)
            {
                Data.TrainingData.Add(new Tuple<double, double[]>(words.Value, Vectorize.VectorizeText(Data.WordsData, words.Key)));
            }
            Data.LearningRate = 0.5;
            Data.NeuralNetwork = new NeuralNetwork(new Topology(Data, Data.WordsData.Count, 1, Data.LearningRate, new int[] { 30 }));
            Data.Layers = Data.NeuralNetwork.Layers;
            Data.SetData();
        }
    }
}
