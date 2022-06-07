using AlivelyMVC.Models;

namespace AlivelyMVC.ViewModels
{
    public class DashboardViewModel
    {
        public List<SMARTGoal> SMARTGoals { get; set; } = new List<SMARTGoal>();

        public List<Models.Task> Tasks { get; set; } = new List<Models.Task>();

        public List<Models.Task> CompletedTasks { get; set; } = new List<Models.Task>();

        public List<Models.Task> IncompleteTasks { get; set; } = new List<Models.Task>();

        public List<SMARTGoal> GoalsOrderedByTargetDate { get; set; } = new List<SMARTGoal>();

        public List<SMARTGoal> GoalsOverdue { get; set; } = new List<SMARTGoal>();

        public SMARTGoal NextNearestGoal { get ; set; } = new SMARTGoal();

        public List<SMARTGoal> GoalsOrderedByTasksCompletedDecreasing { get; set; } = new List<SMARTGoal>();
    }
}
