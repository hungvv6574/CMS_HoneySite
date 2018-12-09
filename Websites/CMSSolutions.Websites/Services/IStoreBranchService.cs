namespace CMSSolutions.Websites.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CMSSolutions;
    using CMSSolutions.Caching;
    using CMSSolutions.Websites.Entities;
    using CMSSolutions.Events;
    using CMSSolutions.Services;
    using CMSSolutions.Data;
    
    
    public interface IStoreBranchService : IGenericService<StoreBranchInfo, int>, IDependency
    {
    }
    
    public class StoreBranchService : GenericService<StoreBranchInfo, int>, IStoreBranchService
    {
        
        public StoreBranchService(IEventBus eventBus, IRepository<StoreBranchInfo, int> repository) : 
                base(repository, eventBus)
        {
        }
    }
}
