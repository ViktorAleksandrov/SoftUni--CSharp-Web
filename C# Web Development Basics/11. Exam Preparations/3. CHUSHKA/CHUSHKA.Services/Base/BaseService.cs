using CHUSHKA.Data;

namespace CHUSHKA.Services.Base
{
    public abstract class BaseService
    {
        protected readonly ChushkaDbContext context;

        protected BaseService(ChushkaDbContext context)
        {
            this.context = context;
        }
    }
}
