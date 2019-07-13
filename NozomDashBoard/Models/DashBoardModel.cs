using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NozomDashBoard.Models
{
    public class DashBoardModel
    {
        //This model is used to handle the data posted and recieved in the dashboard.

        private NozomDashBoardEntities db = new NozomDashBoardEntities();

        public SelectList m_DashBoardType { get; set; }
        public int? m_DashBoardTypeResult { get; set; }

        public SelectList m_Users { get; set; }
        public int? m_UsersResult { get; set; }

        public SelectList m_Projects { get; set; }
        public int? m_ProjectResult { get; set; }

        public DashBoardData m_Task { get; set; }
        public List<int?> m_FinishedTasks { get; set; }
        public List<EquireTasks_Result> m_AllTasks { get; set; }

        public int? m_CurrentProjectID { get; set; }

        public bool? isAccissable { get; set; }
        public bool isAdmin { get; set; }

        public DashBoardModel(int? ProjectID, bool op)
        {
            if (ProjectID != null)
            {
                m_CurrentProjectID = ProjectID;
            }
            AquireSelectLists(op);
        }

        public DashBoardModel(int? ProjectID)
        {
            if (ProjectID != null)
            {
                m_CurrentProjectID = ProjectID;
            }
            AquireSelectLists(false);
        }

        public DashBoardModel()
        {
            //This is intentionally left blank, for mvc cannot work with parameterfull constructors only.
        }

        private void AquireSelectLists(bool op)
        {
            var Useritems = db.Users.ToList();

            if (op == true)
            {
                //This is used for editing or adding a new task, for when a selectlist of users is show, it reverse it, putting the Admin as the last user, then removes it from the list so that only the clients are applicable to be assigned tasks.
                Useritems.Reverse();
                Useritems.RemoveAt(3);
            }

            m_Users = new SelectList(Useritems, "id", "UserName");

            var Projectitems = db.Project.ToList();
            m_Projects = new SelectList(Projectitems, "id", "ProjectName");
        }
    }
}