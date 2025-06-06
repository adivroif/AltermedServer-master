using AltermedManager.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AltermedManager.Models.Entities
    {
    [Table("StoredNotifications")]
    public class StoredNotification
        {
        [Key]
        public int id { get; set; }
        [ForeignKey("userId")]
        public Guid userId { get; set; } // Doctor or patient
        [Required]
        public string title { get; set; }
        [Required]
        public string body { get; set; }
        public bool isRead { get; set; }
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        }
    }
