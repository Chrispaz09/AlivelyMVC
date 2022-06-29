using AlivelyMVC.Data;
using AlivelyMVC.Models;
using AlivelyMVC.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlivelyMVC.Controllers
{
    //Controller not Scaffolded.
    public class SmartGoals : Controller
    {
        public readonly IMapper _mapper;

        private readonly AlivelyDbContext _alivelyDbContext;

        public SmartGoals(IMapper mapper, AlivelyDbContext alivelyDbContext)
        {
            _mapper = mapper;

            _alivelyDbContext = alivelyDbContext;
        }

        public IActionResult Index()
        {
            var goalList = _alivelyDbContext.SMARTGoals.Where(users => users.UserUuid == Guid.Parse(HttpContext.Session.GetString("CurrentUserUuid"))).ToList();

            return View(goalList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SMARTGoalViewModel smartGoalViewModel)
        {
            var smartGoal = _mapper.Map<SMARTGoal>(smartGoalViewModel);

            smartGoal.Uuid = Guid.NewGuid();

            smartGoal.UserUuid = Guid.Parse(HttpContext.Session.GetString("CurrentUserUuid"));

            HttpContext.Session.SetString("CurrentSMARTGoalUuid", smartGoal.Uuid.ToString());

            var addedSMARTGoal = await _alivelyDbContext.SMARTGoals.AddAsync(smartGoal);

            await _alivelyDbContext.SaveChangesAsync();

            TempData["Success"] = "SMART Goal created successfully!";

            return RedirectToAction("Create", "Tasks");
        }

        public IActionResult Update(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }

            var goalFromDatabase = _alivelyDbContext.SMARTGoals.FirstOrDefault(goals => goals.Id == id);

            return View(goalFromDatabase);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(SMARTGoal smartGoal)
        {
            if (ModelState.IsValid)
            {
                _alivelyDbContext.SMARTGoals.Update(smartGoal);

                await _alivelyDbContext.SaveChangesAsync();

                TempData["Success"] = "SMART Goal updated successfully!";

                HttpContext.Session.SetString("CurrentUserUuid", smartGoal.UserUuid.ToString());

                return RedirectToAction("Index");   
            }

            return View(smartGoal);
        }

        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var goalFromDatabase = _alivelyDbContext.SMARTGoals.FirstOrDefault(goals => goals.Id == id);

            return View(goalFromDatabase);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var goalFromDatabase = _alivelyDbContext.SMARTGoals.FirstOrDefault(goals => goals.Id == id);

            var tasks = GetTasks(id);

            if(tasks.Any())
            {
                _alivelyDbContext.Task.RemoveRange(GetTasks(id));
            }

            if (goalFromDatabase is not null)
            {
                _alivelyDbContext.SMARTGoals.Remove(goalFromDatabase);

                await _alivelyDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return NotFound();
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _alivelyDbContext.SMARTGoals == null)
            {
                return NotFound();
            }

            var goals = await _alivelyDbContext.SMARTGoals
                .FirstOrDefaultAsync(m => m.Id == id);

            if (goals == null)
            {
                return NotFound();
            }

            goals.Tasks = GetTasks(id);

            return View(goals);
        }

        public List<Models.Task> GetTasks(int smartGoalId)
        {
            return _alivelyDbContext.Task.Where(tasks => tasks.SMARTGoal.Id == smartGoalId).ToList();
        }
    }
}
