function start() {
    console.log("Enter: Start()");
    $(document).ready(function () {
        console.log("Document Ready.");
        //Refrencing server function
        var DBServer = $.connection.DashBoardHub;
        console.log($.connection.DashBoardHub);
        //create update dashboard
        DBServer.client.UpdateDashBoard = function (taskTitle, taskID, projectID, userID) {
            alert("All Working!");

            //Start signalR connection
            $.connection.hub.start().done(function () {
                DBServer.server.BroadCast("test", 2, 3, 4);
            })
        }
    });
}
