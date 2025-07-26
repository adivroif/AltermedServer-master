using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AltermedManager.Models.Entities
    {
    public class User
        {

        public Guid id { get; set; }
        public required string name { get; set; }
        public required string firebaseId { get; set; }
        public string? msgToken { get; set; }
        public required string role { get; set; }
        
        }
    }
