using System.ComponentModel.DataAnnotations;
using AlivelyMVC.Models;
using Task = AlivelyMVC.Models.Task;

namespace AlivelyMVC.ViewModels
{
    public class SMARTGoalViewModel
    {
        public Guid Uuid { get; set; } 

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
        public DateTime AchieveDate { get; set; }

        public List<Task>? Tasks { get; set; }

        public Guid UserUuid { get; set; }
    }
}
