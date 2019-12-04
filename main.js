// Modules to control application life and create native browser window
const {app, BrowserWindow} = require('electron')
const { ConnectionBuilder } = require('electron-cgi');
const ipcRenderer = require("electron").ipcRenderer;
const path = require('path')

// Keep a global reference of the window object, if you don't, the window will
// be closed automatically when the JavaScript object is garbage collected.
let mainWindow = null;

let connection = new ConnectionBuilder()
    .connectTo("dotnet", "run", "--project", "./core/Core")
    .build();

connection.onDisconnect = () => {
  console.log("connection to DOTNET lost");
}

function createWindow () {
  // Create the browser window.
  mainWindow = new BrowserWindow({
    width: 1280,
    height: 640,
    frame: false,
    backgroundColor: '#d4d4d4',
    webPreferences: {
      preload: path.join(__dirname, 'preload.js'),
      nodeIntegration: true
    }
  });

  // and load the index.html of the app.
  mainWindow.loadFile('index.html')

  // Open the DevTools.
  //mainWindow.webContents.openDevTools()

  // Emitted when the window is closed.
  mainWindow.on('closed', function () {

  });
}

// This method will be called when Electron has finished
// initialization and is ready to create browser windows.
// Some APIs can only be used after this event occurs.
app.on('ready', createWindow)

// Quit when all windows are closed.
app.on('window-all-closed', function () {
  // On macOS it is common for applications and their menu bar
  // to stay active until the user quits explicitly with Cmd + Q
  if (process.platform !== 'darwin') app.quit()
})

app.on('activate', function () {
  // On macOS it's common to re-create a window in the app when the
  // dock icon is clicked and there are no other windows open.
  if (mainWindow === null) createWindow()
})

// In this file you can include the rest of your app's specific main process
// code. You can also put them in separate files and require them here.

setInterval(function() {
  //console.log("checking for a camera signal");
  connection.send('cameraCheck', 'timestamp', msg => {
    let displayError = true;
    let displayWarn = false;
    let cameraError = false;

    if (msg == "1") {
      displayWarn = false;
      displayError = false;
      cameraError = false;
    }
    else if (msg == "0"){
      displayWarn = false;
      displayError = true;
      cameraError = false;
    }
    else if(msg == "2"){
      displayError = false;
      displayWarn = true;
      cameraError = false;
    }
    else {
      displayWarn = false;
      displayError = false;
      cameraError = true;
    }

    // TODO: BRENDAN REMOVE THE '= false' and calculate the value if it
    // should show the error.
    console.log("displayWarn: " + displayWarn)
    console.log("displayErr: " + displayError)
    console.log("displayCamErr: " + cameraError)
    console.log(msg)
    if (displayError && !displayWarn && !cameraError) {
      mainWindow.webContents.send('cameraRestore', displayError);
      mainWindow.webContents.send('cameraError', displayError);
    }
    else if (displayWarn && !displayError && !cameraError) {
      mainWindow.webContents.send('cameraRestore', displayError);
      mainWindow.webContents.send('cameraDistance', displayWarn);
    }
    else if (cameraError && !displayWarn && !displayError) {
      mainWindow.webContents.send('cameraRestore', displayError);
      mainWindow.webContents.send('cameraConnError', cameraError);
    }
    else {
      mainWindow.webContents.send('cameraRestore', cameraError);
    }
  });
}, 1000);
