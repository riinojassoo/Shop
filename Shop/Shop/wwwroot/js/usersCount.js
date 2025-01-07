var connectionUserCount = new signalR.HubConnectionBuilder()
/*.configureLogging(signalR.LogLevel.Information)*/
    .withUrl("/hubs/userCount", signalR.HttpTransportType.WebSockets).build();

connectionUserCount.on("updateTotalViews", (value) => {
    var newCountSpan = document.getElementById("totalViewsCounter");
    newCountSpan.innerText = value.toString();
});

connectionUserCount.on("updateTotalUsers", (value) => {
    var newCountSpan = document.getElementById("totalUsersCounter");
    newCountSpan.innerText = value.toString();
});



//invoke
function newWindowLoadedOnClient() {
    connectionUserCount.invoke("NewWindowLoaded", "Käbi").then((value)=> console.log(value));
}

function fulfilled() {
    console.log("Connection to User Hub Successful");
    newWindowLoadedOnClient();
}
function rejected() {

}

connectionUserCount.start().then(fulfilled, rejected);