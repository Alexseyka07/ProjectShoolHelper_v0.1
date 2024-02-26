using Repository.Models;
using Repository.Data;
using RepositoryDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class DataDb
    {
        public List<Rule> Rules { get; set; }
        public List<Property> Properties { get; set; }
        public List<Example> Examples { get; set; }
        public List<NeuralNetworksData> NeuralNetworksData { get; set; }

        public DataDb() 
        { 
            GetData();
        }
        private void GetData()
        {
            var context = new RulesContext();

            Rules = context.Rules.Where(x => x.Id <= 4).ToList();
            Properties = context.Properties.Where(x => x.Rule.Id <= 4).ToList();
            Examples = context.Examples.Where(x => x.Property.Rule.Id <= 4).ToList();
            NeuralNetworksData = context.NeuralNetworksData.ToList();
        }
        public void SetNN(string data, int id)
        {
            var context = new RulesContext();
            context.NeuralNetworksData.ToList()[id - 1].DataJson = data;

            context.SaveChanges();

        }
    }
}
