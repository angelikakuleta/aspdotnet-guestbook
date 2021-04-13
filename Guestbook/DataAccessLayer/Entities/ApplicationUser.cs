using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }
    }
}
