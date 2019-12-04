const ipcRenderer = require('electron').ipcRenderer;

// This file is required by the index.html file and will
// be executed in the renderer process for that window.
// No Node.js APIs are available in this process because
// `nodeIntegration` is turned off. Use `preload.js` to
// selectively enable features needed in the rendering
// process.
ipcRenderer.on('cameraError', function() {
    if (document.getElementsByClassName("notification-header.hidden").length > 0) {
        document.getElementsByClassName("notification-header.hidden")[0].className = "notification-header";
        document.getElementsByClassName("notification-header-content")[0].innerHTML = "ERROR: Camera Not Found!";
    }
});

ipcRenderer.on('cameraRestore', function() {
    if (document.getElementsByClassName("notification-header").length > 0) {
        document.getElementsByClassName("notification-header")[0].className = "notification-header.hidden";
        document.getElementsByClassName("notification-header-content")[0].innerHTML = "";
    }
});
