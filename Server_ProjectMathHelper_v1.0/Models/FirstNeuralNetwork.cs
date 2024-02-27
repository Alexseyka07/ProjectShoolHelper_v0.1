using NeuralNetworkProject.NeuralNetworkClasses;
using Repository;
using Repository.Models;
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
    public class FirstNeuralNetwork : NeuralNetworkModel
    {       
        public FirstNeuralNetwork(string jsonName) : base(jsonName) 
        {
            LoadData();
          
        }

        public virtual void SaveData()
        {
            Data.Layers = NeuralNetwork.Layers;
            Data.SetData();
        }
        private void LoadData()
        {           
            if (!Data.GetData())
                SetData();
            NeuralNetwork = Data.NeuralNetwork;
        }

       
        private void SetData()
        {
            //Создание избр слов
            foreach (var rule in DataDb.Rules)
            {
                var json = JsonSerializer.Deserialize<Dictionary<string, double>>(rule.FavoriteWordsJson);
                foreach (var word in json)
                {
                    Data.FavoriteWords.Add(word.Key, word.Value);
                }
            }
            //Создание WordsData
            foreach (var example in DataDb.Examples)
            {
                var exampleWords = Vectorize.NormalizeText(example.Description);

                foreach (var word in exampleWords)
                {
                    if (!Data.WordsData.ContainsKey(word))
                        Data.WordsData.Add(word, Data.WordsData.Count + 1);
                }
            }
            //Создание TrainingData
            foreach (var example in DataDb.Examples)
            {
                Data.TrainingData.Add(new Tuple<double, double[]>(example.Property.Rule.Id / 10.0, Vectorize.VectorizeText(Data.WordsData, example.Description, 0.5, Data.FavoriteWords)));
            }
            Console.WriteLine("введите LearningRate:");
            Data.LearningRate = double.Parse(Console.ReadLine());
            Data.NeuralNetwork = new NeuralNetwork(new Topology(Data, Data.WordsData.Count, 1, Data.LearningRate, new int[] { 30 }));
            Data.Layers = Data.NeuralNetwork.Layers;
            Data.SetData();        
        }
    }
}
