using Microsoft.AspNetCore.Identity;

namespace StudentenBeheer.Areas.Identity.Data;

public class ApplicationUser : IdentityUser
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
}

