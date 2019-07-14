using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NozomDashBoard.Models
{
    public class HomeModel
    {
        //This model deals with the data of project selection page.
        private NozomDashBoardEntities db = new NozomDashBoardEntities();
        public SelectList m_Projects { get; set; }
        public int? m_ProjectsResult { get; set; }
        public bool isAdmin { get; set; }
        public Project m_NewProject { get; set; }
        public HomeModel(bool isAdmin)
        {
            var items = db.Project.ToList();
            m_Projects = new SelectList(items, "id", "ProjectName");
            this.isAdmin = isAdmin;
        }
        public HomeModel()
        {

        }

    }
}