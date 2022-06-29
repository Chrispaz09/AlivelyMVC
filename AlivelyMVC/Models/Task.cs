using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlivelyMVC.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        public Guid Uuid { get; set; } = Guid.NewGuid();

        public string Title { get; set; }
        
        public string Objective { get; set; }

        public string Relevance { get; set; }

        [Display(Name ="Importance")]
        public int Value { get; set; }

        public DateTime Deadline { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool Completed { get; set; }

        [ForeignKey("SMARTGoalId")]
        public SMARTGoal SMARTGoal { get; set; }
    }
}
