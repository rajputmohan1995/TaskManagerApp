using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaskManagerApp.Data.DBEntities;
using TaskManagerApp.Data.Entities;
using TaskManagerApp.Data.Repository.Interfaces;
using TaskManagerApp.Data.Services.Interface;

namespace TaskManagerApp.Data.Services
{
    public class TaskService : ITaskService
    {
        // add private ITaskReposotiry object
        private ITaskRepository _taskRepository;

        // add constructor with ITaskRepository object as parameter
        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        // implement ITaskService methods
        public IEnumerable<TaskEntity> GetAllTasks(TaskFilter taskFilter = TaskFilter.All)
        {
            // convert all TblTask to TaskEntity
            return _taskRepository.GetAllTasks(taskFilter).Select(task => new TaskEntity().ToEntity(task));
        }

        public TaskEntity GetTaskById(int id)
        {
            // convert TblTask to TaskEntity
            return new TaskEntity().ToEntity(_taskRepository.GetTaskById(id));
        }

        public TaskEntity CreateTask(TaskEntity taskEntity)
        {
            // implement CreateTask method from ITaskRepository
            // before return convert TblTask to TaskEntity
            var tblTask = new TblTask().ToTblTask(taskEntity);
            return new TaskEntity().ToEntity(_taskRepository.AddTask(tblTask));
        }

        public TaskEntity UpdateTask(int id, TaskEntity taskEntity)
        {
            // implement UpdateTask method from ITaskRepository
            // before return convert TblTask to TaskEntity
            var tblTask = new TblTask().ToTblTask(taskEntity);
            return new TaskEntity().ToEntity(_taskRepository.UpdateTask(tblTask));
        }

        public TaskEntity DeleteTaskById(int id)
        {
            // implement DeleteTaskById method from ITaskRepository
            // before return convert TblTask to TaskEntity
            var result = _taskRepository.DeleteTask(id);
            if (result)
                return new TaskEntity().ToEntity(new TblTask());
            else
                return new TaskEntity().ToEntity(null);
        }

        public TaskEntity IsCompleted(int id, bool isCompleted)
        {
            // implement DeleteTaskById method from ITaskRepository
            // before return convert TblTask to TaskEntity
            var result = _taskRepository.IsCompleted(id, isCompleted);
            if (result)
                return new TaskEntity().ToEntity(new TblTask());
            else
                return new TaskEntity().ToEntity(null);
        }
    }
}