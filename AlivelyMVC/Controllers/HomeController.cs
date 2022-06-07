using AlivelyMVC.Data;
using AlivelyMVC.Models;
using AlivelyMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AlivelyMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private AlivelyDbContext _alivelyDbContext;

        public HomeController(ILogger<HomeController> logger, AlivelyDbContext alivelyDbContext)
        {
            _logger = logger;

            _alivelyDbContext = alivelyDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var allGoals = _alivelyDbContext.SMARTGoals;

            var allTasks = _alivelyDbContext.Task;

            DashboardViewModel dashboardViewModel = new()
            {
                SMARTGoals = allGoals.ToList(),
                Tasks = allTasks.ToList()
            };

            dashboardViewModel.NextNearestGoal = await allGoals.OrderBy(goals => goals.AchieveDate).FirstOrDefaultAsync().ConfigureAwait(false);

            dashboardViewModel.CompletedTasks = allTasks.Where(tasks => tasks.Completed == true).ToList();

            dashboardViewModel.IncompleteTasks = allTasks.Where(tasks => tasks.Completed == false).ToList();

            dashboardViewModel.GoalsOrderedByTargetDate = allGoals.OrderBy(goals => goals.AchieveDate).ToList();

            dashboardViewModel.GoalsOverdue = allGoals.Where(goals => goals.AchieveDate < DateTime.Now).ToList();

            dashboardViewModel.GoalsOrderedByTasksCompletedDecreasing = allGoals.OrderByDescending(goals => goals.Tasks.Where(tasks => tasks.Completed == true).Count()).ToList();

            return View(dashboardViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}