using AlivelyMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace AlivelyMVC.ViewModels
{
    public class TaskViewModel
    {
        public Guid Uuid { get; set; }

        public string Title { get; set; }

        public string Objective { get; set; }

        public string Relevance { get; set; }

        [Display(Name = "Importance")]
        public int Value { get; set; }

        public DateTime Deadline { get; set; }

        public bool Completed { get; set; }

        public SMARTGoal SMARTGoal { get; set; }
    }
}
