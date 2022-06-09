using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlivelyMVC.Models
{
    public class SMARTGoal
    {
        [Key]
        public int Id { get; set; }

        public Guid Uuid { get; set; } = Guid.NewGuid();

        [Required]
        [Display(Name = "specify")]
        public string Target { get; set; }

        [Required]
        [Display(Name = "measure")]
        public string Measure { get; set; }

        [Required]
        [Display(Name = "achievable")]
        public string Plan { get; set; }

        [Required]
        [Display(Name = "relevant")]
        public string Execution { get; set; }

        public bool Completed { get; set; }

        [Required]
        [Display(Name = "target date")]
        public DateTime AchieveDate { get; set; } = DateTime.Now;

        public List<Task>? Tasks { get; set; }
    }
}
