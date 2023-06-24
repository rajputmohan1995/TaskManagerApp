using System.ComponentModel.DataAnnotations;
using TaskManagerApp.Data.DBEntities;

namespace TaskManagerApp.Data.Entities
{
    public class TaskEntity
    {
        // all properties same as TblTask.cs

        public int Id { get; set; }
        [Required(ErrorMessage = "Task title is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Task title with 10-500 characters allowed")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Task description is required")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Task description with 10-500 characters allowed")]
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        // generate a method to convert TblTask to TaskEntity
        // use TblTask as input parameter
        public TaskEntity ToEntity(TblTask tblTask)
        {
            // check if null
            // if null then return null
            if (tblTask == null)
            {
                return null;
            }

            // assign properties one by one
            return new TaskEntity()
            {
                Id = tblTask.Id,
                Name = tblTask.Name,
                Description = tblTask.Description,
                IsCompleted = tblTask.IsCompleted
            };
        }
    }

    public enum TaskFilter
    {
        All = 1,
        Completed = 2,
        Pending = 3
    }
}