
var defaultAlertTimeOut = 5;

function showSuccessAlert(message) {
    window.notie.alert({ type: 'success', text: message, time: defaultAlertTimeOut });
}

function showInfoAlert(message) {
    window.notie.alert({ type: 'info', text: message, time: defaultAlertTimeOut });
}

function showWarningAlert(message) {
    window.notie.alert({ type: 'warning', text: message, time: defaultAlertTimeOut });
}

function showErrorAlert(message) {
    window.notie.alert({ type: 'error', text: message, time: defaultAlertTimeOut });
}

function getConfirmationBox(htmlMessage, successCallback, cancelCallback) {
    window.notie.confirm({
        text: htmlMessage,
        submitCallback: successCallback,
        cancelCallback: cancelCallback,
    });
}