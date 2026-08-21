namespace PFCWebAPP.Repositories.BackOps.Models
{
    public class UserRoleMappingModel
    {
        public int UserRoleMappingID { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public bool UserRoleMappingStatus { get; set; }
        public string UserSESA { get; set; }
    }

    public class UserRoleMappingViewModel
    {
        public string UserSESA { get; set; }
        public IList<UserRoleMappingModel> lstUserRoleMappingModel { get; set; }
    
    }
}
