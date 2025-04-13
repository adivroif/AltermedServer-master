using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AltermedManager.Models.Entities
    {
    public class User
        {

        public Guid id { get; set; }
        public string name { get; set; }
        
        }
    }
