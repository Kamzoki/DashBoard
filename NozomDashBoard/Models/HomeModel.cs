using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NozomDashBoard.Models
{
    public class HomeModel
    {
        private NozomDashBoardEntities db = new NozomDashBoardEntities();
        public SelectList m_Projects { get; set; }
        public int? m_ProjectsResult { get; set; }

        public HomeModel()
        {
            var items = db.Project.ToList();
            m_Projects = new SelectList(items, "id", "ProjectName");
        }

    }
}