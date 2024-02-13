using Repository.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryDb.Models
{
    public class Rule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Property> Properties { get; set;}
    }
}