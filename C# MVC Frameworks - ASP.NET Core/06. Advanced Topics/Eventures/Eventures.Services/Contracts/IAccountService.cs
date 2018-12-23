using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Eventures.Services.Contracts
{
    public interface IAccountService
    {
        Task<bool> ExternalLoginUser(ExternalLoginInfo info);
    }
}
