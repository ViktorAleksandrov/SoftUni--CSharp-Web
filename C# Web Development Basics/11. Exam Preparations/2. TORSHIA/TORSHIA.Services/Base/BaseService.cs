using TORSHIA.Data;

namespace TORSHIA.Services.Base
{
    public abstract class BaseService
    {
        protected readonly TorshiaDbContext context;

        protected BaseService(TorshiaDbContext context)
        {
            this.context = context;
        }
    }
}
