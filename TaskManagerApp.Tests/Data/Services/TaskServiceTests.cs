using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using TaskManagerApp.Data.DBEntities;
using TaskManagerApp.Data.Entities;
using TaskManagerApp.Data.Repository.Interfaces;
using TaskManagerApp.Data.Services;

namespace TaskManagerApp.Tests.Data.Services
{
    [TestClass]
    public class TaskServiceTests
    {
        private Mock<ITaskRepository> _taskRepository;
        private TaskService _taskService;

        TblTask[] _taskEntries = new TblTask[]
        {
            new TblTask() { Id = 1, Name = "Task 1", Description = "Main Desc 1", IsCompleted = false },
            new TblTask() { Id = 2, Name = "Task 2", Description = "Main Desc 2", IsCompleted = false },
            new TblTask() { Id = 3, Name = "Task 3", Description = "Main Desc 3", IsCompleted = true  },
            new TblTask() { Id = 4, Name = "Task 4", Description = "Main Desc 4", IsCompleted = false },
            new TblTask() { Id = 5, Name = "Task 5", Description = "Main Desc 5", IsCompleted = true  },
        };

        public TaskServiceTests()
        {
            _taskRepository = new Mock<ITaskRepository>();
            _taskService = new TaskService(_taskRepository.Object);
        }

        [TestMethod]
        public void GetAllTasks_ShouldReturnsAllTasks_WhenFilterTypeIsAll()
        {
            // Arrange
            _taskRepository
                .Setup(x => x.GetAllTasks(TaskFilter.All))
                .Returns(_taskEntries);

            // Act
            var result = _taskService.GetAllTasks(TaskFilter.All);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count());
        }

        [TestMethod]
        public void GetAllTasks_ShouldReturnsCompletedTasks_WhenFilterTypeIsCompleted()
        {
            // Arrange
            _taskRepository
                .Setup(x => x.GetAllTasks(TaskFilter.Completed))
                .Returns(_taskEntries.Where(x => x.IsCompleted)
                                     .ToArray());

            // Act
            var result = _taskService.GetAllTasks(TaskFilter.Completed);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetAllTasks_ShouldReturnsPendingTasks_WhenFilterTypeIsUncompleted()
        {
            // Arrange
            _taskRepository
                .Setup(x => x.GetAllTasks(TaskFilter.Pending))
                .Returns(_taskEntries.Where(x => !x.IsCompleted)
                                     .ToArray());

            // Act
            var result = _taskService.GetAllTasks(TaskFilter.Pending);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void GetTaskById_ShouldReturnsTask_WhenTaskIdIsValid()
        {
            // Arrange
            _taskRepository
                .Setup(x => x.GetTaskById(1))
                .Returns(_taskEntries[0]);

            // Act
            var result = _taskService.GetTaskById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }


        [TestMethod]
        public void GetTaskById_ShouldReturnsNull_WhenTaskIdIsInvalid()
        {
            // Arrange
            _taskRepository
                .Setup(x => x.GetTaskById(0))
                .Returns(new TblTask());

            // Act
            var result = _taskService.GetTaskById(0);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Id);
        }

        [TestMethod]
        public void CreateTask_ShouldReturnsAddTask_WhenValidTaskObjectIsPassed()
        {
            // Arrange
            var taskEntity = new TaskEntity()
            {
                Id = 6,
                Name = "Task 6",
                Description = "Main Description 6",
                IsCompleted = false
            };
            
            _taskRepository
                .Setup(x => x.AddTask(It.IsAny<TblTask>()))
                .Returns((new TblTask()).ToTblTask(taskEntity));

            // Act
            var result = _taskService.CreateTask(taskEntity);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(taskEntity.Id, result.Id);
        }

        [TestMethod]
        public void UpdateTask_ShouldReturnsUpdatedTask_WhenValidTaskObjectIsPassed()
        {
            // Arrange
            var taskEntity = new TaskEntity()
            {
                Id = 1,
                Name = "Task 1",
                Description = "Main Description 1",
                IsCompleted = true
            };

            _taskRepository
                .Setup(x => x.UpdateTask(It.IsAny<TblTask>()))
                .Returns((new TblTask()).ToTblTask(taskEntity));

            // Act
            var result = _taskService.UpdateTask(taskEntity.Id, taskEntity);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(taskEntity.Id, result.Id);
        }

        [TestMethod]
        public void UpdateTask_ShouldReturnEmptyObject_WhenInvalidTaskObjectIsPassed()
        {
            // Arrange
            var taskEntity = new TaskEntity()
            {
                Id = 0,
                Name = "Task 0",
                Description = "Main Description 0",
                IsCompleted = true
            };

            _taskRepository
                .Setup(x => x.UpdateTask(It.IsAny<TblTask>()))
                .Returns((new TblTask()).ToTblTask(taskEntity));

            // Act
            var result = _taskService.UpdateTask(taskEntity.Id, taskEntity);

            // Arrange
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Id);
        }

        [TestMethod]
        public void DeleteTask_ShouldReturnsTrue_WhenTaskIdIsValid()
        {
            // Arrange
            _taskRepository
                .Setup(x => x.DeleteTask(1))
                .Returns(true);

            // Act
            var result = _taskService.DeleteTaskById(1);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DeleteTask_ShouldReturnsFalse_WhenTaskIdIsInvalid()
        {
            // Arrange
            _taskRepository
                .Setup(x => x.DeleteTask(0))
                .Returns(false);

            // Act
            var result = _taskService.DeleteTaskById(0);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsCompleted_ShouldReturnsTrue_WhenTaskIdAndIsCompletedTrueIsPassed()
        {
            // Arrange
            _taskRepository
                .Setup(x => x.IsCompleted(1, true))
                .Returns(true);

            // Act
            var result = _taskService.IsCompleted(1, true);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsCompleted_ShouldReturnsTrue_WhenTaskIdAndIsCompletedFalseIsPassed()
        {
            // Arrange
            _taskRepository
                .Setup(x => x.IsCompleted(1, false))
                .Returns(true);

            // Act
            var result = _taskService.IsCompleted(1, false);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsCompleted_ShouldReturnsFalse_WhenInvalidTaskIdIsPassed()
        {
            // Arrange
            _taskRepository
                .Setup(x => x.IsCompleted(0, false))
                .Returns(false);

            // Act
            var result = _taskService.IsCompleted(0, false);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
