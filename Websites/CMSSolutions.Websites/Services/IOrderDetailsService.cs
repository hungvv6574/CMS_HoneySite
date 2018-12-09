namespace CMSSolutions.Websites.Services
{
    using CMSSolutions;
    using CMSSolutions.Websites.Entities;
    using CMSSolutions.Events;
    using CMSSolutions.Services;
    using CMSSolutions.Data;
    
    
    public interface IOrderDetailsService : IGenericService<OrderDetailsInfo, int>, IDependency
    {

    }
    
    public class OrderDetailsService : GenericService<OrderDetailsInfo, int>, IOrderDetailsService
    {
        
        public OrderDetailsService(IEventBus eventBus, IRepository<OrderDetailsInfo, int> repository) : 
                base(repository, eventBus)
        {

        }
    }
}
