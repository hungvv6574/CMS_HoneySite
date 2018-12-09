namespace CMSSolutions.Websites.Permissions
{
    using System.Collections.Generic;
    using CMSSolutions.Web.Security.Permissions;
    
    
    public class StoreBranchPermissions : IPermissionProvider
    {
        
        public static readonly Permission ManagerStoreBranch = new Permission
        {
            Name = "ManagerStoreBranch", 
            Category = "Management", 
            Description = "Manager StoreBranch", 
        };

        
        public virtual IEnumerable<Permission> GetPermissions()
        {
            yield return ManagerStoreBranch;
        }
    }
}
