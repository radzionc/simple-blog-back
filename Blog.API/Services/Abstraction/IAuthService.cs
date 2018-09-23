using Blog.API.ViewModels;

namespace Blog.API.Services.Abstraction
{
    public interface IAuthService
    {
        AuthData GetAuthData(int id);
    }
}