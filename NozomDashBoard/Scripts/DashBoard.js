function CheckServerState(timer) {
    window.setInterval(function () {
        console.log("Checking Server State");
        $(document).ready(function () {
            $.post("/DashBoard/UpdateClient", function (data) {
                if (data.isOutDated != "NULL" && data.isOutDated == "true") {
                    location.reload(true);
                }
            })
        });
    }, timer);

}
