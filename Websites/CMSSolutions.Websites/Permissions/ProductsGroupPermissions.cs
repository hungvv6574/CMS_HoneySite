namespace CMSSolutions.Websites.Permissions
{
    using System.Collections.Generic;
    using CMSSolutions.Web.Security.Permissions;
    
    
    public class ProductsGroupPermissions : IPermissionProvider
    {
        
        public static readonly Permission ManagerProductsGroup = new Permission
        {
            Name = "ManagerProductsGroup", 
            Category = "Management", 
            Description = "Manager ProductsGroup", 
        };

        
        public virtual IEnumerable<Permission> GetPermissions()
        {
            yield return ManagerProductsGroup;
        }
    }
}
