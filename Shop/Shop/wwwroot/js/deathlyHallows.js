var cloakSpan = document.getElementById("cloakCounter");
var stoneSpan = document.getElementById("stoneCounter");
var wandSpan = document.getElementById("wandCounter");

var connectionDeathlyHallows = new signalR.HubConnectionBuilder()
    /*.configureLogging(signalR.LogLevel.Information)*/
    .withUrl("/hubs/deathlyhallows").build();

connectionDeathlyHallows.on("updateDeathlyHallowsCount", (cloak, stone, wand) => {
    cloakSpan.innerText = cloak.toString();
    stoneSpan.innerText = stone.toString()
    wandSpan.innerText = wand.toString()
});


function fulfilled() {
    connectionDeathlyHallows.invoke("GetRaceStatus").then((raceCounter) => {
        cloakSpan.innerText = raceCounter.cloak.toString();
        stoneSpan.innerText = raceCounter.stone.toString()
        wandSpan.innerText = raceCounter.wand.toString()
    });
    console.log("Connection to User Hub Successful");
}
function rejected() {

}

connectionDeathlyHallows.start().then(fulfilled, rejected);