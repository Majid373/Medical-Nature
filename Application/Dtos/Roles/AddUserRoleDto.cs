using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Roles
{
    public class AddUserRoleDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }

        public List<SelectListItem> Roles { get; set; }
    }
}
