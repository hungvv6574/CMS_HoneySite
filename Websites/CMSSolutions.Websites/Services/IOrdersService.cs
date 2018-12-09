using System.Linq;

namespace CMSSolutions.Websites.Services
{
    using CMSSolutions;
    using CMSSolutions.Websites.Entities;
    using CMSSolutions.Events;
    using CMSSolutions.Services;
    using CMSSolutions.Data;
    
    
    public interface IOrdersService : IGenericService<OrderInfo, int>, IDependency
    {
        OrderInfo GetByOrderCode(string orderCode);
    }
    
    public class OrdersService : GenericService<OrderInfo, int>, IOrdersService
    {
        
        public OrdersService(IEventBus eventBus, IRepository<OrderInfo, int> repository) : 
                base(repository, eventBus)
        {

        }

        protected override System.Linq.IOrderedQueryable<OrderInfo> MakeDefaultOrderBy(System.Linq.IQueryable<OrderInfo> queryable)
        {
            return queryable.OrderBy(x => x.IsDeliverySuccessful);
        }

        public OrderInfo GetByOrderCode(string orderCode)
        {
            return Repository.Table.FirstOrDefault(x => x.OrderCode == orderCode);
        }
    }
}
