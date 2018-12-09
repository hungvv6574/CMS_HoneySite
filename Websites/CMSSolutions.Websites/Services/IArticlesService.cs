using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace CMSSolutions.Websites.Services
{
    using CMSSolutions;
    using CMSSolutions.Websites.Entities;
    using CMSSolutions.Events;
    using CMSSolutions.Services;
    using CMSSolutions.Data;
    
    
    public interface IArticlesService : IGenericService<ArticlesInfo, int>, IDependency
    {
        string LanguageCode { get; set; }

        int CategoryId { get; set; }

        bool CheckAlias(int id, string alias, string languageCode);

        ArticlesInfo GetByAlias(string alias, string languageCode);

        List<ArticlesInfo> SearchPaged(string searchText, int pageIndex, int pageSize, out int totalRecord);

        ArticlesInfo GetByCategoryId(int categoryId);
    }
    
    public class ArticlesService : GenericService<ArticlesInfo, int>, IArticlesService
    {
        public string LanguageCode { get; set; }

        public int CategoryId { get; set; }

        public ArticlesService(IEventBus eventBus, IRepository<ArticlesInfo, int> repository) : 
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

            var result = (int)ExecuteReaderResult("sp_Articles_CheckAlias", list.ToArray());
            return result > 0;
        }

        public ArticlesInfo GetByAlias(string alias, string languageCode)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@Alias", alias),
                AddInputParameter("@LanguageCode", languageCode)
            };

            return ExecuteReaderRecord<ArticlesInfo>("sp_Articles_GetByAlias", list.ToArray());
        }

        public List<ArticlesInfo> SearchPaged(string searchText, int pageIndex, int pageSize, out int totalRecord)
        {
            var list = new List<SqlParameter>
            {
                AddInputParameter("@SearchText", searchText),
                AddInputParameter("@CategoryId", CategoryId),
                AddInputParameter("@LanguageCode", LanguageCode),
                AddInputParameter("@PageIndex", pageIndex),
                AddInputParameter("@PageSize", pageSize)
            };

            return ExecuteReader<ArticlesInfo>("sp_Articles_Search_Paged", "@TotalRecord", out totalRecord, list.ToArray());
        }

        public ArticlesInfo GetByCategoryId(int categoryId)
        {
            if (Repository.Table.Any())
            {
                return Repository.Table.FirstOrDefault(x => x.LanguageCode == LanguageCode && x.CategoryId == categoryId && x.IsPublished && !x.IsDeleted);
            }

            return  null;
        }
    }
}
