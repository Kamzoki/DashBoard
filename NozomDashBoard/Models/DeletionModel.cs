using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NozomDashBoard.Models
{
    public class DeletionModel
    {
        public DateTime? m_StartingDate { get; set; }
        public DateTime? m_EndingDate { get; set; }

        public int? m_CurrentProjectID { get; set; }
        public DeletionModel(int? projectid)
        {
            m_EndingDate = DateTime.Now;
            m_CurrentProjectID = projectid;
        }
    }
}