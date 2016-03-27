using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityManagementService.entity
{
    /// <summary>
    /// 问卷调查和活动报名的设置
    /// </summary>
    public class DBServer
    {
        private Boolean _used;

        public Boolean Used
        {
            get { return _used; }
            set { _used = value; }
        }

        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _ip;

        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }
        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
    }
}
