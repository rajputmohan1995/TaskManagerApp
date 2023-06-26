using System.Collections.Generic;
using TaskManagerApp.Data.Entities;

namespace TaskManagerApp.Data.Services.Interface
{
    public interface ITaskService
    {
        // Add all methods same as ITaskRepository.cs
        // Use TaskEntity.cs as the return type
        IEnumerable<TaskEntity> GetAllTasks(TaskFilter taskFilter = TaskFilter.All);
        TaskEntity GetTaskById(int id);
        TaskEntity CreateTask(TaskEntity taskEntity);
        TaskEntity UpdateTask(int id, TaskEntity taskEntity);
        bool DeleteTaskById(int id);
        bool IsCompleted(int taskId, bool isCompleted);
    }
}
