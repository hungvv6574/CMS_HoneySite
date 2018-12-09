using System.Data.SqlClient;

namespace CMSSolutions.Websites.Services
{
    using System.Collections.Generic;
    using CMSSolutions;
    using CMSSolutions.Websites.Entities;
    using CMSSolutions.Events;
    using CMSSolutions.Services;
    using CMSSolutions.Data;
    
    
    public interface IProductsGroupService : IGenericService<ProductsGroupInfo, int>, IDependency
    {
        bool CheckAlias(int id, string alias);
    }
    
    public class ProductsGroupService : GenericService<ProductsGroupInfo, int>, IProductsGroupService
    {
        
        public ProductsGroupService(IEventBus eventBus, IRepository<ProductsGroupInfo, int> repository) : 
                base(repository, eventBus)
        {
        }

        public bool CheckAlias(int id, string alias)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@Alias", alias),
                AddInputParameter("@Id", id)
            };

            var result = (int)ExecuteReaderResult("sp_ProductsGroup_CheckAlias", list.ToArray());
            return result > 0;
        }
    }
}
