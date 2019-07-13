using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace NozomDashBoard.SignalR
{
    [HubName("DashBoardHub")]
    public class DashBoardHub : Hub
    {
        public void BroadCast(string taskTitle, int? taskID, int? ProjectID, int? UserID)
        {
            Clients.All.UpdateDashBoard(taskTitle, taskID, ProjectID, UserID);
        }
    }
}