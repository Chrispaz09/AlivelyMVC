using AlivelyMVC.Data;
using AlivelyMVC.Models;
using AlivelyMVC.Services;
using AlivelyMVC.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AlivelyMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IMapper _mapper;

        private UserService _userService;

        private AlivelyDbContext _alivelyDbContext;

        public HomeController(ILogger<HomeController> logger, IMapper mapper, AlivelyDbContext alivelyDbContext)
        {
            _logger = logger;

            _mapper = mapper;

            _alivelyDbContext = alivelyDbContext;

            _userService = new UserService();
        }

        public IActionResult Index()
        {
            HttpContext.Session.SetString("CurrentUserUuid", "");

            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            Guid userUuid = Guid.Parse(HttpContext.Session.GetString("CurrentUserUuid"));

            if(userUuid == Guid.Empty)
            {
                throw new BadHttpRequestException("User Uuid is missing.");
            }

            var allGoals = _alivelyDbContext.SMARTGoals.Where(smartGoal => smartGoal.UserUuid == userUuid);

            DashboardViewModel dashboardViewModel = new();

            if (allGoals.Count() > 0)
            {
                var allTasks = _alivelyDbContext.Task.Where(task => task.SMARTGoal.Uuid == allGoals.FirstOrDefault().Uuid);

                dashboardViewModel.SMARTGoals = allGoals.ToList();

                dashboardViewModel.Tasks = allTasks.ToList();

                dashboardViewModel.NextNearestGoal = await allGoals.OrderBy(goals => goals.AchieveDate).FirstOrDefaultAsync().ConfigureAwait(false);

                dashboardViewModel.CompletedTasks = allTasks.Where(tasks => tasks.Completed == true).ToList();

                dashboardViewModel.IncompleteTasks = allTasks.Where(tasks => tasks.Completed == false).ToList();

                dashboardViewModel.GoalsOrderedByTargetDate = allGoals.OrderBy(goals => goals.AchieveDate).ToList();

                dashboardViewModel.GoalsOverdue = allGoals.Where(goals => goals.AchieveDate < DateTime.Now).ToList();

                dashboardViewModel.GoalsOrderedByTasksCompletedDecreasing = allGoals.OrderByDescending(goals => goals.Tasks.Where(tasks => tasks.Completed == true).Count()).ToList();

                return View(dashboardViewModel);
            }

            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(UserViewModel userViewModel)
        {
            var user = _mapper.Map<User>(userViewModel);
 
            var httpResponseMessage = await _userService.SignupUserAsync(user).ConfigureAwait(false);

            if(!httpResponseMessage.IsSuccessStatusCode)
            {
                TempData["Error"] = "Sign up was unsuccessful. If the error persist, please contact support.";

                return View();
            }

            var addedUser = await httpResponseMessage.Content.ReadFromJsonAsync<User>().ConfigureAwait(false);

            HttpContext.Session.SetString("CurrentUserUuid", addedUser.Uuid.ToString());

            return RedirectToAction("Dashboard");
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

        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {            
            var httpResponseMessage = await _userService.LoginUserAsync(loginViewModel.Username, loginViewModel.Password).ConfigureAwait(false);

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized || httpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TempData["Error"] = "Login was unsuccessful.";

                return RedirectToAction("Index");
            }

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                TempData["Error"] = "Internal server error. If the error persist, please contact support.";

                return RedirectToAction("Index");
            }

            var user = await httpResponseMessage.Content.ReadFromJsonAsync<User>().ConfigureAwait(false);

            HttpContext.Session.SetString("CurrentUserUuid", user.Uuid.ToString());

            return RedirectToAction("Dashboard");
        }
    }
}