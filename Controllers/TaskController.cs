using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Text.Json.Nodes;
using ToDoListApi.Data;
using ToDoListApi.Models;
using ToDoListApi.Services;

namespace ToDoListApi.Controllers
{
    [ApiController]
    [Route("api/task")]
    public class TaskController(TaskManipulationService taskService) : Controller
    {
        private readonly TaskManipulationService _taskService = taskService;

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetTasks([FromQuery(Name = "period")] string? timePeriod, [FromQuery(Name = "is-completed")] bool? isCompleted)
        {
            var result = await _taskService.GetTasksAsync(timePeriod, isCompleted);
            return Ok(result);
        }

        [HttpGet("get/{id:int}")]
        public async Task<ActionResult<TaskModel>> GetTaskById(int id)
        {
            var result = await _taskService.GetTaskByIdAsync(id);
            return (result is not null) ? Ok(result) : StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddTask(TaskInputModel input)
        {
            var isSuccess = await _taskService.AddTaskAsync(input) > 0;
            return Ok(new { isSuccess });
        }

        [HttpPut("update/{id:int}")]
        public async Task<ActionResult> UpdateTask(int id, TaskInputModel input)
        {
            var isSuccess = await _taskService.UpdateTaskAsync(id, input) > 0;
            return Ok(new { isSuccess });
        }

        [HttpPatch("update/{id:int}/completed")]
        public async Task<ActionResult> MarkCompleted(int id)
        {
            var isSuccess = await _taskService.MarkCompletedAsync(id) > 0;
            return Ok(new { isSuccess });
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            var isSuccess = await _taskService.DeleteTaskAsync(id) > 0;
            return Ok(new { isSuccess });
        }
    }
}
