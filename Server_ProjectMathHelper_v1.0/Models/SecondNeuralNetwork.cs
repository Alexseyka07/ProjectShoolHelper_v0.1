using NeuralNetworkProject.NeuralNetworkClasses;
using Repository;
using SchoolChatGPT_v1._0.NeuralNetworkClasses;
using Server_ProjectMathHelper_v1._0.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Server_ProjectMathHelper_v1._0.Models
{
    public class SecondNeuralNetwork : NeuralNetworkModel
    {
        public int RuleId { get; set; }
        public SecondNeuralNetwork(string jsonName, int ruleId) : base(jsonName)
        {
            RuleId = ruleId;
            LoadData();

        }
        private void LoadData()
        {
            if (!Data.GetDataInDataBase(RuleId, DataDb))
                SetData();
            NeuralNetwork = Data.NeuralNetwork;
        }

        
        private void SetData()
        {
            //Создание избр слов
            var rules = DataDb.Rules.Where(x => x.Id == RuleId);
            var properties = DataDb.Properties.Where(x => x.Rule.Id  == RuleId).ToList();
            var examples = DataDb.Examples.Where(x => x.Property.Rule.Id == RuleId).ToList();
            foreach (var rule in rules)
            {
                var json = JsonSerializer.Deserialize<Dictionary<string, double>>(rule.FavoriteWordsJson);
                foreach (var word in json)
                {
                    Data.FavoriteWords.Add(word.Key, word.Value);
                }
            }
            //Создание WordsData
            foreach (var example in examples)
            {
                var exampleWords = Vectorize.NormalizeText(example.Description);

                foreach (var word in exampleWords)
                {
                    if (!Data.WordsData.ContainsKey(word))
                        Data.WordsData.Add(word, Data.WordsData.Count + 1);
                }
            }
            //Создание TrainingData
            foreach (var example in examples)
            {
                if(example.Property.Id >= 10)
                    example.Property.Id = example.Property.Id%10;
                Data.TrainingData.Add(new Tuple<double, double[]>(example.Property.Id / 10.0, Vectorize.VectorizeText(Data.WordsData, example.Description, 0.5, Data.FavoriteWords)));
            }
            Console.WriteLine("введите LearningRate:");
            Data.LearningRate = 0.5;
            Data.NeuralNetwork = new NeuralNetwork(new Topology(Data, Data.WordsData.Count, 1, Data.LearningRate, new int[] { 30 }));
            Data.Layers = Data.NeuralNetwork.Layers;
            Data.SetData();
        }
    }
}
