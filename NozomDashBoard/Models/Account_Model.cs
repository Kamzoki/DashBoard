using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NozomDashBoard.Models
{
    public class Account_Model
    {
        //This model is used in to carry the authentication data
        private NozomDashBoardEntities db = new NozomDashBoardEntities();

        public SelectList m_UserName { get; set; }
        public int m_UserNameResult { get; set; }
        public string m_PassWord { get; set; }

        public Account_Model()
        {
            var item = db.Users.ToList();
            m_UserName = new SelectList(item, "id", "UserName");

        }
    }
}