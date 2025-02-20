namespace Tasks.Tracker.Domain.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? State { get; set; }
        public int Father { get; set; }
    }
}
