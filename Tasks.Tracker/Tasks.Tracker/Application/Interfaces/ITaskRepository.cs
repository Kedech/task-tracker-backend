using Tasks.Tracker.Domain.Entities;

namespace Tasks.Tracker.Application.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllTasks();
        Task<TaskItem> GetTaskById(int id);
        Task AddTask(TaskItem task);
        Task<TaskItem> UpdateTask(TaskItem task);
        Task DeleteTask(int id);
    }
}
