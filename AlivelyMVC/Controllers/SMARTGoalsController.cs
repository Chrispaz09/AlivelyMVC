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

            return View(_mapper.Map<List<SMARTGoalViewModel>>(goalList));
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

        public IActionResult Edit(Guid uuid)
        {
            if(uuid == Guid.Empty)
            {
                return BadRequest();
            }

            var goalFromDatabase = _alivelyDbContext.SMARTGoals.FirstOrDefault(goals => goals.Uuid == uuid);

            return View(_mapper.Map<SMARTGoalViewModel>(goalFromDatabase));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SMARTGoalViewModel smartGoalViewModel)
        {
            if (ModelState.IsValid)
            {

                var smartGoal = await _alivelyDbContext.SMARTGoals.FirstOrDefaultAsync(goals => goals.Uuid == smartGoalViewModel.Uuid).ConfigureAwait(false);

                if(smartGoal is null)
                {
                    return NotFound();
                }

                smartGoal = _mapper.Map<SMARTGoalViewModel, SMARTGoal>(smartGoalViewModel, smartGoal);

                var smartGoalViewModelAdded = _alivelyDbContext.SMARTGoals.Update(smartGoal);

                await _alivelyDbContext.SaveChangesAsync();

                TempData["Success"] = "SMART Goal updated successfully!";

                HttpContext.Session.SetString("CurrentUserUuid", smartGoalViewModel.UserUuid.ToString());

                return RedirectToAction("Index");   
            }

            TempData["Error"] = "There was an error when attempting to update your SMART goal. If the error persists, contact support.";

            return View(smartGoalViewModel);
        }

        public IActionResult Delete(Guid uuid)
        {
            if (uuid == Guid.Empty)
            {
                return BadRequest();
            }

            var goalFromDatabase = _alivelyDbContext.SMARTGoals.FirstOrDefault(goals => goals.Uuid == uuid);

            if(goalFromDatabase is null)
            {
                return NotFound();
            }

            return View(_mapper.Map<SMARTGoalViewModel>(goalFromDatabase));
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(Guid uuid)
        {
            if (uuid == Guid.Empty)
            {
                return BadRequest();
            }

            var goalFromDatabase = _alivelyDbContext.SMARTGoals.FirstOrDefault(goals => goals.Uuid == uuid);

            var tasks = GetTasks(uuid);

            if(tasks.Any())
            {
                _alivelyDbContext.Task.RemoveRange(GetTasks(uuid));
            }

            if (goalFromDatabase is not null)
            {
                _alivelyDbContext.SMARTGoals.Remove(goalFromDatabase);

                await _alivelyDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return NotFound();
        }

        public async Task<IActionResult> Details(Guid uuid)
        {
            if (uuid == Guid.Empty)
            {
                return BadRequest();
            }

            var goal = await _alivelyDbContext.SMARTGoals
                .FirstOrDefaultAsync(m => m.Uuid == uuid);

            if (goal == null)
            {
                return NotFound();
            }

            goal.Tasks = GetTasks(uuid);

            return View(_mapper.Map<SMARTGoalViewModel>(goal));
        }

        public List<Models.Task> GetTasks(Guid smartGoalUuid)
        {
            return _alivelyDbContext.Task.Where(tasks => tasks.SMARTGoal.Uuid == smartGoalUuid).ToList();
        }
    }
}
