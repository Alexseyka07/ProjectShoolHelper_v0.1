using NeuralNetworkProject.NeuralNetworkClasses;
using Repository;
using SchoolChatGPT_v1._0.NeuralNetworkClasses;
using Server_ProjectMathHelper_v1._0.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Server_ProjectMathHelper_v1._0.Models
{
    public class NeuralNetworkModel
    {
        public NeuralNetwork NeuralNetwork { get; set; }
        public Data Data { get; set; }
        public NeuralNetworkRepository NeuralNetworkRepository { get; set; }
        public Dictionary<string, double> FavoriteWords = new Dictionary<string, double>();

        public Topology Topology { get; set; }

        public string JsonName { get; set; }

        protected DataDb DataDb { get; set; }

        public NeuralNetworkModel(string jsonName)
        {
            JsonName = jsonName;
            DataDb = new DataDb();
            Data = new Data() { Name = JsonName };
            LoadData();
            NeuralNetworkRepository = new NeuralNetworkRepository();
                   
            NeuralNetwork = new NeuralNetwork(Topology, Data.Layers);
            

        }

        public virtual void LoadData()
        {

        }

        public virtual void SetData()
        {
            foreach (var example in DataDb.Examples)
            {
                var exampleWords = Vectorize.NormalizeText(example.Description);

                foreach (var word in exampleWords)
                {
                    if (!Data.WordsData.ContainsKey(word))
                        Data.WordsData.Add(word, Data.WordsData.Count + 1);
                }
            }      
        }
        public double Work(string input)
        {
            return NeuralNetworkRepository.Work(NeuralNetwork, input);
        }

        public double Learn(int epoch)
        {
            return NeuralNetworkRepository.Learn(NeuralNetwork, epoch);
        }
    }
}
