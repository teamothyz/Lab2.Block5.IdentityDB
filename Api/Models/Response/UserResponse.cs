namespace Api.Models.Response
{
    public class UserResponse
    {
        public string Email { get; set; } = null!;

        public List<string> Roles { get; set; } = new();
    }
}
