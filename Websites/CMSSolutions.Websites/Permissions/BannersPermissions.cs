namespace CMSSolutions.Websites.Permissions
{
    using System.Collections.Generic;
    using CMSSolutions.Web.Security.Permissions;
    
    
    public class BannersPermissions : IPermissionProvider
    {
        
        public static readonly Permission ManagerBanners = new Permission
        {
            Name = "ManagerBanners", 
            Category = "Management", 
            Description = "Manager Banners", 
        };

        
        public virtual IEnumerable<Permission> GetPermissions()
        {
            yield return ManagerBanners;
        }
    }
}
