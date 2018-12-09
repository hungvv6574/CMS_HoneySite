using System.Collections.Generic;
using System.Data.SqlClient;

namespace CMSSolutions.Websites.Services
{
    using CMSSolutions;
    using CMSSolutions.Websites.Entities;
    using CMSSolutions.Events;
    using CMSSolutions.Services;
    using CMSSolutions.Data;
    
    
    public interface IProductsService : IGenericService<ProductInfo, int>, IDependency
    {
        string LanguageCode { get; set; }

        int CategoryId { get; set; }

        bool CheckAlias(int id, string alias, string languageCode);

        List<ProductInfo> SearchPaged(string searchText, int productGroup, int pageIndex, int pageSize, out int totalRecord);
    }
    
    public class ProductsService : GenericService<ProductInfo, int>, IProductsService
    {
        public string LanguageCode { get; set; }
        public int CategoryId { get; set; }
        public ProductsService(IEventBus eventBus, IRepository<ProductInfo, int> repository) : 
                base(repository, eventBus)
        {

        }

        public bool CheckAlias(int id, string alias, string languageCode)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@Alias", alias),
                AddInputParameter("@Id", id),
                AddInputParameter("@LanguageCode", languageCode)
            };

            var result = (int)ExecuteReaderResult("sp_Products_CheckAlias", list.ToArray());
            return result > 0;
        }

        public List<ProductInfo> SearchPaged(string searchText, int productGroup, int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SearchText", searchText),
                AddInputParameter("@ProductGroup", productGroup),
                AddInputParameter("@CategoryId", CategoryId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<ProductInfo>("sp_Products_Search_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }
    }
}
