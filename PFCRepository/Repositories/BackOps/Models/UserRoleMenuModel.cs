using PFCRepository.DatabaseContext.Models.CustomTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PFCRepository.Repositories.BackOps.Models
{
    public class UserRoleMenuModel
    {
        public string UserName { get; set; }
        public string UserSESA { get; set; }
        public List<Role> lstRoles { get; set; }
        public int SelectedRoleID{ get; set; }
        public string SelectedRoleName { get; set; } = "";
        public List<Menu> lstMenus { get; set; }
        //public List<RoleMenuModel> lstRoleMenuModel { get; set; }
    }
    public class RoleMenuModel
    {
        public int RoleID { get; set; }
        public List<Menu> lstMenus { get; set; }
    }

}