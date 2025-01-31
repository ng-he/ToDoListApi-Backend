using Microsoft.EntityFrameworkCore;
using ToDoListApi.Data;
using ToDoListApi.Models;

namespace ToDoListApi.Services
{
    public class TaskListManipulationService (ToDoListDbContext dbContext)
    {
        private readonly ToDoListDbContext _dbContext = dbContext;

        public async Task<IEnumerable<TaskListModel>> GetAllTaskList()
            => await _dbContext.TaskLists.ToListAsync();

        public async Task<TaskListModel?> GetTaskListAsync(int id)
            => await _dbContext.TaskLists.Where(t => t.TaskListId == id).FirstOrDefaultAsync(); 

        public async Task<IEnumerable<TaskModel>> GetTasksByTaskList(int taskListId)
            => await _dbContext.Tasks.Where(t => t.TaskListId == taskListId).Include(t => t.TaskList).ToListAsync();

        public async Task<int> AddTaskListAsync(TaskListInputModel input)
        {
            var newTaskList = new TaskListModel(input);
            await _dbContext.TaskLists.AddAsync(newTaskList);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateTaskListAsync(int id, TaskListInputModel input)
        {
            var taskList = await _dbContext.TaskLists.Where(t => t.TaskListId == id).FirstOrDefaultAsync();
            if(taskList != null)
            {
                taskList.TaskListName = input.TaskListName;
                taskList.Color = input.Color;
                return await _dbContext.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<int> DeleteTaskListAsync(int id)
        {
            var taskList = await _dbContext.TaskLists.Where(t => t.TaskListId == id).FirstOrDefaultAsync();
            if (taskList != null)
            {
                _dbContext.TaskLists.Remove(taskList);
                return await _dbContext.SaveChangesAsync();
            }

            return 0;
        }
    }
}
