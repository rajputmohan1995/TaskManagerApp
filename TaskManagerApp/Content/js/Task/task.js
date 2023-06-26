var filterType = 1;

$(document).ready(function () {
    getAllTasks(filterType);
});

$(function () {
    $(".nav-item.nav-link").on('click', function () {
        $(".nav-item.nav-link").removeClass("active");
        $(this).addClass("active");

        filterType = $(this).attr("data-loadFor");
        getAllTasks(filterType);
    })
})

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
        function (response) {

            $(listContent).html("");

            if (response && response.Success && response.Data && response.Data.length > 0) {

                for (var i = 0; i < response.Data.length; i++) {
                    var task = response.Data[i];
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
    resetTaskForm();
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
        success: function (response) {

            if (response && response.Success) {
                getAllTasks(filterType);
                resetTaskForm();
                showSuccessAlert(response.Message);
            }
            else if (response.Errors && response.Errors.length > 0) {
                for (var i = 0; i < response.Errors.length; i++) {
                    $("#errTaskMainError").append(`<span class="text-danger">${response.Errors[i]}</span><br />`)
                }
            }
            else if (response.ErrorMessage)
                $("#errTaskMainError").html(`<span class="text-danger">${response.ErrorMessage}</span>`);
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

    $.get(rootUrl + "/Task/Get/" + taskId, function (response) {
        if (response && response.Success) {
            $("#hdnTaskId").val(response.Data.Id);
            $("#txtTitle").val(response.Data.Name);
            $("#txtDescription").val(response.Data.Description);
        }
        else showErrorAlert(response.ErrorMessage);
    });
}

function deleteTask(taskId) {
    getConfirmationBox('Are you sure you want to delete this task?', () => {
        $.post(rootUrl + "/Task/Delete/" + taskId, function (response) {
            if (response && response.Success) {
                showInfoAlert(response.Message);
                getAllTasks(filterType);
            }
            else showErrorAlert(response.ErrorMessage);
        });
    }, () => { })

}

function markAsTaskCompleted(taskId, chkElemId) {
    var taskCheckBox = document.getElementById(chkElemId);

    $.post(rootUrl + "/Task/ChangeStatus/" + taskId + "?isCompleted=" + taskCheckBox.checked, function (response) {
        if (response && response.Success) {
            getAllTasks(filterType);
            showInfoAlert(response.Message);
        }
        else showErrorAlert(response.ErrorMessage);
    });
}


function resetTaskForm() {
    $("#hdnTaskId").val(0);
    $("#txtTitle").val('');
    $("#txtDescription").val('');
    $("#errTaskMainError").html('');
}