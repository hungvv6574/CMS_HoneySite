namespace CMSSolutions.Websites.Permissions
{
    using System.Collections.Generic;
    using CMSSolutions.Web.Security.Permissions;
    
    
    public class OrdersPermissions : IPermissionProvider
    {
        
        public static readonly Permission ManagerOrders = new Permission
        {
            Name = "ManagerOrders", 
            Category = "Management", 
            Description = "Manager Orders", 
        };

        
        public virtual IEnumerable<Permission> GetPermissions()
        {
            yield return ManagerOrders;
        }
    }
}
