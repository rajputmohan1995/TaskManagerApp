using System;
using System.Linq;
using System.Web.Mvc;
using TaskManagerApp.Data.Entities;
using TaskManagerApp.Data.Services.Interface;

namespace TaskManagerApp.Controllers
{
    public class TaskController : Controller
    {
        // create a private property of type ITaskService
        private ITaskService _taskService;

        // create a constructor with ITaskService as parameter
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET: Task
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAll(TaskFilter taskFilter = TaskFilter.All)
        {
            // call GetAllTasks method from ITaskService
            // convert IEnumerable<TaskEntity> to List<TaskEntity>
            var tasks = _taskService.GetAllTasks(taskFilter).ToList();
            return Json(tasks, JsonRequestBehavior.AllowGet);
        }

        // GET: Task/Edit/5
        public JsonResult Get(int id)
        {
            // call GetTaskById method from ITaskService
            // convert TaskEntity to TaskEntity
            var task = _taskService.GetTaskById(id);
            return Json(task, JsonRequestBehavior.AllowGet);
        }

        // POST: Task/Create
        [HttpPost]
        public JsonResult Save(TaskEntity taskEntity)
        {
            try
            {
                // call CreateTask method from ITaskService
                // convert TaskEntity to TaskEntity

                // validate taskEntity model
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) }, JsonRequestBehavior.AllowGet);
                }

                TaskEntity savedTask = null;
                if (taskEntity.Id <= 0)
                    savedTask = _taskService.CreateTask(taskEntity);
                else
                    savedTask = _taskService.UpdateTask(taskEntity.Id, taskEntity);
                return Json(new { success = true, data = savedTask, message = "Task saved successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }

        // POST: Task/Delete/5
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                // call DeleteTask method from ITaskService
                _taskService.DeleteTaskById(id);
                return Json(new { success = true, message = "Task deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ChangeStatus(int id, bool isCompleted)
        {
            try
            {
                // call DeleteTask method from ITaskService
                _taskService.IsCompleted(id, isCompleted);
                return Json(new { success = true, message = $"Task marked as {(isCompleted ? "completed" : "incomplete")}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }

    }
}