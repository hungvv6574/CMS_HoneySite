namespace CMSSolutions.Websites.Services
{
    using CMSSolutions;
    using CMSSolutions.Websites.Entities;
    using CMSSolutions.Events;
    using CMSSolutions.Services;
    using CMSSolutions.Data;


    public interface IProductVolumeService : IGenericService<ProductVolumeInfo, int>, IDependency
    {

    }
    
    public class ProductVolumeService : GenericService<ProductVolumeInfo, int>, IProductVolumeService
    {
        
        public ProductVolumeService(IEventBus eventBus, IRepository<ProductVolumeInfo, int> repository) : 
                base(repository, eventBus)
        {
        }
    }
}
