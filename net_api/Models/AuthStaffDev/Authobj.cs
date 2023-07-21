using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace net_api.Models.AuthStaffDev
{

    public class Authen_User
    {
        public int status { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
    }

    public class ReadTokenInfo
    {
        public string uId { get; set; }
        public string ValidTo { get; set; }
        public int status { get; set; }
        public string message { get; set; }
    }

}