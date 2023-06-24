using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TaskManagerApp.Data.Entities;

namespace TaskManagerApp.Data.DBEntities
{
    public class TblTask
    {
        //Generate properties for the Task table
        //Properties should include
        //Id, Name, Description, IsCompleted
        //All properties should be public

        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [DefaultValue(false)]
        public bool IsCompleted { get; set; }

        // generate a method to convert TaskEntity to TblTask
        public TblTask ToTblTask(TaskEntity taskEntity)
        {
            // check if null
            // if null then return null
            if (taskEntity == null)
            {
                return null;
            }

            // assign properties one by one
            return new TblTask()
            {
                Id = taskEntity.Id,
                Name = taskEntity.Name,
                Description = taskEntity.Description,
                IsCompleted = taskEntity.IsCompleted
            };
        }
    }
}