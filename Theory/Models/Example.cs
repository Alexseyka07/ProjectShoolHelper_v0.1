using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models
{
    public class Example
    {
        public int Id { get; set; }
        public string Description { get; set; }        
        public string Solution { get; set; }
        public string Answer { get; set; }
        public string ImageDescription { get; set; }

        public string ImageSolution { get; set; }

        [ForeignKey("IdProperty")]
        public Property Property { get; set; }
        

    }
}
