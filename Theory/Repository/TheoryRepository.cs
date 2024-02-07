using Microsoft.Data.Sqlite;
using RepositoryDb.Models;

namespace RepositoryDb.Repository
{
    public class TheoryRepository
    {
        private SqliteConnection connection = new SqliteConnection(@"Data Source=E:\Yandex Disk\YandexDisk\prodaction\GitHub\ProjectShoolHelper_v0.1\Theory\bin\Debug\net7.0\TheoryDb.db");
        private SqliteCommand commandDb;

        public List<Rule> AllRules()
        {
            connection.Open();
            List<Rule> rules = new List<Rule>();
            string command = "SELECT * FROM Rules";
            commandDb = new SqliteCommand(command, connection);

            SqliteDataReader reader = commandDb.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var id = reader.GetValue(0);
                    var name = reader.GetValue(1);
                    var description = reader.GetValue(2);
                    //  var image = reader.GetValue(3);
                    rules.Add(new Rule { Id = (int)(Int64)id, Name = (string)name, Description = (string)description });
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