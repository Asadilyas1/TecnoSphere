using TecnoSphere.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace TecnoSphere.Core.Repositories
{
    public interface IRoleRepository
    {
        ICollection<IdentityRole> GetRoles();
    }
}
