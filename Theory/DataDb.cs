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

        public DataDb() 
        { 
            GetData();
        }
        private void GetData()
        {
            var context = new RulesContext();

            Rules = context.Rules.ToList();
            Properties = context.Properties.ToList();
            Examples = context.Examples.ToList();
        } 
    }
}
