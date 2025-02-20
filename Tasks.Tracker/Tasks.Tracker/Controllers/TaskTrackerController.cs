using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Tasks.Tracker.Application.Interfaces;
using Tasks.Tracker.Domain.Entities;

namespace Tasks.Tracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskTrackerController : ControllerBase
    {
        private readonly static string nameAssembly = Assembly.GetExecutingAssembly().GetName().Name!;
        private readonly ILogger<TaskTrackerController> _logger;
        private readonly ITaskRepository _taskRepository;

        public TaskTrackerController(ILogger<TaskTrackerController> logger, ITaskRepository taskRepository)
        {
            _logger = logger;
            _taskRepository = taskRepository;
        }

        [HttpGet]
        [Route("v1/GetAllTasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                return Ok(await _taskRepository.GetAllTasks());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameAssembly} - {MethodBase.GetCurrentMethod()} - {ex.Message}");
                return NotFound(ex);
            }            
        }

        [HttpGet("v1/GetTaskById/{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                var task = await _taskRepository.GetTaskById(id);
                return task is not null ? Ok(task) : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameAssembly} - {MethodBase.GetCurrentMethod()} - {ex.Message}");
                return NotFound(ex);
            }            
        }

        [HttpPost]
        [Route("v1/CreateTask")]
        public async Task<IActionResult> CreateTask(TaskItem task)
        {
            try
            {
                await _taskRepository.AddTask(task);
                return CreatedAtAction(nameof(GetTaskById), new {id = task.Id}, task);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameAssembly} - {MethodBase.GetCurrentMethod()} - {ex.Message}");
                return NotFound(ex);
            }
        }

        [HttpPut("v1/UpdateTask/{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskItem task)
        {
            try
            {
                if (id != task.Id) BadRequest("Mismatched task ID");
                var updatedTask = await _taskRepository.UpdateTask(task);
                return Ok(updatedTask);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameAssembly} - {MethodBase.GetCurrentMethod()} - {ex.Message}");
                return NotFound(ex);
            }
        }

        [HttpDelete("v1/DeleteTask/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                await _taskRepository.DeleteTask(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameAssembly} - {MethodBase.GetCurrentMethod()} - {ex.Message}");
                return NotFound(ex);
            }            
        }
    }
}
