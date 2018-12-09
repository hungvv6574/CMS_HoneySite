namespace CMSSolutions.Websites.Permissions
{
    using System.Collections.Generic;
    using CMSSolutions.Web.Security.Permissions;
    
    
    public class ProductsPermissions : IPermissionProvider
    {
        
        public static readonly Permission ManagerProducts = new Permission
        {
            Name = "ManagerProducts", 
            Category = "Management", 
            Description = "Manager Products", 
        };

        
        public virtual IEnumerable<Permission> GetPermissions()
        {
            yield return ManagerProducts;
        }
    }
}
