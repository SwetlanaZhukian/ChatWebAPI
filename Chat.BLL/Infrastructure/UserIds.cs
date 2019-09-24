using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.BLL.Infrastructure
{
    public class UserIds
    {
        public string userId;
        public string connectionId;
        public static List<UserIds> usersList = new List<UserIds>();
    }
}
