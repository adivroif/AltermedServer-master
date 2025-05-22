using AltermedManager.Models.Entities;
using AltermedManager.Models.Enums;

namespace AltermedManager.Models.Dtos
    {
    public class NewUserDto
        {

        public Guid id { get; set; }
        public string name { get; set; }
        //public required string role { get; set; }
        }
    }
