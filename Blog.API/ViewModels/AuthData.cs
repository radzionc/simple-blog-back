namespace Blog.API.ViewModels
{
    public class AuthData
    {
        public string Token { get; set; }
        public int TokenExpirationTime { get; set; }
        public int Id { get; set; }
    }
}