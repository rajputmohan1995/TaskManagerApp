using System;
using System.Collections.Generic;
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
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
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
            return Json(new ResponseResult<List<TaskEntity>>(true, tasks), JsonRequestBehavior.AllowGet);
        }

        // GET: Task/Edit/5
        public JsonResult Get(int id)
        {
            // call GetTaskById method from ITaskService
            // convert TaskEntity to TaskEntity
            var task = _taskService.GetTaskById(id);
            if (task == null)
                return Json(new ResponseResult<TaskEntity>(false, "Task not found"), JsonRequestBehavior.AllowGet);
            return Json(new ResponseResult<TaskEntity>(true, task), JsonRequestBehavior.AllowGet);
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
                    return Json(new ResponseResult<TaskEntity>(false, ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList()), JsonRequestBehavior.AllowGet);
                }

                TaskEntity savedTask = null;
                if (taskEntity.Id <= 0)
                    savedTask = _taskService.CreateTask(taskEntity);
                else
                    savedTask = _taskService.UpdateTask(taskEntity.Id, taskEntity);

                return Json(new ResponseResult<TaskEntity>(true, savedTask, "Task saved successfully"));
            }
            catch (Exception ex)
            {
                return Json(new ResponseResult<TaskEntity>(false, ex.Message));
            }
        }

        // POST: Task/Delete/5
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                // call DeleteTask method from ITaskService
                var isDeleted = _taskService.DeleteTaskById(id);
                if (isDeleted)
                    return Json(new ResponseResult<TaskEntity>(true, null, "Task deleted successfully"));
                return Json(new ResponseResult<TaskEntity>(false, "Task not found"));
            }
            catch (Exception ex)
            {
                return Json(new ResponseResult<TaskEntity>(false, ex.Message));
            }
        }

        [HttpPost]
        public JsonResult ChangeStatus(int id, bool isCompleted)
        {
            try
            {
                // call DeleteTask method from ITaskService
                var isStatusChanged = _taskService.IsCompleted(id, isCompleted);
                if (isStatusChanged)
                    return Json(new ResponseResult<TaskEntity>(true, null, $"Task marked as {(isCompleted ? "completed" : "in-complete")}"));
                return Json(new ResponseResult<TaskEntity>(false, "Task not found"));
            }
            catch (Exception ex)
            {
                return Json(new ResponseResult<TaskEntity>(false, ex.Message));
            }
        }

    }
}