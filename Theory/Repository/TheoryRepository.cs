using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using RepositoryDb.Models;

namespace RepositoryDb.Repository
{
    public class TheoryRepository
    {
        SqliteConnection connection = new SqliteConnection(@"Data Source=E:\Yandex Disk\YandexDisk\prodaction\GitHub\ProjectShoolHelper_v0.1\Theory\bin\Debug\net7.0\TheoryDb.db");
        SqliteCommand commandDb;

        public List<Rule> AllRules()
        {
            connection.Open();
            List<Rule> rules = new List<Rule>();
            string command = "SELECT Rules.name, Rules.description FROM Rules";
            commandDb = new SqliteCommand(command, connection);

            SqliteDataReader reader = commandDb.ExecuteReader();




            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //var id = reader.GetValue(0);
                    var name = reader.GetValue(1);
                    var description = reader.GetValue(2);
                  //  var image = reader.GetValue(3);
                    rules.Add(new Rule {  Name = (string)name, Description = (string)description });
                }
            }
            connection.Close();

            return rules;
        }

        public bool AddRule(Rule rule)
        {
            connection.Open();
            commandDb = new SqliteCommand();
            commandDb.CommandText = $"INSERT INTO Rules (name, description) VALUES ('{rule.Name}', '{rule.Description}')";
            commandDb.Connection = connection;
            commandDb.ExecuteNonQuery();
            connection.Close();
             return true;
        }

        
    }
}
