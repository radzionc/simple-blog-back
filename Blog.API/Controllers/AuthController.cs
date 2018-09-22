using Blog.API.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        [HttpPost("login")]
        public ActionResult<AuthData> Post([FromBody]LoginViewModel model)
        {
            return new AuthData {
                Token = model.Email + model.Password,
                Id = 0,
                TokenExpirationTime = 666
            };
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