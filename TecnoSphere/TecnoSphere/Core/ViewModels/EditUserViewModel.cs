using TecnoSphere.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TecnoSphere.Core.ViewModels
{
    public class EditUserViewModel
    {
        public ApplicationUser? User { get; set; }

        public IList<SelectListItem>? Roles { get; set; }
    }
}
