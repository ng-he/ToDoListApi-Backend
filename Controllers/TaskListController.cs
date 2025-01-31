using Microsoft.AspNetCore.Mvc;
using ToDoListApi.Models;
using ToDoListApi.Services;

namespace ToDoListApi.Controllers
{
    [ApiController]
    [Route("api/tasklist")]
    public class TaskListController(TaskListManipulationService taskListService) : Controller
    {
        private readonly TaskListManipulationService _taskListService = taskListService;

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<TaskListModel>>> GetAll()
        {
            var result = await _taskListService.GetAllTaskList();
            return Ok(result);
        }

        [HttpGet("get/{id:int}")]
        public async Task<ActionResult<TaskModel>> GetTaskListById(int id)
        {
            var result = await _taskListService.GetTaskListAsync(id);
            return (result is not null) ? Ok(result) : StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpGet("get/{id:int}/tasks")]
        public async Task<ActionResult<IEnumerable<TaskListModel>>> GetTasks(int id)
        {
            var result = await _taskListService.GetTasksByTaskList(id);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddTaskList(TaskListInputModel input)
        {
            var isSuccess = await _taskListService.AddTaskListAsync(input) > 0;
            return Ok(new { isSuccess });
        }

        [HttpPut("update/{id:int}")]
        public async Task<ActionResult> UpdateTaskList(int id, TaskListInputModel input)
        {
            var isSuccess = await _taskListService.UpdateTaskListAsync(id, input) > 0;
            return Ok(new { isSuccess });
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<ActionResult> DeleteTaskList(int id)
        {
            var isSuccess = await _taskListService.DeleteTaskListAsync(id) > 0;
            return Ok(new { isSuccess });
        }
    }
}
