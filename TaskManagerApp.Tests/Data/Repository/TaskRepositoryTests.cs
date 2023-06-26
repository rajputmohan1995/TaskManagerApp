using Moq;
using Moq.EntityFramework;
using System.Linq;
using TaskManagerApp.Data.DBContext;
using TaskManagerApp.Data.DBEntities;
using TaskManagerApp.Data.Repository;
using TaskManagerApp.Tests.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManagerApp.Data.Entities;

namespace TaskManagerApp.Tests.Data.Repository
{
    [TestClass]
    public class TaskRepositoryTests
    {
        private Mock<TaskManagerDBContext> _dbContext;
        private TaskRepository _taskRepository;

        FakeDbSet<TblTask> _taskEntries = new FakeDbSet<TblTask>()
        {
            new TblTask() { Id = 1, Name = "Task 1", Description = "Main Desc 1", IsCompleted = false },
            new TblTask() { Id = 2, Name = "Task 2", Description = "Main Desc 2", IsCompleted = false },
            new TblTask() { Id = 3, Name = "Task 3", Description = "Main Desc 3", IsCompleted = true  },
            new TblTask() { Id = 4, Name = "Task 4", Description = "Main Desc 4", IsCompleted = false },
            new TblTask() { Id = 5, Name = "Task 5", Description = "Main Desc 5", IsCompleted = true  },
        };

        public TaskRepositoryTests()
        {
            _dbContext = new Mock<TaskManagerDBContext>();
            _dbContext.Setup(x => x.Set<TblTask>()).Returns(_taskEntries);
            _taskRepository = new TaskRepository(_dbContext.Object);
        }

        [TestMethod]
        public void GetAllTasks_ShouldReturnsAllTasks_WhenFilterTypeIsAll()
        {
            // Arrange

            // Act
            var result = _taskRepository.GetAllTasks(TaskFilter.All);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count());
        }

        [TestMethod]
        public void GetAllTasks_ShouldReturnsCompletedTasks_WhenFilterTypeIsCompleted()
        {
            // Arrange

            // Act
            var result = _taskRepository.GetAllTasks(TaskFilter.Completed);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }


        [TestMethod]
        public void GetAllTasks_ShouldReturnsPendingTasks_WhenFilterTypeIsPending()
        {
            // Arrange

            // Act
            var result = _taskRepository.GetAllTasks(TaskFilter.Pending);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }


        [TestMethod]
        public void GetTaskById_ShouldReturnNull_WhenInvalidTaskIdIsPassed()
        {
            // Arrange

            // Act
            var result = _taskRepository.GetTaskById(0);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetTaskById_ShouldReturnTaskObject_WhenValidTaskIdIsPassed()
        {
            // Arrange

            // Act
            var result = _taskRepository.GetTaskById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public void AddTask_ShouldAddTaskToDbSet_WhenValidTaskObjectIsPassed()
        {
            // Arrange

            var task = new TblTask() { Id = 6, Name = "Task 6", Description = "Main Description 6", IsCompleted = false };

            // Act
            var result = _taskRepository.AddTask(task);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.Id);
        }

        [TestMethod]
        public void UpdateTask_ShouldUpdateTaskToDbSet_WhenValidTaskObjectIsPassed()
        {
            // Arrange

            var task = new TblTask() { Id = 1, Name = "Task 1", Description = "Main Description 1", IsCompleted = false };

            // Act
            var result = _taskRepository.UpdateTask(task);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public void UpdateTask_ShouldReturnNull_WhenInValidTaskObjectIsPassed()
        {
            // Arrange

            var task = new TblTask() { Id = 0, Name = "Task 0", Description = "Main Description 0", IsCompleted = false };

            // Act
            var result = _taskRepository.UpdateTask(task);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DeleteTask_ShouldDeleteTaskFromDbSet_WhenValidTaskObjectIsPassed()
        {
            // Arrange

            // Act
            var result = _taskRepository.DeleteTask(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void DeleteTask_ShouldReturnNull_WhenInValidTaskIdIsPassed()
        {
            // Arrange

            // Act
            var result = _taskRepository.DeleteTask(0);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void IsCompleted_ShouldSetTaskAsCompleted_WhenValidTaskIdAndIsCompletedTrueIsPassed()
        {
            // Arrange

            // Act
            var result = _taskRepository.IsCompleted(1, true);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void IsCompleted_ShouldReturnFalse_WhenInValidTaskIdIsPassed()
        {
            // Arrange

            // Act
            var result = _taskRepository.IsCompleted(0, true);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void IsCompleted_ShouldSetTaskAsPending_WhenValidTaskIdAndIsCompletedFalseIsPassed()
        {
            // Arrange

            // Act
            var result = _taskRepository.IsCompleted(1, false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result);
        }
    }


}
