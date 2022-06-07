using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlivelyMVC.Data;
using AlivelyMVC.Models;

namespace AlivelyMVC.Controllers
{
    //Controller Scaffolded.
    public class TasksController : Controller
    {
        private readonly AlivelyDbContext _context;

        public TasksController(AlivelyDbContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            return _context.Task != null ?
                        View(await _context.Task.ToListAsync()) :
                        Problem("Entity set 'AlivelyDbContext.Task'  is null.");
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Task == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .FirstOrDefaultAsync(m => m.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create
        public async Task<IActionResult> Create(SMARTGoal smartGoal)
        {
            smartGoal.Tasks = GetTasks(smartGoal.Id);

            var placeHolderTask = new Models.Task()
            {
                SMARTGoal = smartGoal
            };

            return View(placeHolderTask);
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Objective,Relevance,Value,Deadline,CreatedDate,Completed, SMARTGoal")] Models.Task createTask)
        {
            createTask.Id = 0;

            var smartGoal = await _context.SMARTGoals.FirstOrDefaultAsync(goals => goals.Id == createTask.SMARTGoal.Id);

            createTask.SMARTGoal = smartGoal;

            var addedTask = _context.Task.Add(createTask);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Task created successfully!";

            return RedirectToAction(nameof(Create), addedTask.Entity.SMARTGoal);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Task == null)
            {
                return NotFound();
            }

            var task = await _context.Task.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Objective,Relevance,Value,Deadline,CreatedDate,Completed")] Models.Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }
            try
            {
                _context.Update(task);

                await _context.SaveChangesAsync();

                TempData["Success"] = "Task updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(task.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Task == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .FirstOrDefaultAsync(m => m.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Task == null)
            {
                return Problem("Entity set 'AlivelyDbContext.Task'  is null.");
            }
            var task = await _context.Task.FindAsync(id);

            if (task != null)
            {
                _context.Task.Remove(task);
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Task deleted successfully!";

            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return (_context.Task?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public List<Models.Task> GetTasks(int smartGoalId)
        {
            return _context.Task.Where(tasks => tasks.SMARTGoal.Id == smartGoalId).ToList();
        }
    }
}
