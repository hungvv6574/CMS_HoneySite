namespace CMSSolutions.Websites.Permissions
{
    using System.Collections.Generic;
    using CMSSolutions.Web.Security.Permissions;
    
    
    public class ProductVolumePermissions : IPermissionProvider
    {
        
        public static readonly Permission ManagerProductVolume = new Permission
        {
            Name = "ManagerProductVolume", 
            Category = "Management", 
            Description = "Manager ProductVolume", 
        };

        
        public virtual IEnumerable<Permission> GetPermissions()
        {
            yield return ManagerProductVolume;
        }
    }
}
