using Microsoft.AspNetCore.Identity;

namespace juliWebApi.model;

public class MyIdentityUser : IdentityUser
{
    [PersonalData]
    public string ? firstName { get; set; }
    [PersonalData]
    public string ? lastName { get; set; }

}