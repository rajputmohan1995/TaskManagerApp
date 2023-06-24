var filterType = 1;

$(document).ready(function () {
    getAllTasks(filterType);
});

function getAllTasks(taskFilter) {

    var listContent = $("div.ps-content").find("ul");

    $(listContent).html("");
    $(listContent).append(`<li class="list-group-item">
                                <div class="widget-content p-0">
                                    <div class="widget-content-left flex2">
                                        <div class="widget-heading text-center">
                                            <div class="spinner-grow" role="status">
                                                <span class="sr-only">Loading...</span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </li>`);

    $.get(rootUrl + "/Task/GetAll?taskFilter=" + taskFilter,
        function (data) {

            $(listContent).html("");

            if (data && data.length > 0) {

                for (var i = 0; i < data.length; i++) {
                    var task = data[i];
                    var chkTask = "taskInput" + task.Id;

                    $(listContent).append(`<li class="list-group-item">
                                                <div class="widget-content p-0">
                                                    <div class="widget-content-wrapper">

                                                        <div class="widget-content-left mr-2">
                                                            <div class="custom-checkbox custom-control">
                                                                <input class="custom-control-input" type="checkbox" id="${chkTask}" onclick="markAsTaskCompleted(${task.Id}, '${chkTask}')" ${task.IsCompleted ? "checked" : ""} />
                                                                <label class="custom-control-label" for="${chkTask}">&nbsp;</label>
                                                            </div>
                                                        </div>

                                                        <div class="widget-content-left flex2">
                                                            <div class="widget-heading" style="text-decoration: ${task.IsCompleted ? "line-through" : "none"}">
                                                                ${task.Name}
                                                            </div>
                                                            <div class="widget-subheading" style="text-decoration: ${task.IsCompleted ? "line-through" : "none"}">
                                                                ${task.Description != null ? task.Description : ""}
                                                            </div>
                                                        </div>

                                                        <div class="widget-content-right">
                                                            <button class="border-0 btn-transition btn btn-outline-info btn-sm" onclick="editTask(${task.Id})">
                                                                <i class="fa fa-edit"></i>
                                                            </button>
                                                            <button class="border-0 btn-transition btn btn-outline-danger btn-sm" onclick="deleteTask(${task.Id})">
                                                                <i class="fa fa-trash"></i>
                                                            </button>
                                                        </div>

                                                    </div>
                                                </div>
                                            </li>`);
                }

            }
            else {
                $(listContent).append(`<li class="list-group-item">
                                            <div class="widget-content p-0">
                                                <div class="widget-content-wrapper">
                                                    <div class="widget-content-left flex2">
                                                        <div class="widget-heading text-center">
                                                            No records found.
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </li>`);
            }
        });
}

function showNewTaskSection() {
    $("#btnAddTask").hide();
    $("#divAddTaskSection").show();
}

function hideSaveTaskSection() {
    $("#btnAddTask").show();
    $("#divAddTaskSection").hide();
}

function saveTask() {

    $("#errTaskMainError").html("");

    var task = {
        Id: $("#hdnTaskId").val(),
        Name: $("#txtTitle").val(),
        Description: $("#txtDescription").val()
    };

    $.ajax({
        url: rootUrl + "/Task/Save",
        type: "POST",
        data: JSON.stringify(task),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            if (data && data.success) {
                getAllTasks(filterType);
                $("#hdnTaskId").val(0);
                $("#txtTitle").val('');
                $("#txtDescription").val('');
                showSuccessAlert(data.message);
            }
            else if (data.errors) {
                for (var i = 0; i < data.errors.length; i++) {
                    $("#errTaskMainError").append(`<span class="text-danger">${data.errors[i].ErrorMessage}</span><br />`)
                }
            }
            else if (data.errorMessage)
                $("#errTaskMainError").html(`<span class="text-danger">${data.errorMessage}</span>`);
            else
                $("#errTaskMainError").html(`<span class="text-danger">Internal error occured</span>`);

        },
        error: function (err) {
            $("#errTaskMainError").html(`<span class="text-danger">Something went wrong. Please try again later</span>`);
        }
    });
}

function editTask(taskId) {
    showNewTaskSection();

    $.get(rootUrl + "/Task/Get/" + taskId, function (data) {
        if (data) {
            $("#hdnTaskId").val(data.Id);
            $("#txtTitle").val(data.Name);
            $("#txtDescription").val(data.Description);
        }
        else showErrorAlert(data.errorMessage);
    });
}

function deleteTask(taskId) {
    getConfirmationBox('Are you sure you want to delete this task?', () => {
        $.post(rootUrl + "/Task/Delete/" + taskId, function (data) {
            if (data && data.success) {
                showInfoAlert(data.message);
                getAllTasks(filterType);
            }
            else showErrorAlert(data.errorMessage);
        });
    }, () => { })

}

function markAsTaskCompleted(taskId, chkElemId) {
    var taskCheckBox = document.getElementById(chkElemId);

    $.post(rootUrl + "/Task/ChangeStatus/" + taskId + "?isCompleted=" + taskCheckBox.checked, function (data) {
        if (data && data.success) {
            getAllTasks(filterType);
            showSuccessAlert(data.message);
        }
        else showErrorAlert(data.errorMessage);
    });
}
