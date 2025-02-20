using System.Text.Json;
using Tasks.Tracker.Application.Interfaces;
using Tasks.Tracker.Domain.Entities;

namespace Tasks.Tracker.Infrastructure.Persistence
{
    public class TaskRepository : ITaskRepository
    {
        private readonly static string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "tasks.json");

        List<TaskItem> _tasks;
        public TaskRepository() 
        { 
            _tasks = LoadTasksFromFile();
        }

        private static List<TaskItem> LoadTasksFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return new List<TaskItem>();
            }
            string json = File.ReadAllText(_filePath);

            return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        }

        private void SaveTasksToFile()
        {
            string json = JsonSerializer.Serialize(_tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public async Task AddTask(TaskItem task)
        {
            if (task == null)           
                throw new NotImplementedException();

            int taskId = 1;

            if(_tasks.Count > 0)
            {
                taskId = _tasks.LastOrDefault() == null ? taskId : _tasks.LastOrDefault()!.Id + 1;
            }
            task.Id = taskId;
            _tasks.Add(task);
            SaveTasksToFile();
            await Task.CompletedTask;
        }

        public async Task DeleteTask(int id)
        {
            _tasks.RemoveAll(task => task.Id == id);
            SaveTasksToFile();
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasks()
        {
            return await Task.FromResult( _tasks);
        }

        public async Task<TaskItem> GetTaskById(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);

            return await Task.FromResult(task ?? new TaskItem());
        }

        public async Task<TaskItem> UpdateTask(TaskItem task)
        {
            var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
            if (existingTask != null)
            {
                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.State = task.State;
                existingTask.Father = task.Father;
                SaveTasksToFile();
                return await Task.FromResult(existingTask);
            }
            throw new KeyNotFoundException($"Task with ID {task.Id} not found.");
        }
    }
}
