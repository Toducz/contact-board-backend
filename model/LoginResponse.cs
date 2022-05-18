
namespace juliWebApi.model
{
    public class LoginResponse
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public IList<String> roles { get; set; } =  new List<String>();
        public string? token { get; set; }
    }
}

/*
 number;
    firstName: string;
    lastName: string;
    username: string;
    role: Role;
    token?: string;
}*/