using SchoolChatGPT_v1._0.NeuralNetworkClasses;
using Server_ProjectMathHelper_v1._0.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_ProjectMathHelper_v1._0.Models
{
    public class FirstNeuralNetwork
    {
        public NeuralNetwork NeuralNetwork { get; set; }
        public Data Data { get; set; }
        public NeuralNetworkRepository NeuralNetworkRepository { get; set; }

        public FirstNeuralNetwork(Topology topology)
        {
            
        }
    }
}
