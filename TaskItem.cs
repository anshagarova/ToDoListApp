   public class TaskItem
    {
        public string Description { get; set; }
        public bool IsImportant { get; set; }

        public TaskItem(string description, bool isImportant) 
        {
            Description = description;
            IsImportant = isImportant;
        }
    }