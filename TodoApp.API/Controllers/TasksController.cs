using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Core.Common;
using TodoApp.Services.DTOs.Task;
using TodoApp.Services.Interfaces;

namespace TodoApp.API.Controllers
{
    [Authorize]
    public class TasksController : BaseApiController
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _taskService.GetByIdAsync(CurrentUserId, id, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] TaskQueryParameters query, CancellationToken cancellationToken)
        {
            var result = await _taskService.GetTasksAsync(CurrentUserId, query, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskDto dto, CancellationToken cancellationToken)
        {
            var result = await _taskService.CreateTaskAsync(CurrentUserId, dto, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, UpdateTaskDto dto, CancellationToken cancellationToken)
        {
            var result = await _taskService.UpdateTaskAsync(CurrentUserId, id, dto, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id, CancellationToken cancellationToken)
        {
            await _taskService.DeleteTaskAsync(CurrentUserId, id, cancellationToken);
            return NoContent();
        }
    }
}