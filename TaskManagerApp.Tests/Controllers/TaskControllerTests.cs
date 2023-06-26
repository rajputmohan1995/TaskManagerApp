using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManagerApp.Controllers;
using TaskManagerApp.Data.Services.Interface;
using System.Web.Mvc;
using TaskManagerApp.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagerApp.Tests.Controllers
{
    [TestClass]
    public class TaskControllerTests
    {
        public readonly TaskController _taskController;
        private readonly Mock<ITaskService> _taskServiceMock;
        List<TaskEntity> _taskEntries = new List<TaskEntity>()
        {
            new TaskEntity() { Id = 1, Name = "Task 1", Description = "Main Desc 1", IsCompleted = false },
            new TaskEntity() { Id = 2, Name = "Task 2", Description = "Main Desc 2", IsCompleted = false },
            new TaskEntity() { Id = 3, Name = "Task 3", Description = "Main Desc 3", IsCompleted = true  },
            new TaskEntity() { Id = 4, Name = "Task 4", Description = "Main Desc 4", IsCompleted = false },
            new TaskEntity() { Id = 5, Name = "Task 5", Description = "Main Desc 5", IsCompleted = true  },
        };

        ResponseResult<TaskEntity> taskResponseResult = new ResponseResult<TaskEntity>();
        ResponseResult<List<TaskEntity>> taskListResponseResult = new ResponseResult<List<TaskEntity>>();

        public TaskControllerTests()
        {
            _taskServiceMock = new Mock<ITaskService>();
            _taskController = new TaskController(_taskServiceMock.Object);
        }

        [TestMethod]
        public void Index_ShouldReturnViewResult()
        {
            // Arrange

            // Act
            var viewResult = _taskController.Index();

            // Assert
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(typeof(ViewResult), viewResult.GetType());
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllTasks_WhenAllTasksAreRequested()
        {
            // Arrange
            var taskFilter = TaskFilter.All;
            _taskServiceMock
                .Setup(x => x.GetAllTasks(taskFilter))
                .Returns(_taskEntries);

            // Act
            var getAllTaskResult = _taskController.GetAll(taskFilter);

            // Assert
            Assert.IsNotNull(getAllTaskResult);
            Assert.AreEqual(typeof(JsonResult), getAllTaskResult.GetType());
            
            taskListResponseResult = taskListResponseResult.GetResponseResult(getAllTaskResult.Data);
            Assert.AreEqual(true, taskListResponseResult.Success);
            Assert.AreEqual(5, taskListResponseResult.Data.Count);
        }

        [TestMethod]
        public void GetAll_ShouldReturnCompletedTasks_WhenCompletedTasksAreRequested()
        {
            // Arrange
            var taskFilter = TaskFilter.Completed;
            _taskServiceMock
                .Setup(x => x.GetAllTasks(taskFilter))
                .Returns(_taskEntries.Where(t => t.IsCompleted));

            // Act
            var getCompletedTaskResult = _taskController.GetAll(taskFilter);

            // Assert
            Assert.IsNotNull(getCompletedTaskResult);
            Assert.AreEqual(typeof(JsonResult), getCompletedTaskResult.GetType());

            taskListResponseResult = taskListResponseResult.GetResponseResult(getCompletedTaskResult.Data);
            Assert.AreEqual(true, taskListResponseResult.Success);
            Assert.AreEqual(2, taskListResponseResult.Data.Count);
        }

        [TestMethod]
        public void GetAll_ShouldReturnRemainingTasks_WhenPendingTasksAreRequested()
        {
            // Arrange
            var taskFilter = TaskFilter.Pending;
            _taskServiceMock
                .Setup(x => x.GetAllTasks(taskFilter))
                .Returns(_taskEntries.Where(t => !t.IsCompleted));

            // Act
            var getPendingTaskResult = _taskController.GetAll(taskFilter);

            // Assert
            Assert.IsNotNull(getPendingTaskResult);
            Assert.AreEqual(typeof(JsonResult), getPendingTaskResult.GetType());

            taskListResponseResult = taskListResponseResult.GetResponseResult(getPendingTaskResult.Data);
            Assert.AreEqual(true, taskListResponseResult.Success);
            Assert.AreEqual(3, taskListResponseResult.Data.Count);
        }

        [TestMethod]
        public void Get_ShouldReturnTaskById_WhenValidTaskIdPassed()
        {
            // Arrange
            var taskId = 1;
            _taskServiceMock
                .Setup(x => x.GetTaskById(taskId))
                .Returns(_taskEntries.FirstOrDefault(t => t.Id == taskId));

            // Act
            var getTaskResult = _taskController.Get(taskId);

            // Assert
            Assert.IsNotNull(getTaskResult);
            Assert.AreEqual(typeof(JsonResult), getTaskResult.GetType());

            taskResponseResult = taskResponseResult.GetResponseResult(getTaskResult.Data);
            Assert.AreEqual(true, taskResponseResult.Success);
            Assert.AreEqual(taskId, taskResponseResult.Data.Id);
        }

        [TestMethod]
        public void Get_ShouldReturnErrorMessage_WhenInvalidTaskIdPassed()
        {
            // Arrange
            var taskId = 0;
            _taskServiceMock
                .Setup(x => x.GetTaskById(taskId))
                .Returns(_taskEntries.FirstOrDefault(t => t.Id == taskId));

            // Act
            var getTaskResult = _taskController.Get(taskId);

            // Assert
            Assert.IsNotNull(getTaskResult);
            Assert.AreEqual(typeof(JsonResult), getTaskResult.GetType());

            taskResponseResult = taskResponseResult.GetResponseResult(getTaskResult.Data);
            Assert.AreEqual(false, taskResponseResult.Success);
            Assert.AreEqual("Task not found", taskResponseResult.ErrorMessage);
        }

        [TestMethod]
        public void Save_ShouldReturnRequiredValidationErrors_WhenTaskNameAndDescriptionAreNotPassed()
        {
            // Arrange
            var taskNameRequiredErrorMessage = "Task name is required";
            var taskDescriptionRequiredErrorMessage = "Task description is required";
            var newTask = new TaskEntity() { Id = 0, Name = null, Description = null, IsCompleted = false };
            _taskController.ModelState.AddModelError("Name", taskNameRequiredErrorMessage);
            _taskController.ModelState.AddModelError("Description", taskDescriptionRequiredErrorMessage);

            // Act
            var saveTaskResult = _taskController.Save(newTask);

            // Assert
            Assert.IsNotNull(saveTaskResult);
            Assert.AreEqual(typeof(JsonResult), saveTaskResult.GetType());

            taskResponseResult = taskResponseResult.GetResponseResult(saveTaskResult.Data);
            Assert.AreEqual(false, taskResponseResult.Success);
            Assert.AreEqual(2, taskResponseResult.Errors.Count);
            Assert.AreEqual(taskNameRequiredErrorMessage, taskResponseResult.Errors.FirstOrDefault());
            Assert.AreEqual(taskDescriptionRequiredErrorMessage, taskResponseResult.Errors.Skip(1).FirstOrDefault());
        }

        [TestMethod]
        public void Save_ShouldReturnLengthValidationErrors_WhenInvalidLengthTaskNameAndDescriptionArePassed()
        {
            // Arrange
            var taskNameLengthErrorMessage = "Task title with 2-100 characters allowed";
            var taskDescriptionLengthErrorMessage = "Task description with 10-500 characters allowed";
            var newTask = new TaskEntity() { Id = 0, Name = "a", Description = "> 10 chars", IsCompleted = false };
            _taskController.ModelState.AddModelError("Name", taskNameLengthErrorMessage);
            _taskController.ModelState.AddModelError("Description", taskDescriptionLengthErrorMessage);

            // Act
            var saveTaskResult = _taskController.Save(newTask);

            // Assert
            Assert.IsNotNull(saveTaskResult);
            Assert.AreEqual(typeof(JsonResult), saveTaskResult.GetType());

            taskResponseResult = taskResponseResult.GetResponseResult(saveTaskResult.Data);
            Assert.AreEqual(false, taskResponseResult.Success);
            Assert.AreEqual(2, (taskResponseResult.Errors).Count);
            Assert.AreEqual(taskNameLengthErrorMessage, taskResponseResult.Errors.FirstOrDefault());
            Assert.AreEqual(taskDescriptionLengthErrorMessage, taskResponseResult.Errors.Skip(1).FirstOrDefault());
        }

        [TestMethod]
        public void Save_ShouldCreateNewTaskAndReturnSuccessResponse_WhenValidTaskDetailsArePassed()
        {
            // Arrange
            var newTask = new TaskEntity() { Id = 0, Name = "New Task", Description = "New Task Description", IsCompleted = false };
            _taskServiceMock
                .Setup(x => x.CreateTask(newTask))
                .Returns(newTask);

            // Act
            var createTaskResult = _taskController.Save(newTask);

            // Assert
            Assert.IsNotNull(createTaskResult);
            Assert.AreEqual(typeof(JsonResult), createTaskResult.GetType());

            taskResponseResult = taskResponseResult.GetResponseResult(createTaskResult.Data);
            Assert.AreEqual(true, taskResponseResult.Success);
            Assert.AreEqual(newTask.Name, taskResponseResult.Data.Name);
            Assert.AreEqual("Task saved successfully", taskResponseResult.Message);
        }

        [TestMethod]
        public void Save_ShouldUpdateTaskAndReturnSuccessResponse_WhenValidTaskDetailsArePassed()
        {
            // Arrange
            var updatedTask = new TaskEntity() { Id = 1, Name = "Updated Task", Description = "Updated Task Description", IsCompleted = false };
            _taskServiceMock
                .Setup(x => x.UpdateTask(updatedTask.Id, updatedTask))
                .Returns(updatedTask);

            // Act
            var updateTaskResult = _taskController.Save(updatedTask);

            // Assert   
            Assert.IsNotNull(updateTaskResult);
            Assert.AreEqual(typeof(JsonResult), updateTaskResult.GetType());

            taskResponseResult = taskResponseResult.GetResponseResult(updateTaskResult.Data);
            Assert.AreEqual(true, taskResponseResult.Success);
            Assert.AreEqual(updatedTask.Id, taskResponseResult.Data.Id);
            Assert.AreEqual("Task saved successfully", taskResponseResult.Message);
        }

        [TestMethod]
        public void Delete_ShouldReturnErrorMessage_WhenInvalidTaskIdPassed()
        {
            // Arrange
            var taskId = 0;
            _taskServiceMock
                .Setup(x => x.DeleteTaskById(taskId))
                .Returns(false);

            // Act
            var deleteTaskResult = _taskController.Delete(taskId);

            // Assert
            Assert.IsNotNull(deleteTaskResult);
            Assert.AreEqual(typeof(JsonResult), deleteTaskResult.GetType());

            taskResponseResult = taskResponseResult.GetResponseResult(deleteTaskResult.Data);
            Assert.AreEqual(false, taskResponseResult.Success);
            Assert.AreEqual("Task not found", taskResponseResult.ErrorMessage);
        }

        [TestMethod]
        public void Delete_ShouldReturnValidResponse_WhenValidTaskIdPassed()
        {
            // Arrange
            var taskId = 1;
            _taskServiceMock
                .Setup(x => x.DeleteTaskById(taskId))
                .Returns(true);

            // Act
            var deleteTaskResult = _taskController.Delete(taskId);

            // Assert
            Assert.IsNotNull(deleteTaskResult);
            Assert.AreEqual(typeof(JsonResult), deleteTaskResult.GetType());

            taskResponseResult = taskResponseResult.GetResponseResult(deleteTaskResult.Data);
            Assert.AreEqual(true, taskResponseResult.Success);
            Assert.AreEqual("Task deleted successfully", taskResponseResult.Message);
        }

        [TestMethod]
        public void ChangeStatus_ShouldReturnErrorMessage_WhenInvalidTaskIdPassed()
        {
            // Arrange
            var taskId = 0;
            var isCompleted = false;
            _taskServiceMock
                .Setup(x => x.IsCompleted(taskId, isCompleted))
                .Returns(false);

            // Act
            var changeTaskStatusResult = _taskController.ChangeStatus(taskId, isCompleted);

            // Assert
            Assert.IsNotNull(changeTaskStatusResult);
            Assert.AreEqual(typeof(JsonResult), changeTaskStatusResult.GetType());

            taskResponseResult = taskResponseResult.GetResponseResult(changeTaskStatusResult.Data);
            Assert.AreEqual(false, taskResponseResult.Success);
            Assert.AreEqual("Task not found", taskResponseResult.ErrorMessage);
        }

        [TestMethod]
        public void ChangeStatus_ShouldMarkTaskAsCompletedAndReturnValidResponse_WhenValidTaskIdPassed()
        {
            // Arrange
            var taskId = 1;
            var isCompleted = true;
            _taskServiceMock
                .Setup(x => x.IsCompleted(taskId, isCompleted))
                .Returns(true);

            // Act
            var changeTaskStatusResult = _taskController.ChangeStatus(taskId, isCompleted);

            // Assert
            Assert.IsNotNull(changeTaskStatusResult);
            Assert.AreEqual(typeof(JsonResult), changeTaskStatusResult.GetType());

            taskResponseResult = taskResponseResult.GetResponseResult(changeTaskStatusResult.Data);
            Assert.AreEqual(true, taskResponseResult.Success);
            Assert.AreEqual("Task marked as completed", taskResponseResult.Message);
        }

        [TestMethod]
        public void ChangeStatus_ShouldMarkTaskAsInCompleteAndReturnValidResponse_WhenValidTaskIdPassed()
        {
            // Arrange
            var taskId = 1;
            var isCompleted = false;
            _taskServiceMock
                .Setup(x => x.IsCompleted(taskId, isCompleted))
                .Returns(true);

            // Act
            var changeTaskStatusResult = _taskController.ChangeStatus(taskId, isCompleted);

            // Assert
            Assert.IsNotNull(changeTaskStatusResult);
            Assert.AreEqual(typeof(JsonResult), changeTaskStatusResult.GetType());

            taskResponseResult = taskResponseResult.GetResponseResult(changeTaskStatusResult.Data);
            Assert.AreEqual(true, taskResponseResult.Success);
            Assert.AreEqual("Task marked as in-complete", taskResponseResult.Message);
        }
    }
}
