using NeuralNetworkProject.NeuralNetworkClasses;
using Repository;
using SchoolChatGPT_v1._0.NeuralNetworkClasses;
using Server_ProjectMathHelper_v1._0.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Server_ProjectMathHelper_v1._0.Models
{
    public class SecondNeuralNetwork : NeuralNetworkModel
    {
        private int ruleId;
        List<Dictionary<string, double>> FavoriteWordsList = new List<Dictionary<string, double>>();
        public SecondNeuralNetwork(string jsonName, int ruleId) : base(jsonName) 
        {
           
        }
        public override void LoadData()
        {

            foreach (var rule in DataDb.Rules)
            {
                FavoriteWordsList.Add(JsonSerializer.Deserialize<Dictionary<string, double>>(rule.FavoriteWordsJson));
            }
            var isData = Data.GetData();
            if (isData) NeuralNetwork = Data.NeuralNetwork;
            else SetData();
            Topology = new Topology(Data, Data.WordsData.Count, 1, Data.LearningRate, new int[] {30});
        }
        public override void SetData()
        {
            base.SetData();
            foreach (var example in DataDb.Examples)
            {
                Data.TrainingData.Add(new Tuple<double, double[]>(example.Property.Id / 10.0, Vectorize.VectorizeText(Data.WordsData, example.Description, 0.5, FavoriteWords)));
            }
            Data.FavoriteWords = FavoriteWords;
            Data.LearningRate = 0.5;
            Data.NeuralNetwork = new NeuralNetwork(new Topology(Data, Data.WordsData.Count, 1, Data.LearningRate, new int[] {30 }));
            Data.Layers = Data.NeuralNetwork.Layers;
            Data.SetData();
         
        }
    }
}
