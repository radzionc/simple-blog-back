using Blog.API.Services.Abstraction;
using Blog.API.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("login")]
        public ActionResult<AuthData> Post([FromBody]LoginViewModel model)
        {
            var authData = authService.GetAuthData(0);
            return authData;
        }

        [HttpPost("register")]
        public ActionResult<AuthData> Post([FromBody]RegisterViewModel model)
        {
            return new AuthData {
                Token = model.Email + model.Username + model.Password,
                Id = 0,
                TokenExpirationTime = 666
            };
        }
    }
}