using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoListApi.Models
{
    public class TaskInputModel
    {
        public string TaskTitle { get; set; } = string.Empty;

        public string? TaskDescription { get; set; }

        public int? TaskListId { get; set; }

        public DateTime? DueDate { get; set; } = DateTime.Today;

        public bool IsCompleted { get; set; } = false;
    }


    [Table("Task")]
    public class TaskModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }

        public string TaskTitle { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        public string? TaskDescription { get; set; }

        public int? TaskListId { get; set; }

        public DateTime? DueDate { get; set; } = DateTime.Today;

        public bool IsCompleted { get; set; } = false;

        public TaskListModel? TaskList { get; set; } = default;

        public TaskModel() { }

        public TaskModel(TaskInputModel input)
        {
            TaskTitle = input.TaskTitle;
            TaskDescription = input.TaskDescription;
            TaskListId = input.TaskListId;
            DueDate = input.DueDate;
            IsCompleted = input.IsCompleted;
        }
    }
}
