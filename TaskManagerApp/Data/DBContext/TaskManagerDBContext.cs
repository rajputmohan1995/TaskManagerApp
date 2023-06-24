using System.Data.Entity;
using TaskManagerApp.Data.DBEntities;

namespace TaskManagerApp.Data.DBContext
{
    //inherit from DBContext
    
    public class TaskManagerDBContext: DbContext
    {
        //Create a constructor that calls the base constructor
        public TaskManagerDBContext() : base("TaskManagerDBContext")
        {
        }
        //Create a property for the Task table
        //Use the TblTask class to define the type
        public DbSet<TblTask> Tasks { get; set; }
    }
}