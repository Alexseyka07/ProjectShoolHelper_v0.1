using RepositoryDb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models
{
    public class NeuralNetworksData
    {
        public int Id { get; set; }
        public string DataJson { get; set; }
        [ForeignKey("IdRule")]
        public Rule Rule { get; set; }
    }
}
