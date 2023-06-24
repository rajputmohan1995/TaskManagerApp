using System.Linq;
using TaskManagerApp.Data.DBContext;
using TaskManagerApp.Data.DBEntities;
using TaskManagerApp.Data.Entities;
using TaskManagerApp.Data.Repository.Interfaces;

namespace TaskManagerApp.Data.Repository
{
    public class TaskRepository : ITaskRepository
    {
        // implement the ITaskRepository interface

        // create a private field of type TaskManagerDBContext
        // create a constructor that initializes the private field
        private TaskManagerDBContext _context;
        // create a constructor that initializes the private field
        public TaskRepository(TaskManagerDBContext context)
        {
            _context = context;
        }

        public TblTask[] GetAllTasks(TaskFilter taskFilter = TaskFilter.All)
        {
            //implement the GetAllTasks method
            //return all tasks from the Task table
            //use the ToArray method to convert the DbSet to an array
            var tasks = new TblTask[] { };
            if (taskFilter == TaskFilter.All)
                tasks = _context.Tasks.OrderBy(t => t.IsCompleted).ThenByDescending(t => t.Id).ToArray();
            else if (taskFilter == TaskFilter.Completed)
                tasks = _context.Tasks.Where(t => t.IsCompleted == true).OrderByDescending(t => t.Id).ToArray();
            else if (taskFilter == TaskFilter.Pending)
                tasks = _context.Tasks.Where(t => t.IsCompleted == false).OrderByDescending(t => t.Id).ToArray();

            return tasks;
        }
        public TblTask GetTaskById(int taskId)
        {
            //implement the GetTaskById method
            //return a task from the Task table by Id
            //use the FirstOrDefault method to return the first task that matches the Id
            bool taskExists = _context.Tasks.Any(t => t.Id == taskId);
            if (taskExists)
            {
                return _context.Tasks.FirstOrDefault(t => t.Id == taskId);
            }
            else
            {
                return null;
            }
        }
        public TblTask AddTask(TblTask task)
        {
            //implement the AddTask method
            //add a task to the Task table
            //use the Add method to add the task to the DbSet
            //use the SaveChanges method to save the changes to the database
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return task;
        }
        public TblTask UpdateTask(TblTask task)
        {
            //implement the UpdateTask method
            //update a task in the Task table
            //use the Attach method to attach the task to the DbSet
            //use the SaveChanges method to save the changes to the database
            var existingTask = _context.Tasks.FirstOrDefault(t => t.Id == task.Id);
            if (existingTask == null)
                return null;

            existingTask.Name = task.Name;
            existingTask.Description = task.Description;
            existingTask.IsCompleted = task.IsCompleted;

            _context.SaveChanges();
            return task;
        }
        public bool DeleteTask(int taskId)
        {
            //implement the DeleteTask method
            //delete a task from the Task table
            //use the FirstOrDefault method to return the first task that matches the Id
            //use the Remove method to remove the task from the DbSet
            //use the SaveChanges method to save the changes to the database
            bool taskExists = _context.Tasks.Any(t => t.Id == taskId);
            if (taskExists)
            {
                TblTask task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
                _context.Tasks.Remove(task);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsCompleted(int taskId, bool isCompleted)
        {
            bool taskExists = _context.Tasks.Any(t => t.Id == taskId);
            if (taskExists)
            {
                TblTask task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
                task.IsCompleted = isCompleted;
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}