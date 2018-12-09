namespace CMSSolutions.Websites.Services
{
    using CMSSolutions;
    using CMSSolutions.Websites.Entities;
    using CMSSolutions.Events;
    using CMSSolutions.Services;
    using CMSSolutions.Data;
    
    
    public interface IBannersService : IGenericService<BannerInfo, int>, IDependency
    {

    }
    
    public class BannersService : GenericService<BannerInfo, int>, IBannersService
    {
        
        public BannersService(IEventBus eventBus, IRepository<BannerInfo, int> repository) : 
                base(repository, eventBus)
        {

        }
    }
}
