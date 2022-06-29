using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlivelyMVC.Data;
using AlivelyMVC.Models;
using AlivelyMVC.ViewModels;
using AutoMapper;
using Task = AlivelyMVC.Models.Task;
using Ardalis.GuardClauses;

namespace AlivelyMVC.Controllers
{
    //Controller Scaffolded.
    public class TasksController : Controller
    {
        private readonly IMapper _mapper;

        private readonly AlivelyDbContext _context;

        public TasksController(IMapper mapper, AlivelyDbContext context)
        {
            _mapper = mapper;

            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var currentSMARTGoal = _context.SMARTGoals.FirstOrDefault(goals => goals.UserUuid == Guid.Parse(HttpContext.Session.GetString("CurrentUserUuid")));

            var taskViewModels = new List<TaskViewModel>();

            if(currentSMARTGoal == null)
            {
                return View(taskViewModels);
            }

            HttpContext.Session.SetString("CurrentSMARTGoalUuid", currentSMARTGoal.Uuid.ToString());

            var tasks = await _context.Task.Where(tasks => tasks.SMARTGoal.Uuid == currentSMARTGoal.Uuid).ToListAsync().ConfigureAwait(false);

            taskViewModels = _mapper.Map<List<Task>, List<TaskViewModel>>(tasks);

            return View(taskViewModels);
        }

        public async Task<IActionResult> Details(Guid uuid)
        {
            if (uuid == Guid.Empty )
            {
                return BadRequest();
            }

            var task = await _context.Task.FirstOrDefaultAsync(tasks => tasks.Uuid == uuid);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        public async Task<IActionResult> Create()
        {
            var smartGoal = await _context.SMARTGoals.FirstOrDefaultAsync(goals => goals.Uuid == Guid.Parse(HttpContext.Session.GetString("CurrentSMARTGoalUuid"))).ConfigureAwait(false);

            Guard.Against.Null(smartGoal, nameof(smartGoal), "SMART goal is missing.");

            smartGoal.Tasks = GetTasks(smartGoal.Uuid);

            var placeHolderTask = new TaskViewModel()
            {
                SMARTGoal = smartGoal
            };

            return View(placeHolderTask);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskViewModel taskViewModel)
        {
            var smartGoalUuid = Guid.Parse(HttpContext.Session.GetString("CurrentSMARTGoalUuid"));

            var smartGoal = await _context.SMARTGoals.FirstOrDefaultAsync(goals => goals.Uuid == smartGoalUuid);

            var task = _mapper.Map<Task>(taskViewModel);

            task.Uuid = Guid.NewGuid();

            task.SMARTGoal = smartGoal;

            var addedTask = await _context.Task.AddAsync(task).ConfigureAwait(false);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Task created successfully!";

            return RedirectToAction(nameof(Create), _mapper.Map<SMARTGoal>(addedTask.Entity.SMARTGoal));
        }

        public async Task<IActionResult> Edit(Guid uuid)
        {
            if(uuid == Guid.Empty)
            {
                return BadRequest();
            }

            var task = await _context.Task.FirstOrDefaultAsync(tasks => tasks.Uuid == uuid).ConfigureAwait(false);

            if (task == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<TaskViewModel>(task));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid uuid, TaskViewModel taskViewModel)
        {
            if (uuid != taskViewModel.Uuid)
            {
                return NotFound();
            }

            var task = await _context.Task.FirstOrDefaultAsync(tasks => tasks.Uuid == taskViewModel.Uuid).ConfigureAwait(false);

            Guard.Against.Null(task, nameof(task), "Task not found. ");

            _mapper.Map<TaskViewModel, Task>(taskViewModel, task);

            _context.Update(task);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Task updated successfully!";

            return RedirectToAction(nameof(Index));
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(Guid uuid)
        {
            if (uuid == Guid.Empty)
            {
                return BadRequest();
            }

            var task = await _context.Task.FirstOrDefaultAsync(m => m.Uuid == uuid);

            if (task == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<TaskViewModel>(task));
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid uuid)
        {
            if(uuid == Guid.Empty)
            {
                return BadRequest();
            }

            var task = await _context.Task.FirstOrDefaultAsync(tasks => tasks.Uuid == uuid).ConfigureAwait(false);

            if (task != null)
            {
                _context.Task.Remove(task);
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Task deleted successfully!";

            return RedirectToAction(nameof(Index));
        }

        public List<Models.Task> GetTasks(Guid smartGoalUuid)
        {
            return _context.Task.Where(tasks => tasks.SMARTGoal.Uuid == smartGoalUuid).ToList();
        }
    }
}
