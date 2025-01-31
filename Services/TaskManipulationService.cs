using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Text.Json;
using System.Text.Json.Nodes;
using ToDoListApi.Data;
using ToDoListApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ToDoListApi.Services
{
    public static class TaskDbSetExtensions
    {
        public static async Task<IEnumerable<TaskModel>> Within(this DbSet<TaskModel> tasks, string? timePeriod, bool? isCompleted)
        {
            if (timePeriod == null)
            {
                return isCompleted != null
                    ? await tasks.Where(t => t.IsCompleted == isCompleted).Include(t => t.TaskList).ToListAsync()
                    : await tasks.Include(t => t.TaskList).ToListAsync();
            }

            DateTime today = DateTime.Today;
            DateTime tomorrow = DateTime.Today.AddDays(1);
            DateTime nextWeek = DateTime.Today.AddDays(7);

            return (timePeriod) switch
            {
                "Today" => isCompleted != null
                    ? await tasks.Where(t => t.DueDate == today && t.IsCompleted == isCompleted).Include(t => t.TaskList).ToListAsync()
                    : await tasks.Where(t => t.DueDate == today).Include(t => t.TaskList).ToListAsync(),

                "Tomorrow" => isCompleted != null
                    ? await tasks.Where(t => t.DueDate == tomorrow && t.IsCompleted == isCompleted).Include(t => t.TaskList).ToListAsync()
                    : await tasks.Where(t => t.DueDate == tomorrow).Include(t => t.TaskList).ToListAsync(),

                "NextWeek" => isCompleted != null
                    ? await tasks.Where(t => t.DueDate > tomorrow && t.DueDate <= nextWeek && t.IsCompleted == isCompleted).Include(t => t.TaskList).ToListAsync()
                    : await tasks.Where(t => t.DueDate > tomorrow && t.DueDate <= nextWeek).Include(t => t.TaskList).ToListAsync(),

                _ => throw new InvalidOperationException("Invalid time period")
            };
        }
    }
    public class TaskManipulationService(ToDoListDbContext dbContext)
    {
        private readonly ToDoListDbContext _dbContext = dbContext;

        public async Task<IEnumerable<TaskModel>> GetTasksAsync(string? timePeriod, bool? isCompleted) 
            => await _dbContext.Tasks.Within(timePeriod, isCompleted);

        public async Task<TaskModel?> GetTaskByIdAsync(int id)
            => await _dbContext.Tasks.Where(t => t.TaskId == id).Include(t => t.TaskList).FirstOrDefaultAsync();

        public async Task<int> AddTaskAsync(TaskInputModel input)
        {
            var newTask = new TaskModel(input);
            await _dbContext.Tasks.AddAsync(newTask);

            if(input.TaskListId != null)
            {
                var taskList = await _dbContext.TaskLists.FirstOrDefaultAsync(tl => tl.TaskListId == input.TaskListId);
                taskList!.Count += 1;
            }

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateTaskAsync(int id, TaskInputModel input)
        {
            var task = await _dbContext.Tasks.Where(t => t.TaskId == id).FirstOrDefaultAsync();
            if (task != null)
            {
                task.TaskTitle = input.TaskTitle;
                task.TaskDescription = input.TaskDescription;
                task.DueDate = input.DueDate;
                task.IsCompleted = input.IsCompleted;

                if(task.TaskListId != input.TaskListId)
                {
                    if(task.TaskListId != null)
                    {
                        var oldTaskList = await _dbContext.TaskLists.FirstOrDefaultAsync(tl => tl.TaskListId == task.TaskListId);
                        oldTaskList!.Count -= 1;
                    }

                    if(input.TaskListId != null)
                    {
                        var newTaskList = await _dbContext.TaskLists.FirstOrDefaultAsync(tl => tl.TaskListId == input.TaskListId);
                        newTaskList!.Count += 1;
                    }
                }

                task.TaskListId = input.TaskListId;

                return await _dbContext.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<int> MarkCompletedAsync(int id)
        {
            var task = await _dbContext.Tasks.Where(t => t.TaskId == id).FirstOrDefaultAsync();
            if(task != null)
            {
                if (task.TaskListId != null)
                {
                    var taskList = await _dbContext.TaskLists.FirstOrDefaultAsync(tl => tl.TaskListId == task.TaskListId);
                    taskList!.Count -= 1;
                }

                task.IsCompleted = true;
                return await _dbContext.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<int> DeleteTaskAsync(int id)
        {
            var task = await _dbContext.Tasks.Where(t => t.TaskId == id).FirstOrDefaultAsync();
            if (task != null)
            {
                if (task.TaskListId != null)
                {
                    var taskList = await _dbContext.TaskLists.FirstOrDefaultAsync(tl => tl.TaskListId == task.TaskListId);
                    taskList!.Count -= 1;
                }

                _dbContext.Tasks.Remove(task);
                return await _dbContext.SaveChangesAsync();
            }

            return 0;
        }
    }
}
