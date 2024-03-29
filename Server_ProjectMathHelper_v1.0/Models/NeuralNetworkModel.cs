﻿using NeuralNetworkProject.NeuralNetworkClasses;
using Repository;
using Repository.Models;
using SchoolChatGPT_v1._0.NeuralNetworkClasses;
using Server_ProjectMathHelper_v1._0.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Server_ProjectMathHelper_v1._0.Models
{
    public class NeuralNetworkModel
    {
        public NeuralNetwork NeuralNetwork { get; set; }
        public Data Data { get; set; }
        public NeuralNetworkRepository NeuralNetworkRepository { get; set; }
       

        public string JsonName { get; set; }

        public DataDb DataDb { get; set; }

        public NeuralNetworkModel(string jsonName)
        {
            DataDb = new DataDb();
            NeuralNetworkRepository = new NeuralNetworkRepository();
            JsonName = jsonName;
            
            Data = new Data() { Name = JsonName };
           
            

        }
        public virtual void SaveData()
        {
            Data.Layers = NeuralNetwork.Layers;
            DataDb.SetNN(JsonSerializer.Serialize(Data),int.Parse(JsonName.Last().ToString()));
            Data.SetData();
        }

        public virtual double Work(string input)
        {
            return NeuralNetworkRepository.Work(NeuralNetwork, input);
        }

        public double Learn(int epoch)
        {
            return NeuralNetworkRepository.Learn(NeuralNetwork, epoch);
        }

        public double FindClosestOutput(double output, List<Tuple<double, double[]>> trainingData)
        {
            var outputs = SetOutputs(trainingData);
            var closestOutput = outputs[0];
            var minDifference = Math.Abs(outputs[0] - output);

            foreach (var number in outputs)
            {
                var difference = Math.Abs(number - output);
                if (difference < minDifference)
                {
                    minDifference = difference;
                    closestOutput = number;
                }
            }

            return closestOutput;
        }
        private double[] SetOutputs(List<Tuple<double, double[]>> trainingData)
        {
            var result = new List<double>();
            foreach (var item in trainingData)
            {
                if (!result.Contains(item.Item1))
                    result.Add(item.Item1);
            }
            return result.ToArray();
        }
    }
}
