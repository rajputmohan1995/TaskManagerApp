using TaskManagerApp.Data.DBEntities;
using TaskManagerApp.Data.Entities;

namespace TaskManagerApp.Data.Repository.Interfaces
{
    public interface ITaskRepository
    {
        //Generate methods for the Task table
        //Methods should include
        //GetAllTasks, GetTaskById, AddTask, UpdateTask, DeleteTask

        //Use the TblTask class to define the return type for the methods
        //Use the TblTask class to define the parameter type for the methods
        //Use the TblTask class to define the return type for the methods
        TblTask[] GetAllTasks(TaskFilter taskFilter = TaskFilter.All);
        TblTask GetTaskById(int taskId);
        TblTask AddTask(TblTask task);
        TblTask UpdateTask(TblTask task);
        bool DeleteTask(int taskId);
        bool IsCompleted(int taskId, bool isCompleted);


    }
}
