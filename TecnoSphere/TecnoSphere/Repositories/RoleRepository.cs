using TecnoSphere.Areas.Identity.Data;
using TecnoSphere.Core.Repositories;
using Microsoft.AspNetCore.Identity;

namespace TecnoSphere.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDBContext _context;

        public RoleRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public ICollection<IdentityRole> GetRoles()
        {
            return _context.Roles.ToList();
        }
    }
}
