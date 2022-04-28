using System.Collections.Generic;
//using NodeManager.Domain;
using NodeManager.Web.DBInfrastucture;
using NodeManager.Web.Abstract;

namespace NodeManager.Web.Models
{
    public class AllUsersModel : IUser
    {
        public string UserName { get; set; }
        public bool IsLogin { get; set; }
        public List<Users> Users { get; set; }

    }
}
