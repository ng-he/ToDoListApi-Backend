using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoListApi.Models
{
    public class TaskListInputModel
    {
        public string TaskListName { get; set; } = string.Empty;
        public int Color { get; set; }
    }

    [Table("TaskList")]
    public class TaskListModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskListId { get; set; }
        public string TaskListName { get; set; } = string.Empty;
        public int Color { get; set; }
        public int Count { get; set; } = 0;
        public ICollection<TaskModel>? TaskModels { get; set; }

        public TaskListModel() { }

        public TaskListModel(TaskListInputModel input)
        {
            TaskListName = input.TaskListName;
            Color = input.Color;
        }
    }
}
